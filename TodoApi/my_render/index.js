const express = require('express');
const renderApi = require('api')('@render-api/v1.0#5bgo00fma3164bm');

const app = express();
const PORT = process.env.PORT || 3000;

renderApi.auth('rnd_ZCkrJG5TYR7ciKnbQRhyCg84IVGd');

app.get('/apps', async (req, res) => {
  try {
    const { data } = await renderApi.listServices({
      includePreviews: 'true',
      limit: '20'
    });
    res.json(data);
  } catch (err) {
    console.error('שגיאה בשליפת רשימת אפליקציות:', err);
    res.status(500).json({ error: 'שגיאה בקבלת האפליקציות מ-Render' });
  }
});

app.listen(PORT, () => {
  console.log(`השרת רץ על http://localhost:${PORT}`);
});