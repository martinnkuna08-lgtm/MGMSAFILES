import { redisService, appwriteService, postgresService } from "../../Routes/Services.js";
import { Query } from "appwrite";
import { encryptPhone, maskPhone } from "../UserModel/phoneCrypto.js";

export class UserCreateMessageModel {
  static async create(userMessage) {
  
   const pendingKey = `contact:pending:${userMessage.AsenderID}:${userMessage.AreceiverID}`;
   const finalKey = `contact:${userMessage.AsenderID}:${userMessage.AreceiverID}`;

    // Ensure Redis is connected
    if (!redisService.client.isOpen) await redisService.connect();

    // --- STEP 1: Queue contact in Redis ---
    let attempts = 1;
    const existingAttempts = await redisService.client.hGet(pendingKey, "attempts");
    if (existingAttempts) attempts = parseInt(existingAttempts, 10) + 1;

    await redisService.client.hSet(pendingKey, {

      AreceiverID: userMessage.AreceiverID,
      PreceiverID: userMessage.PreceiverID,
      TextMessages: userMessage.TextMessages,
       IsRead: String(userMessage.IsRead),
      AreceiverPhone: userMessage.AreceiverPhone,
      MessageType: userMessage.MessageType,
      AsenderID: userMessage.AsenderID,
      PsenderID: userMessage.PsenderID,

      status: "pending",
      attempts: attempts.toString(),
      lastError: "",
      createdAt: new Date().toISOString(),
    });
    await redisService.client.expire(pendingKey, 86400 * 7); // 7 days
   
let appwriteDoc;
try {
  const dbId = process.env.APPWRITE_CONTACTS_DB_ID;
  const tableId = "messages";

  // 1️⃣ Insert the new message
  appwriteDoc = await appwriteService.databases.createDocument(
    dbId,
    tableId,
    "unique()",
    {
      AreceiverID: userMessage.AreceiverID,
      TextMessages: userMessage.TextMessages,
       IsRead: userMessage.IsRead, // ✅ FIX
         AreceiverPhone: userMessage.AreceiverPhone,
      MessageType: userMessage.MessageType ,
      AsenderID: userMessage.AsenderID,

    }
  );

  // 3️⃣ Update Redis
  await redisService.client.hSet(pendingKey, {
    status: "appwrite_done",
    appwriteId: appwriteDoc.$id,
    lastError: "",
  });

} catch (err) {
  console.error("Appwrite failed:", err.message);
  await redisService.client.hSet(pendingKey, {
    status: "appwrite_failed",
    lastError: err.message,
  });
  throw err;
}

    // --- STEP 3: Insert into PostgreSQL ---
    try {
      // Pass encrypted number to the stored procedure
      const query = `SELECT "CreateMessages"($1, $2, $3,$4, $5, $6);`;
      const values = [userMessage.PreceiverID, userMessage.TextMessages,  userMessage.IsRead, userMessage.MessageType,userMessage.AreceiverPhone, userMessage.PsenderID];
       await postgresService.query(query, values);
    
      // Cleanup Redis
      await redisService.client.del(pendingKey);

      // Cache final contact with both IDs (appwriteDoc contains encrypted ContactNumber)
      const cachedData = { ...appwriteDoc};
      await redisService.client.set(finalKey, JSON.stringify(cachedData));
      await redisService.client.expire(finalKey, 86400); // 24h cache
     
      // Return both IDs (note: appwriteDoc fields include encrypted ContactNumber)
      return;
    } catch (err) {
      console.error("PostgreSQL failed:", err.message);
      const newAttempts = attempts + 1;
      await redisService.client.hSet(pendingKey, {
        status: "postgres_failed",
        lastError: err.message,
        attempts: newAttempts.toString()
      });
      if (newAttempts >= 10) {
        try {
          await redisService.client.rename(pendingKey, `contact:dead:${userMessage.AsenderID}:${userMessage.AreceiverID}`);
          console.error("Moved to dead letter after too many failures");
        } catch (renameErr) {
          console.error("Failed to rename pending key:", renameErr.message);
        }
      }
      throw err;
    }
  }
}
