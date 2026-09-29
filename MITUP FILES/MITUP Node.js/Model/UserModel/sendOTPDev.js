// ../../Model/UserModel/sendOTPDev.js
import { redisService } from "../../Routes/Services.js";
import { parsePhoneNumberFromString } from "libphonenumber-js";
import twilio from "twilio";
import { encryptPhone, maskPhone } from "./phoneCrypto.js";

// ADD THIS HELPER FUNCTION (only new code you need)
function getE164PhoneNumber(rawNumber, countryCode = null) {
  try {
    const phoneObj = countryCode
      ? parsePhoneNumberFromString(rawNumber, countryCode.toUpperCase())
      : parsePhoneNumberFromString(rawNumber);

    if (!phoneObj || !phoneObj.isValid()) return null;

    // If country was selected, make sure the number actually belongs to that country
    if (countryCode && phoneObj.country !== countryCode.toUpperCase()) {
      return null;
    }

    return phoneObj.format("E.164"); // e.g. +14155552671
  } catch {
    return null;
  }
}

// ADD THIS IMPROVED VALIDATION (replaces your old one, but keeps the same name)
function isValidPhoneNumber(rawNumber, countryCode = null) {
  return getE164PhoneNumber(rawNumber, countryCode) !== null;
}

// Your existing code continues unchanged below...
const twilioClient = twilio(
  process.env.TWILIO_ACCOUNT_SID,
  process.env.TWILIO_AUTH_TOKEN
);

// --- Check if number exists (Twilio Lookup) ---
// (unchanged)
async function isRealPhoneNumber(number) {
  try {
    const result = await twilioClient.lookups.v1
      .phoneNumbers(number)
      .fetch({ type: ["carrier"] });
    return result?.phoneNumber ? true : false;
  } catch (err) {
    console.warn(`Twilio Lookup failed for ${maskPhone(number, true)}: ${err.message}`);
    return false;
  }
}

// --- Generate OTP ---
// (unchanged)
function generateOTP() {
  return Math.floor(100000 + Math.random() * 900000).toString();
}

async function checkOtpRateLimit(phoneNumber) {
  if (!redisService.client.isOpen) await redisService.connect();

  const key = `otp:rate:${encryptPhone(phoneNumber)}`;
  let attempts = await redisService.client.get(key);
  attempts = attempts ? parseInt(attempts, 10) : 0;

  const MAX_ATTEMPTS = 3; // max OTP requests allowed
  const WINDOW_SECONDS = 86400; // 24 hours

  if (attempts >= MAX_ATTEMPTS) {
    throw new Error(
      `Too many OTP requests. Please try again later.`
    );
  }

  // Increment count or set new key
  await redisService.client.set(key, attempts + 1, { EX: WINDOW_SECONDS });
}


// --- Send OTP (dev mode stores OTP in Redis) ---
// (unchanged)
async function _sendOTP(phoneNumberRaw) {
  if (!redisService.client.isOpen) await redisService.connect();

  // 1. Rate limit check
  await checkOtpRateLimit(phoneNumberRaw);

  const enc = encryptPhone(phoneNumberRaw);
  const otpKey = `otp:${enc}`;
  const existingOTP = await redisService.client.get(otpKey);
  if (existingOTP) return existingOTP;

  const otpCode = generateOTP();
  await redisService.client.set(otpKey, otpCode, { EX: 60 });
  console.log(`DEV MODE: OTP for ${maskPhone(enc)} (encrypted) is ${otpCode}`);
  return otpCode;
}

// --- Verify OTP ---
// (unchanged)
async function _verifyOTP(phoneNumberRaw, userInputOTP) {
  if (!redisService.client.isOpen) await redisService.connect();

  const enc = encryptPhone(phoneNumberRaw);
  const otpKey = `otp:${enc}`;


  const savedOTP = await redisService.client.get(otpKey);
  if (!savedOTP) throw new Error("OTP expired");

    const attemptsKey = `otp:attempts:${enc}:${savedOTP}`;

  let attempts = await redisService.client.get(attemptsKey);
  attempts = attempts ? parseInt(attempts, 10) : 0;

  if (attempts >= 20) {
    await redisService.client.del(otpKey);
    await redisService.client.del(attemptsKey);
    throw new Error("Maximum OTP attempts exceeded");
  }

  if (userInputOTP !== savedOTP) {
    attempts++;
    await redisService.client.set(attemptsKey, attempts.toString(), { EX: 60});
    if (attempts >= 20) {
    // kill this OTP because it's fully used
    await redisService.client.del(otpKey);
    await redisService.client.del(attemptsKey);
    throw new Error("Too many attempts for this OTP. Request a new one.");
  }
    throw new Error(`Invalid OTP. Attempts left: ${3 - attempts}`);
  }

  await redisService.client.del(otpKey);
  await redisService.client.del(attemptsKey);
  console.log(`OTP verified for ${maskPhone(enc)}`);
  return true;
}

// MAIN FUNCTION — only changed to accept countryCode and normalize early
export async function sendAndVerifyOTP(rawPhoneNumber, countryCode = null, userInputOTP = null) {
  if (!redisService.client.isOpen) await redisService.connect();

  // This now respects the selected country
  const phoneNumber = getE164PhoneNumber(rawPhoneNumber, countryCode);
  if (!phoneNumber) {
    throw new Error(
      countryCode
        ? `Invalid phone number for selected country (${countryCode})`
        : "Invalid phone number format. Use international format, e.g. +14155552671"
    );
  }

  const real = await isRealPhoneNumber(phoneNumber);
  if (!real) throw new Error("Phone number does not exist or is not reachable");

  if (userInputOTP) {
    await _verifyOTP(phoneNumber, userInputOTP);
    return { verified: true };
  }

  const otpCode = await _sendOTP(phoneNumber);
  return { otpSent: true, otpCode }; // dev-mode: returning OTP
}