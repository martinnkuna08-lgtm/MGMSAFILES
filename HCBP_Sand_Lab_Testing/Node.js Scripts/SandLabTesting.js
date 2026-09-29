import express from 'express';
import cors from 'cors';
import userRoutes from './SandLabTesting/Routes/userroutes.js';

const app = express();
const PORT = 3000;

app.use(cors());
app.use(express.json());                    // parse JSON bodies
app.use(express.urlencoded({ extended: true }));  // parse URL-encoded bodies (form submissions)

app.use('/', userRoutes); // or '/api'

app.use((err, req, res, next) => {
  console.error(err.stack);
  res.status(500).json({ error: 'Internal Server Error' });
});

app.listen(PORT, () => {
  console.log(`✅ MVC API running on http://localhost:${PORT}`);
});
