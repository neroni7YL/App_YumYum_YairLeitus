using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace App_YumYum_YairLeitus
{
    // Service -רישום אוטומטי לקובץ המניפסט כ
    [Service(Label = "StatusNotificationService")]
    public class StatusNotificationService : Service
    {
        // NotificatioChannelמפתח ערוץ הודעות הסטטוס במקרה של שימוש ב
        private const string CHANNEL_ID = "Channel_StatusNotification";

        // של השירות הנ"ל Foreground Service מפתח 
        private const int FOREGROUND_SERVICE_NOTIFICATION_ID = 1;

        // Activity.StartService פעולה המופעלת כל פעם כאשר הסרוויס נקרא דרך הפעולה 
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // מערך מחרוזות המסמל את מידע ההודעה - כותרת ותוכן מתאימים
            string[] notificationDetails = intent.GetStringArrayExtra("StatusNotification_Details");

            // Does not stop the service from running just takes it out of the foreground state
            // removes the notification - מחיקת הודעה קודמת
            StopForeground(true);

            // פעולת עזר המטפלת בקישור הסרוויס והפעלתו
            RegisterForService(notificationDetails);

            // מסמל שהסרוויס יכול להסגר תחת עומס בזכרון NotSticky .כנדרש StartCommandResult מחזיר 
            return StartCommandResult.NotSticky;
        }

        // פעולת עזר המקבלת מערך מחרוזות המכיל את כותרת ותוכן ההודעה ויוצר הודעה מתאימה 
        private void RegisterForService(string[] notificationDetails)
        {
            // Notification אובייקט הודעה
            Notification notification;

            // Oreo בדיקת אם גרסת המכשיר גדולה או שווה לגרסת
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                // Oreo and above require Notification Channel
                // עם נתינת מפתח הערוץ Notification ומעלה יש ליצור Oreo מגרסת 
                notification = new NotificationCompat.Builder(this, CHANNEL_ID)
                    .SetContentTitle(notificationDetails[0])
                    .SetContentText(notificationDetails[1])
                    .SetSmallIcon(Resource.Drawable.YumYumImage)
                    .SetAutoCancel(true)
                    .Build();
            }
            else
            {
                // Pre - Oreo behavior
                // NotificationChannelלא נדרש שימוש ב Oreo בגרסאות שיצאו טרם 
                notification = new NotificationCompat.Builder(this)
                    .SetContentTitle(notificationDetails[0])
                    .SetContentText(notificationDetails[1])
                    .SetSmallIcon(Resource.Drawable.YumYumImage)
                    .SetAutoCancel(true)
                    .Build();
            }

            // Enlist this instance of the service as a foreground service
            // התחלתו והצגת הודעה מתאימה ,Foreground Serviceיצירת הסרוויס כ
            StartForeground(FOREGROUND_SERVICE_NOTIFICATION_ID, notification);
        }

        // פעולה שחובה להצהיר עליה בסרוויס
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}