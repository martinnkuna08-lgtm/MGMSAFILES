import { appwriteService} from "../../Routes/Services.js";
import { Query } from "appwrite";
import { decryptPhone } from "./phoneCrypto.js";

export class SelectUserByContactIDModel {

  static async SelectUserByContactID(AppWriteID) {
   
    try {
      const dbId = process.env.APPWRITE_CONTACTS_DB_ID;
      const collectionId = "users";  // must be COLLECTION ID
      const result = await appwriteService.databases.listDocuments(
        dbId,
        collectionId,
        [
          Query.equal("ContactID", AppWriteID.toString()),
          Query.limit(1),
        ]
      );
      if (result.total > 0) {
        const user = result.documents[0];
        const decryptedDOB = decryptPhone(user.DateOfBirth);
        const decryptedLocation = decryptPhone(user.Location);
        // Transform Appwrite fields to match your desired format
       return {
        firstNames: user.FirstNames,
        lastName: user.LastName,
        gender: user.Gender,
        appwriteId: AppWriteID,
        dateOfBirth: decryptedDOB,
        bio: user.Bio,
        location: decryptedLocation,
        profilePicture: user.ProfilePictureURL,
        coverImage: user.CoverImageURL,
        status: user.UserStatus,
      };  
      } else {
        console.log(`❌ No Appwrite user found with AppWriteID: ${AppWriteID}`);
      }
      
    } catch (err) {
      console.error("❌ Appwrite error:", err.message);
       return null;
    }
  
  }
}
