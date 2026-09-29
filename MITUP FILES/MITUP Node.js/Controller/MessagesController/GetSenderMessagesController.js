import { GetMessagesBySenderIDModel } from "../../Model/MessagesModel/GetSenderMessageModel.js";

export async function GetMessagesBySenderIDController(req, res) {
  try {
    const {AsenderID, AreceiverID} = req.params;

    if (!AsenderID || !AreceiverID ) {
      return res.status(400).json({
        message: "Missing required parameter: AsenderID and AreceiverID",
      });
    }

    const result = await GetMessagesBySenderIDModel.GetMessagesBySenderID(AsenderID, AreceiverID);
  
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