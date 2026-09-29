import { UserCreateContactModel } from "../../Model/TimeLineModel/UserCreateContact.js";

export async function UserCreateContact(req, res) {
 try {

    const { UserID, AppWriteID, FirstName, MiddleName, LastName, Phone, HomePhone, WorkPhone, Email, WorkEmail, JobTitle, Department, Company, Website, } = req.body;
    
   
     const result = await UserCreateContactModel.create({  UserID, AppWriteID, FirstName, MiddleName, LastName, Phone, HomePhone, WorkPhone, Email, WorkEmail, JobTitle, Department, Company, Website });
  
     return res.status(201).json({
       message: "Contact created successfully",
       user: result
     });
 
   } catch (err) {
    console.error("Error creating user:", err.message);
    return res.status(400).json({ message: err.message });
  }
}
