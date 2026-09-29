import { redisService, appwriteService, postgresService } from "../../Routes/Services.js";
import { Query,ID } from "node-appwrite";
import { InputFile } from "node-appwrite/file";
import { Permission, Role } from "node-appwrite";
import { encryptPhone } from "./phoneCrypto.js";

// Make sure these are in your .env file
const APPWRITE_ENDPOINT = process.env.APPWRITE_ENDPOINT || "https://cloud.appwrite.io/v1";
const APPWRITE_PROJECT_ID = process.env.APPWRITE_PROJECT_ID;

export class CreateUserModel {

  static async uploadToAppwriteStorage(file, appwriteID, type) {
    if (!file) return null;
    if (Array.isArray(file)) file = file[0];

    const bucketId = type === "cover"
      ? "6931584f00341c909950"  // Cover bucket
      : "6931583c002eaef13895"; // Profile bucket

    try {
     

      const uploaded = await appwriteService.storage.createFile({
        bucketId,
        fileId: ID.unique(),
        file: InputFile.fromBuffer(file.buffer, file.originalname),
        permissions: [
          Permission.read(Role.any()),               // Anyone can view the image
          Permission.update(Role.user(appwriteID)),  // Only owner can update
          Permission.delete(Role.user(appwriteID)),  // Only owner can delete
        ],
      });

      // === CRITICAL FIX: Manually build the public view URL ===
      // Because getFileView() is currently returning binary data instead of URL
      const viewUrl = `${APPWRITE_ENDPOINT}/storage/buckets/${bucketId}/files/${uploaded.$id}/view?project=${APPWRITE_PROJECT_ID}&mode=public`;

      return viewUrl;

    } catch (err) {
      console.error(`❌ Failed to upload ${type}:`, err.message || err);
      return null;
    }
  }

  static async create(user) {
    
    const pendingKey = `user:pending:${user.AppWriteID}`;
    const finalKey = `user:${user.AppWriteID}`;

    const encryptedDOB = encryptPhone(user.DateOfBirth);
    const encryptedLocation = encryptPhone(user.Location);
    
    if (!redisService.client.isOpen) {await redisService.connect(); }
   
// --- STEP 1: Queue or update contact in Redis ---
    let attempts = 1;
    const existingAttempts = await redisService.client.hGet(pendingKey, "attempts");
    if (existingAttempts) attempts = parseInt(existingAttempts, 10) + 1;
    
  

    // Upload images and replace with URLs (strings)
    if (user.ProfileImage) {
      user.ProfileImage = await CreateUserModel.uploadToAppwriteStorage(
        user.ProfileImage,
        user.AppWriteID,
        "profile"
      );
    }

    if (user.CoverImage) {
      user.CoverImage = await CreateUserModel.uploadToAppwriteStorage(
        user.CoverImage,
        user.AppWriteID,
        "cover"
      );
    }

      await redisService.client.hSet(pendingKey, {
        FirstNames: user.FirstNames,
        LastName: user.LastName,
        Gender: user.Gender,
        AppWriteID: user.AppWriteID,
        ContactID: user.ContactID,
        DateOfBirth: encryptedDOB,
        Bio: user.Bio,
        Location: encryptedLocation,
        ProfilePictureURL: user.ProfileImage,   // Now a proper string URL
        CoverImageURL: user.CoverImage,         // Now a proper string URL
        lastError: "",
        createdAt: new Date().toISOString(),
        status: "pending",
      });

      await redisService.client.expire(pendingKey, 86400 * 7); // 7 days
      
    // --- STEP 2: Insert or update in Appwrite ---
   let appwriteDoc;
   try {
     const dbId = process.env.APPWRITE_CONTACTS_DB_ID;
     const tableId = "users";
   
     // Query Appwrite by ContactID
     const existingDocs = await appwriteService.databases.listDocuments(dbId, tableId, [
       Query.equal("ContactID",  user.AppWriteID)
     ]);
   
     if (existingDocs.total > 0) {
       // Update existing document
       const docId = existingDocs.documents[0].$id;
       appwriteDoc = await appwriteService.databases.updateDocument(
         dbId,
         tableId,
         docId,
         {
        FirstNames: user.FirstNames,
        LastName: user.LastName,
        Gender: user.Gender,
        DateOfBirth:  encryptedDOB,
        Bio: user.Bio,
        Location: encryptedLocation,
        ContactID: user.AppWriteID,
        ProfilePictureURL: user.ProfileImage,   // Now a proper string URL
        CoverImageURL: user.CoverImage,         // Now a proper string URL
         }
       );
      
     } else {
       // Create new document
       appwriteDoc = await appwriteService.databases.createDocument(
         dbId,
         tableId,
         "unique()",
         {
        FirstNames: user.FirstNames,
        LastName: user.LastName,
        Gender: user.Gender,
        DateOfBirth:  encryptedDOB,
        Bio: user.Bio,
        Location: encryptedLocation,
        ContactID: user.AppWriteID,
        ProfilePictureURL: user.ProfileImage,   // Now a proper string URL
        CoverImageURL: user.CoverImage,         // Now a proper string URL

         
         }
       );
     
     }
   
     // Update Redis status
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
       const query = `SELECT * FROM "CreateUser"($1,$2,$3,$4,$5,$6,$7,$8,$9);`;
      // let dateOfBirth = encryptedDOB;
 
       // Normalize date format for PostgreSQL
      // if (typeof dateOfBirth === "string") {
       //  dateOfBirth = new Date(dateOfBirth).toISOString().split("T")[0];
      // }
 
       const values = [
         user.FirstNames,
         user.LastName,
         user.Gender,
         encryptedDOB,
         user.Bio,
         encryptedLocation,
         user.ContactID,
         user.ProfileImage,
        user.CoverImage
       ];
 
       const pgResult = await postgresService.query(query, values);
       const userRow = pgResult.rows[0];
      
 
       // Cleanup Redis pending key
       await redisService.client.del(pendingKey);
      
 
       // Cache final contact
       const cachedData = { ...userRow, ...appwriteDoc };
       await redisService.client.set(finalKey, JSON.stringify(cachedData));
       await redisService.client.expire(finalKey, 86400); // 24h cache
      
 
       return cachedData;
     } catch (err) {
       console.error("PostgreSQL failed:", err.message);
       const newAttempts = attempts + 1;
 
       // Update Redis pending key with failure info
       await redisService.client.hSet(pendingKey, {
         status: "postgres_failed",
         lastError: err.message,
         attempts: newAttempts.toString()
       });
 
       if (newAttempts >= 10) {
         try {
           await redisService.client.rename(pendingKey, `user:dead:${user.AppWriteID}`);
           console.error("Moved to dead letter after too many failures");
         } catch (renameErr) {
           console.error("Failed to rename pending key:", renameErr.message);
         }
       }
       throw err;
     }
  }
}