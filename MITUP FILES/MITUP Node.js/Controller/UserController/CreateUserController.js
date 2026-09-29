import { CreateUserModel } from "../../Model/UserModel/CreateUser.js";

export async function CreateUser(req, res) {
 try {

    const { FirstNames, LastName, Gender, AppWriteID, ContactID, DateOfBirth, Bio, Location} = req.body;
    
    const profileFile = req.files?.ProfileImage?.[0];  
    const coverFile = req.files?.CoverImage?.[0];
    
     if (!FirstNames || !LastName || !Gender || !AppWriteID || !ContactID || !DateOfBirth) {
       return res.status(400).json({ message: "Missing required fields" });
     }
 
  
     const result = await CreateUserModel.create({ FirstNames, LastName, Gender, AppWriteID, ContactID, DateOfBirth, Bio, Location, CoverImage: coverFile ,ProfileImage: profileFile });
  
     return res.status(201).json({
       message: "Contact created successfully",
       user: result
     });
 
   } catch (err) {
    console.error("Error creating user:", err.message);
    return res.status(400).json({ message: err.message });
  }
}
