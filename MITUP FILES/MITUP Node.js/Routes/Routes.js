import express from 'express';
import multer from "multer";
const storage = multer.memoryStorage();
export const upload = multer({ storage });
import { GetUserByContactID } from "../Controller/UserController/SelectUserByContactIDController.js";
import { GetUserContactsByID } from "../Controller/TimeLineController/SelectUserContactsByIDController.js";
import { GetUserByIDController } from "../Controller/TimeLineController/GetUserByIDController.js";

import { CreateContact } from "../Controller/UserController/CreateContactController.js";
import { CreateMessages } from "../Controller/MessagesController/UserCreateMessageController.js";
import { CreateTextPost } from "../Controller/TimeLineController/UserCreateTextPostController.js";
import { GetMessagesBySenderIDController } from "../Controller/MessagesController/GetSenderMessagesController.js";
import { GetAllMessagesBySenderIDController } from "../Controller/MessagesController/GetAllSenderMessagesController.js";
import { CreateUser } from "../Controller/UserController/CreateUserController.js";
import { UserCreateContact } from "../Controller/TimeLineController/UserCreateController.js";
import { CancelOTP } from "../Controller/UserController/CancelOTPController.js";

const router = express.Router();

router.post('/UserCreateContact', UserCreateContact);
router.post('/CreateContact', CreateContact);
router.post('/CreateMessages', CreateMessages);
router.post('/CreateTextPost', CreateTextPost);
router.post(
  "/CreateUser",
  upload.fields([
    { name: "ProfileImage", maxCount: 1 },
    { name: "CoverImage", maxCount: 1 },
  ]),
  CreateUser
);
router.get('/GetUserContactsByID/:AppWriteID/:UserID', GetUserContactsByID);
router.get('/GetMessagesBySenderID/:AsenderID/:AreceiverID', GetMessagesBySenderIDController);
router.get('/GetAllMessagesBySenderID/:AsenderID', GetAllMessagesBySenderIDController);
router.get('/GetUserByID/:AppWriteID/:UserContactID', GetUserByIDController);
router.get('/GetUserByContactID/:AppWriteID', GetUserByContactID);
router.post("/CancelOTP", CancelOTP);

export default router;

