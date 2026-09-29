import { SelectUserContactsByIDModel } from "../../Model/TimeLineModel/SelectUserContactsByID.js";

export async function GetUserContactsByID(req, res) {
  try {
    const { AppWriteID, UserID} = req.params;

    if (!AppWriteID) {
      return res.status(400).json({
        message: "Missing required parameter: AppWriteID",
      });
    }

    if (!UserID) {
      return res.status(400).json({
        message: "Missing required parameter: UserID",
      });
    }

    const result =
      await SelectUserContactsByIDModel.SelectUserContactsByID(AppWriteID);

       const resultsql =
      await SelectUserContactsByIDModel.SelectUserContactsByIDSQL(UserID);

    console.log("Controller result:", result);
    console.log("Controller result:", resultsql);

    // ✅ Handle not found
    if (!result && !resultsql) {
      return res.status(404).json({
        message: "User contact not found",
        data: null,
      });
    }

    // ✅ Success
    return res.status(200).json({
      message: "User fetched successfully",
      data: result,
      sqldata: resultsql,
    });

  } catch (err) {
    console.error("Error in GetUserContactsByID controller:", err.message);

    return res.status(500).json({
      message: "Internal server error",
      error: err.message,
    });
  }
}