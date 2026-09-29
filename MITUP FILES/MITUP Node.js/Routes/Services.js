import dotenv from "dotenv";
dotenv.config();

import { Client as AppwriteClient, Databases, Storage } from "node-appwrite";
import { createClient as createRedisClient } from "redis";
import { Pool as PgPool } from "pg";

// --- Appwrite Singleton ---
class AppwriteService {
  constructor() {
    this.client = new AppwriteClient()
      .setEndpoint(process.env.APPWRITE_ENDPOINT)
      .setProject(process.env.APPWRITE_PROJECT_ID)
      .setKey(process.env.APPWRITE_API_KEY);
    this.databases = new Databases(this.client);
    this.storage = new Storage(this.client); // <--- ADD THIS
  }

  async listDatabases() {
    try {
      const dbs = await this.databases.list();
      console.log("✔ Appwrite reachable. Databases:", dbs.databases.map(d => d.$id));
      return dbs.databases;
    } catch (err) {
      console.error("❌ Appwrite error:", err.message);
      throw err;
    }
  }
}

export const appwriteService = new AppwriteService();
// --- Redis Singleton ---
class RedisService {
  constructor() {
    this.client = createRedisClient({
      socket: {
        host: process.env.REDIS_HOST,
        port: parseInt(process.env.REDIS_PORT, 10),
      },
    });
    this.client.on("error", (err) => console.error("❌ Redis error:", err));
  }
  async connect() {
    if (!this.client.isOpen) {
      await this.client.connect();
      console.log("✔ Redis connected");
    }
  }

  async disconnect() {
    if (this.client.isOpen) {
      await this.client.disconnect();
      console.log("✔ Redis disconnected");
    }
  }
}

export const redisService = new RedisService();


// --- PostgreSQL Singleton ---
class PostgresService {
  constructor() {
    this.pool = new PgPool({
      host: process.env.PG_HOST,
      port: parseInt(process.env.PG_PORT, 10),
      user: process.env.PG_USER,
      password: process.env.PG_PASSWORD,
      database: process.env.PG_DATABASE,
      max: 20,            // max clients in pool
      idleTimeoutMillis: 30000, // close idle clients after 30s
      connectionTimeoutMillis: 2000, // return error if connection takes > 2s
    });
  }

  async query(text, params) {
    try {
      const res = await this.pool.query(text, params);
      return res;
    } catch (err) {
      console.error("❌ PostgreSQL query error:", err.message);
      throw err;
    }
  }

  async testConnection() {
    try {
      const res = await this.query("SELECT NOW()");
      console.log("✔ PostgreSQL reachable. Current time:", res.rows[0]);
    } catch (err) {
      console.error("❌ PostgreSQL connection error:", err.message);
    }
  }

  async close() {
    await this.pool.end();
    console.log("✔ PostgreSQL pool closed");
  }
}

export const postgresService = new PostgresService();


// --- Initialize all connections ---
export async function initializeServices() {
  await redisService.connect();
  await appwriteService.listDatabases();
  await postgresService.testConnection();
  console.log("✅ All services initialized");
}
