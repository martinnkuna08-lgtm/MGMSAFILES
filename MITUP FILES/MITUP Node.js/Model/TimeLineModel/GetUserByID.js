import { appwriteService } from "../../Routes/Services.js";
import { Query } from "appwrite";

export class GetUserByIDModel {

  static async GetUserByID(AppWriteID, UserContactID) {
    try {
      const dbId = process.env.APPWRITE_CONTACTS_DB_ID;

      // 1️⃣ Get ContactNumber
      const contactsResult = await appwriteService.databases.listDocuments(
        dbId,
        "contacts",
        [
          Query.equal("$id", AppWriteID.toString()),
          Query.select(["ContactNumber", "NationalNumber"])

        ]
      );

     const contactNumber = contactsResult.total > 0 ? contactsResult.documents[0].ContactNumber : null;
     const nationalNumber = contactsResult.total > 0 ? contactsResult.documents[0].NationalNumber : null;
      // 2️⃣ Get user contacts
      const userResult = await appwriteService.databases.listDocuments(
        dbId,
        "usercontact",
             [
                  Query.or([
                      Query.equal("Phone", contactNumber),
                     Query.equal("Phone", nationalNumber),
                    ]),
                 Query.equal("AppwriteID", UserContactID.toString()),
           ] 
      );
   
       // 2️⃣ Get user contacts
      const user = await appwriteService.databases.listDocuments(
        dbId,
        "users",
        [
          Query.equal("ContactID", AppWriteID.toString()),
       
        ]
      );

      return {
        contactNumber,
        userResult : userResult.documents,
        user: user.documents,
      };
    } catch (err) {
      console.error("❌ Appwrite error:", err.message);
      throw err;
    }
  }
}

