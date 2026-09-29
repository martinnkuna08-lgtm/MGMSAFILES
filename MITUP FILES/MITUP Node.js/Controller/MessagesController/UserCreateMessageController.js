// ../../Controller/Path/CreateContactController.js
import { Result } from "pg";
import { UserCreateMessageModel } from "../../Model/MessagesModel/UserCreateMessageModel.js";


export async function CreateMessages(req, res) {
  try {
    const { AreceiverID, PreceiverID, AreceiverPhone, TextMessages, IsRead, MessageType, AsenderID,  PsenderID  } = req.body;
    if (!AreceiverID || !PreceiverID || !TextMessages || !AreceiverPhone  || !MessageType || !AsenderID || !PsenderID) {
      return res.status(400).json({ message: "Missing required fields" });
    }

    const result = await UserCreateMessageModel.create({ AreceiverID, PreceiverID, TextMessages, IsRead,AreceiverPhone, MessageType, AsenderID,  PsenderID });
  
    return res.status(201).json({
      message: "Contact created successfully",
      user: result,
   
    });
  } catch (err) {
    return res.status(400).json({ message: err.message });
  }
}
