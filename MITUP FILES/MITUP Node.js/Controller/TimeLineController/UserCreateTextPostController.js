// ../../Controller/Path/CreateContactController.js
import { Result } from "pg";
import { UserCreateTextPostModel } from "../../Model/TimeLineModel/UserCreateTextPostModel.js";


export async function CreateTextPost(req, res) {
  try {
    const { AppWriteID, UserID, TextPost } = req.body;
    if (!AppWriteID || !UserID || !TextPost ) {
      return res.status(400).json({ message: "Missing required fields" });
    }

    const result = await UserCreateTextPostModel.create({ AppWriteID, UserID, TextPost });
  
    return res.status(201).json({
      message: "Text Post created successfully",
      user: result,
   
    });
  } catch (err) {
    return res.status(400).json({ message: err.message });
  }
}