import twilio from "twilio";
import { redisService } from "../../Routes/Services.js";

const client = twilio(process.env.TWILIO_ACCOUNT_SID, process.env.TWILIO_AUTH_TOKEN);

// Generate 6-digit OTP
function generateOTP() {
  return Math.floor(100000 + Math.random() * 900000).toString();
}

/**
 * Send OTP via Twilio and store in Redis with 1-minute expiry
 * @param {string} phoneNumber
 * @returns {Promise<string>} The OTP code
 */
export async function sendOTP(phoneNumber) {
  if (!redisService.client.isOpen) await redisService.connect();

  const otpKey = `otp:${phoneNumber}`;

  // Check if OTP already exists
  const existingOTP = await redisService.client.get(otpKey);
  if (existingOTP) return existingOTP; // reuse existing OTP

  const otpCode = generateOTP();
  await redisService.client.set(otpKey, otpCode, { EX: 60 });

  try {
    const message = await client.messages.create({
      body: `Your OTP code is ${otpCode}. It expires in 1 minute.`,
      from: process.env.TWILIO_PHONE_NUMBER,
      to: phoneNumber,
    });
    console.log(`OTP sent to ${phoneNumber}, SID: ${message.sid}`);
  } catch (err) {
    console.error("Failed to send OTP via Twilio:", err);
    throw new Error("Failed to send OTP");
  }

  return otpCode;
}

/**
 * Verify OTP with max 3 attempts
 * @param {string} phoneNumber
 * @param {string} userInputOTP
 */
export async function verifyOTP(phoneNumber, userInputOTP) {
  if (!redisService.client.isOpen) await redisService.connect();

  const otpKey = `otp:${phoneNumber}`;
  const attemptsKey = `otp:attempts:${phoneNumber}`;

  const savedOTP = await redisService.client.get(otpKey);
  if (!savedOTP) throw new Error("OTP expired");

  let attempts = await redisService.client.get(attemptsKey);
  attempts = attempts ? parseInt(attempts) : 0;

  if (attempts >= 3) {
    await redisService.client.del(otpKey);
    await redisService.client.del(attemptsKey);
    throw new Error("Maximum OTP attempts exceeded");
  }

  if (userInputOTP !== savedOTP) {
    attempts++;
    await redisService.client.set(attemptsKey, attempts.toString(), { EX: 60 });
    throw new Error(`Invalid OTP. Attempts left: ${3 - attempts}`);
  }

  // OTP correct → delete keys
  await redisService.client.del(otpKey);
  await redisService.client.del(attemptsKey);
  console.log(`OTP verified for ${phoneNumber}`);
  return true;
}
