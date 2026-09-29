import { GetAllMessagesBySenderIDModel } from "../../Model/MessagesModel/GetAllSenderMessagesModel.js";

export async function GetAllMessagesBySenderIDController(req, res) {
  try {
    const {AsenderID} = req.params;

    if (!AsenderID ) {
      return res.status(400).json({
        message: "Missing required parameter: AsenderID",
      });
    }

    const result = await GetAllMessagesBySenderIDModel.GetAllMessagesBySenderID(AsenderID);
  
    return res.status(200).json({
      message: "User fetched successfully",
      data: result,
    });

  } catch (err) {
    console.error("Error in GetUserByIDController:", err.message);

    return res.status(500).json({
      message: "Internal server error",
      error: err.message,
    });
  }
}