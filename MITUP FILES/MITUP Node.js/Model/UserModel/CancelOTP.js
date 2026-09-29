// ../../Model/UserModel/cancelOTP.js
import { redisService } from "../../Routes/Services.js";
import { encryptPhone, maskPhone } from "./phoneCrypto.js";

export class CancelOTPModel {
  static async cancel(phoneNumberRaw) {
    if (!redisService.client.isOpen) await redisService.connect();

    const encryptedNumber = encryptPhone(phoneNumberRaw);
    const otpKey = `otp:${encryptedNumber}`;
    const attemptsKeyPrefix = `otp:attempts:${encryptedNumber}:`;

    // Delete main OTP
    const deleted = await redisService.client.del(otpKey);

    // Delete any attempt keys (Redis keys starting with attemptsKeyPrefix)
    const keys = await redisService.client.keys(`${attemptsKeyPrefix}*`);
    if (keys.length > 0) {
      await redisService.client.del(keys);
    }

    console.log(`OTP canceled for ${maskPhone(encryptedNumber)}`);
    return deleted > 0; // returns true if OTP existed and was deleted
  }
}
