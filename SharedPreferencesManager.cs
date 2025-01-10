using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App_YumYum_YairLeitus
{
    public static class SharedPreferencesManager
    {
        // Interface for accessing and modifying preference data
        // ממשק שמאפשר שמירת מידע בזיכרון המכשיר
        private static ISharedPreferences preferences;

        // מפתח שמציין את שם הקובץ השמור בזיכרון
        private const string USER_INFO_KEY = "UserInfo";

        // מפתח שמציין את שם המשתמש השמור בקובץ
        private const string USERNAME_KEY = "Username";

        // מפתח שמציין האם המכשיר זוכר את המשתמש בקובץ
        private const string REMEMBER_ME_KEY = "RememberMe";

        // האפליקציה הכללי Context פעולה המקבלת את
        public static void CreateSharedPreferences(Context context)
        {
            // האפליקציה כך שרק האובייקט הנ"ל ראשי לכתוב ולקרוא ממנו Contextיצירת הקובץ ב 
            preferences = context.GetSharedPreferences(USER_INFO_KEY, FileCreationMode.Private);
        }

        // פעולה המחזירה את שם המשתמש המחובר מהקובץ
        public static string GetUsername()
        {
            return preferences.GetString(USERNAME_KEY, "");
        }

        // שינוי שם המשתמש המחובר בקובץ
        public static void SetUsername(string username)
        {
            // Interface used for modifying values in a ISharedPreferences
            // משתנה המקבל הרשאת עריכה לקובץ 
            ISharedPreferencesEditor editor = preferences.Edit();
            editor.PutString(USERNAME_KEY, username);
            editor.Commit();
        }

        // פעולה המחזירה את סטטוס המשתמש המחובר מהקובץ
        public static bool GetRememberMe()
        {
            return preferences.GetBoolean(REMEMBER_ME_KEY, false);
        }

        // שינוי סטטוס המשתמש המחובר בקובץ
        public static void SetRememberMe(bool status)
        {
            // משתנה המקבל הרשאת עריכה לקובץ 
            ISharedPreferencesEditor editor = preferences.Edit();
            editor.PutBoolean(REMEMBER_ME_KEY, status);
            editor.Commit();
        }

        // איפוס נתוני המשתמש כאשר המשתמש מתנתק או יוצא מהאפליקציה ולא זכור במכשיר
        public static void ResetPreferences()
        {
            // משתנה המקבל הרשאת עריכה לקובץ 
            ISharedPreferencesEditor editor = preferences.Edit();
            editor.PutString(USERNAME_KEY, "");
            editor.PutBoolean(REMEMBER_ME_KEY, false);
            editor.Commit();
        }
    }
}