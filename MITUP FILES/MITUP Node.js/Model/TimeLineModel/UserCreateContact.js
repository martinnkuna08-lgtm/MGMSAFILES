import { redisService, appwriteService, postgresService } from "../../Routes/Services.js";
import { Query } from "appwrite";
import { encryptPhone, maskPhone } from "../UserModel/phoneCrypto.js";

export class UserCreateContactModel {
  static async create(userContact) {
    // contact.ContactNumber is raw E.164 input
    const encryptedNumber = encryptPhone(userContact.Phone);
    const pendingKey = `contact:pending:${userContact.AppWriteID}:${userContact.Phone}`;
    const finalKey = `contact:${userContact.AppWriteID}:${userContact.Phone}`;

    // Ensure Redis is connected
    if (!redisService.client.isOpen) await redisService.connect();

    // --- STEP 1: Queue contact in Redis ---
    let attempts = 1;
    const existingAttempts = await redisService.client.hGet(pendingKey, "attempts");
    if (existingAttempts) attempts = parseInt(existingAttempts, 10) + 1;

    await redisService.client.hSet(pendingKey, {
      UserID: userContact.UserID,
      AppwriteID: userContact.AppWriteID,
      FirstName: userContact.FirstName,
      MiddleName: userContact.MiddleName,
      LastName: userContact.LastName,
      Phone: userContact.Phone,
      HomePhone: userContact.HomePhone,
      WorkPhone: userContact.WorkPhone,
      Email: userContact.Email,
      WorkEmail: userContact.WorkEmail,
      JobTitle: userContact.JobTitle,
      Department: userContact.Department,
      Company: userContact.Company,
      Website: userContact.Website,
      status: "pending",
      attempts: attempts.toString(),
      lastError: "",
      createdAt: new Date().toISOString(),
    });

    await redisService.client.expire(pendingKey, 86400 * 7); // 7 days

    let appwriteDoc;

    try {
      const dbId = process.env.APPWRITE_CONTACTS_DB_ID;
      const tableId = "usercontact";
      const contactsCollection = "contacts";

      /**
       * STEP 1: Search number in CONTACTS
       */
      const contactResult = await appwriteService.databases.listDocuments(
        dbId,
        contactsCollection,
        [
          Query.or([
            Query.equal("ContactNumber", userContact.Phone),
            Query.equal("NationalNumber", userContact.Phone),
          ]),
        ]
      );

      /**
       * CASE 1: NUMBER FOUND IN CONTACTS
       * → use contactDoc.ContactNumber
       * → check duplicate in usercontact
       */
      if (contactResult.total > 0) {
        const contactDoc = contactResult.documents[0];
        const contactNumber = contactDoc.ContactNumber;

        /**
         * STEP 3: Look in usercontact using contactDoc.ContactNumber
         */
        const existingDocs = await appwriteService.databases.listDocuments(
          dbId,
          tableId,
          [
            Query.equal("AppwriteID", userContact.AppWriteID),
            Query.equal("Phone", contactNumber),
          ]
        );

        /**
         * STEP 4: If number already exists → DO NOT SAVE
         */
        if (existingDocs.total > 0) {
          const existingContact = existingDocs.documents[0];

          return {
            status: "already_exists",
            message: "Number already exists",
            firstName: existingContact.FirstName,
            phone: existingContact.Phone,
          };
        }

        /**
         * STEP 5: Save using contactDoc.ContactNumber
         */
        appwriteDoc = await appwriteService.databases.createDocument(
          dbId,
          tableId,
          "unique()",
          {
            AppwriteID: userContact.AppWriteID,
            FirstName: userContact.FirstName,
            MiddleName: userContact.MiddleName,
            LastName: userContact.LastName,
            Phone: contactNumber,
            HomePhone: userContact.HomePhone,
            WorkPhone: userContact.WorkPhone,
            Email: userContact.Email,
            WorkEmail: userContact.WorkEmail,
            JobTitle: userContact.JobTitle,
            Department: userContact.Department,
            Company: userContact.Company,
            Website: userContact.Website,
          }
        );
      }

      /**
       * CASE 2: NUMBER NOT FOUND IN CONTACTS
       * → jump directly to save
       * → use userContact.Phone
       */
      else {
        appwriteDoc = await appwriteService.databases.createDocument(
          dbId,
          tableId,
          "unique()",
          {
            AppwriteID: userContact.AppWriteID,
            FirstName: userContact.FirstName,
            MiddleName: userContact.MiddleName,
            LastName: userContact.LastName,
            Phone: userContact.Phone,
            HomePhone: userContact.HomePhone,
            WorkPhone: userContact.WorkPhone,
            Email: userContact.Email,
            WorkEmail: userContact.WorkEmail,
            JobTitle: userContact.JobTitle,
            Department: userContact.Department,
            Company: userContact.Company,
            Website: userContact.Website,
          }
        );
      }

      /**
       * STEP 6: Update Redis after success
       */
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
      const query = `SELECT "usercreatecontact"($1, $2, $3,$4, $5, $6, $7, $8, $9, $10, $11, $12, $13);`;
      const values = [
        userContact.UserID,
        userContact.FirstName,
        userContact.MiddleName,
        userContact.LastName,
        userContact.Phone,
        userContact.HomePhone,
        userContact.WorkPhone,
        userContact.Email,
        userContact.WorkEmail,
        userContact.JobTitle,
        userContact.Department,
        userContact.Company,
        userContact.Website,
      ];

      const pgResult = await postgresService.query(query, values);
      const UserContactID = pgResult.rows[0];

      await redisService.client.del(pendingKey);

      const cachedData = { ...appwriteDoc, UserContactID };
      await redisService.client.set(finalKey, JSON.stringify(cachedData));
      await redisService.client.expire(finalKey, 86400);

      return;
    } catch (err) {
      console.error("PostgreSQL failed:", err.message);
      const newAttempts = attempts + 1;

      await redisService.client.hSet(pendingKey, {
        status: "postgres_failed",
        lastError: err.message,
        attempts: newAttempts.toString(),
      });

      if (newAttempts >= 10) {
        try {
          await redisService.client.rename(
            pendingKey,
            `contact:dead:${userContact.AppWriteID}:${userContact.Phone}`
          );
        } catch (renameErr) {
          console.error("Failed to rename pending key:", renameErr.message);
        }
      }

      throw err;
    }
  }
}
