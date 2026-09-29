import { CancelOTPModel } from "../../Model/UserModel/CancelOTP.js";

export async function CancelOTP(req, res) {
  try {
    const { ContactNumber } = req.body;

    if (!ContactNumber) {
      return res.status(400).json({ message: "ContactNumber is required" });
    }

    const canceled = await CancelOTPModel.cancel(ContactNumber);

    if (canceled) {
      return res.status(200).json({ message: "OTP canceled successfully" });
    } else {
      return res.status(404).json({ message: "No OTP found for this number" });
    }

  } catch (err) {
    console.error("CancelOTP error:", err.message);
    return res.status(500).json({ message: "Internal server error" });
  }
}
