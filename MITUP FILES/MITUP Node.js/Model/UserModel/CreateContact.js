// ../../Model/UserModel/CreateContact.js
import { redisService, appwriteService, postgresService } from "../../Routes/Services.js";
import { Query } from "appwrite";
import { encryptPhone, maskPhone, e164ToLocal} from "./phoneCrypto.js";

export class CreateContactModel {
  static async create(contact) {
    const localNumber = e164ToLocal(contact.ContactNumber);
    // contact.ContactNumber is raw E.164 input
    const encryptedNumber = encryptPhone(contact.ContactNumber);
    const encryptedNationalNumber = encryptPhone(contact.ContactNumber);
    const pendingKey = `contact:pending:${contact.ContactNumber}`;
    const finalKey = `contact:${contact.ContactNumber}`;
    // Ensure Redis is connected
    if (!redisService.client.isOpen) await redisService.connect();
    // --- STEP 1: Queue contact in Redis ---
    let attempts = 1;
    const existingAttempts = await redisService.client.hGet(pendingKey, "attempts");
    if (existingAttempts) attempts = parseInt(existingAttempts, 10) + 1;
    
    await redisService.client.hSet(pendingKey, {
      ContactNumber: contact.ContactNumber,
      NationalNumber: localNumber,
      Country: contact.Country,
      CountryCode: contact.CountryCode,
      status: "pending",
      attempts: attempts.toString(),
      lastError: "",
      createdAt: new Date().toISOString(),
    });
    await redisService.client.expire(pendingKey, 86400 * 7); // 7 days
  
    let appwriteDoc;
    try {
      const dbId = process.env.APPWRITE_CONTACTS_DB_ID;
      const tableId = "contacts";
      // Check if the contact already exists using encrypted value
      const existingDocs = await appwriteService.databases.listDocuments(dbId, tableId, [
        Query.equal("ContactNumber", contact.ContactNumber)
      ]);

      if (existingDocs.total > 0) {
        appwriteDoc = existingDocs.documents[0];
       
      } else {
        appwriteDoc = await appwriteService.databases.createDocument(
          dbId,
          tableId,
          "unique()",
          {
            ContactNumber: contact.ContactNumber,
            NationalNumber: localNumber,
            Country: contact.Country,
            CountryCode: contact.CountryCode,
          }
        );
       
      }
      await redisService.client.hSet(pendingKey, {
        status: "appwrite_done",
        appwriteId: appwriteDoc.$id,
        lastError: ""
      });
    } catch (err) {
      console.error("Appwrite failed:", err.message);
      await redisService.client.hSet(pendingKey, {
        status: "appwrite_failed",
        lastError: err.message
      });
      throw err;
    }

    // --- STEP 3: Insert into PostgreSQL ---
    try {
      // Pass encrypted number to the stored procedure
      const query = `SELECT "CreateContacts"($1, $2, $3,  $4) AS contactid;`;
      const values = [contact.ContactNumber, contact.Country, contact.CountryCode, localNumber];

      const pgResult = await postgresService.query(query, values);
      const contactID = pgResult.rows[0].contactid;
     
      // Cleanup Redis
      await redisService.client.del(pendingKey);
      // Cache final contact with both IDs (appwriteDoc contains encrypted ContactNumber)
      const cachedData = { ...appwriteDoc, contactID };
      await redisService.client.set(finalKey, JSON.stringify(cachedData));
      await redisService.client.expire(finalKey, 86400); // 24h cache
      // Return both IDs (note: appwriteDoc fields include encrypted ContactNumber)
      return cachedData;
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
          await redisService.client.rename(pendingKey, `contact:dead:${contact.ContactNumber}`);
          console.error("Moved to dead letter after too many failures");
        } catch (renameErr) {
          console.error("Failed to rename pending key:", renameErr.message);
        }
      }
      throw err;
    }
  }
}
