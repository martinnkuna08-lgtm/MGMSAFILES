// ../../Model/UserModel/phoneCrypto.js
import crypto from "crypto";
import  parsePhoneNumberFromString from "libphonenumber-js";

export function e164ToLocal(e164) {
  const phone = parsePhoneNumberFromString(e164);
  if (!phone || !phone.isValid()) return null;

  // Correct worldwide national format (no spaces)
  return phone.formatNational().replace(/\s+/g, "");
}

function getKey() {
  const keyEnv = process.env.PHONE_ENCRYPTION_KEY;
  if (!keyEnv) {
    throw new Error("PHONE_ENCRYPTION_KEY not set in env");
  }

  let key;
  // detect hex (64 hex chars for 32 bytes)
  if (/^[0-9a-fA-F]{64}$/.test(keyEnv)) {
    key = Buffer.from(keyEnv, "hex");
  } else {
    // assume base64
    key = Buffer.from(keyEnv, "base64");
  }

  if (key.length !== 32) {
    throw new Error("PHONE_ENCRYPTION_KEY must decode to 32 bytes (AES-256).");
  }
  return key;
}

export function encryptPhone(plainPhone) {
  if (!plainPhone || typeof plainPhone !== "string") throw new Error("Invalid phone to encrypt");
  const key = getKey();
  // AES-256-ECB deterministic; null IV; use PKCS#7 padding (default)
  const cipher = crypto.createCipheriv("aes-256-ecb", key, null);
  cipher.setAutoPadding(true);
  let encrypted = cipher.update(plainPhone, "utf8", "base64");
  encrypted += cipher.final("base64");
  return encrypted; // base64 string
}

export function decryptPhone(encryptedBase64) {
  if (!encryptedBase64 || typeof encryptedBase64 !== "string") throw new Error("Invalid encrypted phone");
  const key = getKey();
  const decipher = crypto.createDecipheriv("aes-256-ecb", key, null);
  decipher.setAutoPadding(true);
  let decrypted = decipher.update(encryptedBase64, "base64", "utf8");
  decrypted += decipher.final("utf8");
  return decrypted;
}

// optional helper to mask a raw or encrypted phone for logs
export function maskPhone(value, raw = false) {
  // if raw === true we assume value is plain phone number and return last 4 visible
  if (raw) {
    const s = value || "";
    const last4 = s.slice(-4);
    return `****${last4}`;
  }
  // if it's encrypted base64 we just return first 8 chars + '...'
  if (typeof value === "string") return `${value.slice(0, 8)}...`;
  return "****";
}
