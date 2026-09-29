import { SelectUserByContactIDModel } from "../../Model/UserModel/SelectUserByContactID.js";

export async function GetUserByContactID(req, res) {
  try {
  
    const { AppWriteID } = req.params;

    if (!AppWriteID) {
      return res.status(400).json({ message: "Missing required parameter: AppWriteID" });
    }
    // Call the model method
    const result = await SelectUserByContactIDModel.SelectUserByContactID(AppWriteID);
console.log("Controller result:", result); // 👈 THIS LINE

    return res.status(200).json({
      message: "User fetched successfully",
      data: result,
    });

  } catch (err) {
    console.error("Error in GetUserByContactID controller:", err.message);
    return res.status(500).json({ message: "Internal server error", error: err.message });
  }
}
