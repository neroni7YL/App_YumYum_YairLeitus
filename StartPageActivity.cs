using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App_YumYum_YairLeitus
{
    [Activity(Label = "YumYum", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/yumyumimage")]
    public class StartPageActivity : Activity
    {
        // הרושם את הודעת הפתיחה בהתאמה TextView פקד 
        TextView tvStatus;

        // משתנה המסוגל לשמור סוג טיפוס, שומר את האקטיביטי אליו יעבור הדף
        Type typeofNextActivity;

        // אובייקט מטיפוס טיימר
        Timer timer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.startPage_layout);

            // יצירת טבלאות משתמשים ומתכונים בכניסה לאפליקציה במידה והן אינן קיימות
            DatabaseManager.CreateTables();

            // Create ISharedPreferences using helper class when entering the app, initialized once when opening the app
            SharedPreferencesManager.CreateSharedPreferences(ApplicationContext);

            tvStatus = FindViewById<TextView>(Resource.Id.startPage_tvStatus);

            // Checking the status of user
            // באמצעות פעולה ממחלקת עזר "ISharedPreferences" בדיקה אם המערכת זוכרת את המשתמש באמצעות לקיחת המידע מקובץ בזכרון 
            if (SharedPreferencesManager.GetRememberMe() == false)
            {
                // איפוס נתוני המשתמש השמורים בקובץ השמור בזכרון כאשר המשתמש לא זכור במכשיר או נכנס בפעם הראשונה
                SharedPreferencesManager.ResetPreferences();

                // JoinActivity המשתנה מצביע על סוג טיפוס 
                typeofNextActivity = typeof(JoinActivity);
                // הצגת הודעת ברוכים הבאים
                tvStatus.Text = "Welcome to YumYum";
            }
            else
            {
                // HomepageActivity המשתנה מצביע על סוג טיפוס 
                typeofNextActivity = typeof(HomepageActivity);
                // "ISharedPreferences" הצגת הודעה מתאימה ולקיחת שם המשתמש מקובץ 
                tvStatus.Text = "Welcome back " + SharedPreferencesManager.GetUsername();
            }

            // Oreo בדיקה אם גרסת המכשיר גדולה או שווה לגרסת
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                // במידה ואינו קיים NotificationChannel בכדי ששירות ההודעות יפעל בגרסאות אלו יש ליצור
                // Notification ומעלה לשם הצגת Oreo יצירתו היא חובה מגרסת 
                NotificationChannel notificationChannel = new NotificationChannel("Channel_StatusNotification", "Status Notification Channel", NotificationImportance.Default);
                NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(notificationChannel);
            }

            // יצירת אובייקט מטיפוס טיימר
            timer = new Timer();

            // delay for 2 seconds
            timer.Interval = 2000;

            // לאחר שהטיימר מפסיק הפעולה מתממשת
            timer.Elapsed += delegate
            {
                // Stops at first tick which takes 2 seconds
                timer.Stop();

                // typeofNextActivity מעבר לאקטיביטי הבא הנקבע ע"י 
                Intent intent = new Intent(this, typeofNextActivity);
                StartActivity(intent);
                Finish();
            };
            // התחלת הטיימר
            timer.Start();
        }
    }
}