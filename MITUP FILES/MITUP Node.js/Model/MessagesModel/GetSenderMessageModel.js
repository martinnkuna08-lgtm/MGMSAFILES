import { appwriteService } from "../../Routes/Services.js";
import { Query } from "appwrite";

export class GetMessagesBySenderIDModel {

  static async GetMessagesBySenderID(AsenderID, AreceiverID) {
    try {
      const dbId = process.env.APPWRITE_CONTACTS_DB_ID;

      const message = await appwriteService.databases.listDocuments(
  dbId,
  "messages",
  [
           Query.or([
            Query.and([
              Query.equal("AsenderID", AsenderID.toString()),
              Query.equal("AreceiverID", AreceiverID.toString()),
            ]),
            Query.and([
              Query.equal("AsenderID", AreceiverID.toString()),
              Query.equal("AreceiverID", AsenderID.toString()),
            ]),
          ]),
  ]
);
 
      return {
        user: message.documents,
      };
    } catch (err) {
      console.error("❌ Appwrite error:", err.message);
      throw err;
    }
  }
}
