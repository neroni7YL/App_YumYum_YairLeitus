using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace App_YumYum_YairLeitus
{
    public class ImageManager
    {
        // DB-המרת תמונה למחרוזת ביטים לצורך איחסון ב
        public static string BitmapToBase64(Bitmap bitmap)
        {
            string str = "";
            // לאורך הסוגריים המסולסלות לאחר מכן הוא משוחרר MemoryStream שימוש באובייקט מטיפוס 
            using (MemoryStream stream = new MemoryStream())
            {
                // למחרוזת Bitmap המרת 
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                byte[] bytes = stream.ToArray();
                str = Convert.ToBase64String(bytes);
            }
            return str;
        }

        // המרת מחרוזת ביטים המייצגת תמונה לאובייקט תמונה לצורך תצוגה
        public static Bitmap Base64ToBitmap(string base64String)
        {
            // Bitmapהמרת מחרוזת ל
            byte[] imageAsBytes = Base64.Decode(base64String, Base64Flags.Default);
            return BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);
        }
    }
}