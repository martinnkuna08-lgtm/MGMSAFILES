import { appwriteService, postgresService } from "../../Routes/Services.js";
import { Query } from "appwrite";

export class SelectUserContactsByIDModel {

  static async SelectUserContactsByID(AppWriteID) {
    const dbId = process.env.APPWRITE_CONTACTS_DB_ID;
    const userContactsCollection = "usercontact";
    const contactsCollection = "contacts";

    try {
      // 1️⃣ Get all contacts from UserContacts with the AppWriteID
      const userResult = await appwriteService.databases.listDocuments(
        dbId,
        userContactsCollection,
        [Query.equal("AppwriteID", AppWriteID.toString())]
      );

      if (userResult.total === 0) {
        return null;
      }

      // ✅ Map through all user contacts
      const results = await Promise.all(
        userResult.documents.map(async (userContact) => {
          // 2️⃣ Check if contact exists in Contacts table
          const contactResult = await appwriteService.databases.listDocuments(
            dbId,
            contactsCollection,
            [
              Query.or([
                Query.equal("ContactNumber", userContact.Phone),
                Query.equal("NationalNumber", userContact.Phone),
                
              ])
            ]
          );

          const availability = contactResult.total > 0 ? "available" : "unavailable";
          const areceiverId = contactResult.total > 0 ? contactResult.documents[0].$id : null;
          const contactNumber = contactResult.total > 0 ? contactResult.documents[0].ContactNumber : null;

          // 3️⃣ Return merged response for this contact
          return {
            AppwriteID: userContact.AppwriteID,
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
            UserContactStatus: userContact.UserContactStatus,
            availability,
            areceiverId,
            contactNumber
          };
        })
      );
      return results;
    } catch (err) {
      console.error("❌ Appwrite error:", err.message);
      return null;
    } 
  }

   static async SelectUserContactsByIDSQL(UserID) {
    // Additional SQL query using UserID
    try {
      const query = 'SELECT "UserID", availability, "ContactID", "ContactNumber" FROM "selectusercontactsbyid"($1)';
      const values = [UserID];

      const { rows } = await  postgresService.query(query, values);
      return rows;
    } catch (err) {
      console.error('Error fetching user contacts from SQL:', err);
      throw err;
    }
  }
}


