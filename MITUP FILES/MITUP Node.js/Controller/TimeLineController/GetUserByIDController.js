import { GetUserByIDModel } from "../../Model/TimeLineModel/GetUserByID.js";

export async function GetUserByIDController(req, res) {
  try {
    const { AppWriteID, UserContactID } = req.params;

    if (!AppWriteID || !UserContactID ) {
      return res.status(400).json({
        message: "Missing required parameter: AppWriteID and UserContactID",
      });
    }

    const result = await GetUserByIDModel.GetUserByID(AppWriteID,UserContactID );
    

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
