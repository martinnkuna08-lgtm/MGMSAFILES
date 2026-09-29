// ../../Controller/Path/CreateContactController.js
import { CreateContactModel } from "../../Model/UserModel/CreateContact.js";
import { sendAndVerifyOTP } from "../../Model/UserModel/sendOTPDev.js";
import { SelectUserByContactIDModel } from "../../Model/UserModel/SelectUserByContactID.js";

export async function CreateContact(req, res) {
  try {
    const { ContactNumber, Country, CountryCode, OTP } = req.body;
    if (!ContactNumber || !Country || !CountryCode) {
      return res.status(400).json({ message: "Missing required fields" });
    }

    // CORRECT ORDER: phone, countryCode, OTP
    const otpResult = await sendAndVerifyOTP(ContactNumber, CountryCode, OTP);
    //                                                ↑
    if (otpResult.otpSent) {
      return res.status(200).json({
        message: `OTP sent to ${ContactNumber}. Please verify to continue.`,
        otpSent: true,
        otpCode: otpResult.otpCode // only for dev mode
      });
    }
    // OTP verified → create contact
    const result = await CreateContactModel.create({ ContactNumber, Country, CountryCode });
    const user = await SelectUserByContactIDModel.SelectUserByContactID(
      result.$id
    );
    return res.status(201).json({
      message: "Contact created successfully",
      user: user,
      appwriteId: result.$id,
      contactID: result.contactID
    });

  } catch (err) {
    return res.status(400).json({ message: err.message });
  }
}
