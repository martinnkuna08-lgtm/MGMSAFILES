import { appwriteService } from "../../Routes/Services.js";
import { Query } from "appwrite";

export class GetAllMessagesBySenderIDModel {
 static async GetAllMessagesBySenderID(AsenderID) {
  try {
    if (!AsenderID) {
      throw new Error("AsenderID is undefined");
    }

    const senderIdStr = String(AsenderID);
    const dbId = process.env.APPWRITE_CONTACTS_DB_ID;

    const messageResult = await appwriteService.databases.listDocuments(
      dbId,
      "messages",
      [
        Query.or([
          Query.equal("AsenderID", senderIdStr),
          Query.equal("AreceiverID", senderIdStr),
        ]),
        Query.orderDesc("$createdAt"), // newest first
      ]
    );

    if (messageResult.total === 0) return null;

    const messages = messageResult.documents;
   // collect all other user IDs
// 2️⃣ Collect all otherUserIds
const otherUserIds = messages.map(msg =>
  msg.AsenderID === AsenderID.toString() ? msg.AreceiverID : msg.AsenderID
);

const uniqueUserIds = Array.from(new Set(otherUserIds));

// 3️⃣ Build OR queries for contacts
const contactsQuery = Query.or(uniqueUserIds.map(id => Query.equal("$id", id)));
const contactsResult = await appwriteService.databases.listDocuments(
  dbId,
  "contacts",
  [contactsQuery, Query.select(["ContactNumber", "NationalNumber"])]
);
console.log("My Contact Results", contactsResult.documents);





// 4️⃣ Build OR queries for usercontact
const userContactsQuery = Query.or(uniqueUserIds.map(id => Query.equal("AppwriteID", id)));
const userContactsResult = await appwriteService.databases.listDocuments(
  dbId,
  "usercontact",
  [userContactsQuery]
);
console.log("User Contact Results", userContactsResult.documents);






// 5️⃣ Build OR queries for users
const usersQuery = Query.or(uniqueUserIds.map(id => Query.equal("ContactID", id)));
const usersResult = await appwriteService.databases.listDocuments(
  dbId,
  "users",
  [usersQuery]
);
console.log("Users Results", usersResult.documents);



    return { 
      messages ,
    userContacts: userContactsResult.documents,
    users: usersResult.documents,

    };
  } catch (err) {
    console.error("❌ Appwrite error:", err.message);
    throw err;
  }
}

}


