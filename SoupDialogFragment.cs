using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App_YumYum_YairLeitus
{
    public class SoupDialogFragment : DialogFragment
    {
        // מערך המייצג את הכפתורים המכילים את סוגי המרקים
        private Button[] btnSoupType;

        // מחרוזת סוג המרק שנבחר
        private string soupType;

        // מערך המייצג את הכפתורים המכילים האם מדובר בבישול ארוך/קצר
        private Button[] btnDuration;

        // מחרוזת המציינת בישול ארוך/קצר
        private string duration;

        // המציין את זמן הבישול בדקות EditText פקד
        private EditText etBoilingTime;

        // המציין את טמפרטורת הבישול EditText פקד
        private EditText etBoilingTemperature;

        // שמסתיר נתונים שאין בהם שימוש בזמן סינון LinearLayout
        private LinearLayout layoutSortGone;

        // כפתור שמירה בסיום הטופס
        private Button btnSave;

        // משתנה השומר את ההודעה המתאימה במקרה של שגיאה
        private string errorText;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            // כך שהוא יוצג על המסך DialogFragment ניפוח
            View view = inflater.Inflate(Resource.Layout.soupDialogFragment_layout, container, false);

            soupType = "";
            // מערך כפתורי סוגי המרק מכיל 5 סוגים
            btnSoupType = new Button[5];
            btnSoupType[0] = view.FindViewById<Button>(Resource.Id.soupDialogFragment_btnSoupType1);
            btnSoupType[1] = view.FindViewById<Button>(Resource.Id.soupDialogFragment_btnSoupType2);
            btnSoupType[2] = view.FindViewById<Button>(Resource.Id.soupDialogFragment_btnSoupType3);
            btnSoupType[3] = view.FindViewById<Button>(Resource.Id.soupDialogFragment_btnSoupType4);
            btnSoupType[4] = view.FindViewById<Button>(Resource.Id.soupDialogFragment_btnSoupType5);
            // לולאה המטפלת בכפתורים הנ"ל ברגע לחיצה
            for (int i = 0; i < btnSoupType.Length; i++)
            {
                btnSoupType[i].Click += BtnSoupType_Click;
            }

            duration = "";
            // מערך כפתורי האם הבישול ארוך/קצר מכיל 2 אפשרויות
            btnDuration = new Button[2];
            btnDuration[0] = view.FindViewById<Button>(Resource.Id.soupDialogFragment_btnDurationLong);
            btnDuration[1] = view.FindViewById<Button>(Resource.Id.soupDialogFragment_btnDurationShort);
            // לולאה המטפלת בכפתורים הנ"ל ברגע לחיצה
            for (int i = 0; i < btnDuration.Length; i++)
            {
                btnDuration[i].Click += BtnDuration_Click;
            }

            etBoilingTime = view.FindViewById<EditText>(Resource.Id.soupDialogFragment_btnBoilingTime);
            etBoilingTemperature = view.FindViewById<EditText>(Resource.Id.soupDialogFragment_btnBoilingTemperature);

            layoutSortGone = view.FindViewById<LinearLayout>(Resource.Id.soupDialogFragment_layoutSortGone);
            // יעלמו layoutSortGoneאם האקטיביטי שברקע היא אחת משלושתם אזי מדובר בסינון ופקדי זמן וטמפרטורת הבישול שנמצאים ב
            if (Activity is HomepageActivity || Activity is FavoriteRecipesActivity || Activity is MyRecipesActivity)
            {
                layoutSortGone.Visibility = ViewStates.Gone;
            }

            btnSave = view.FindViewById<Button>(Resource.Id.soupDialogFragment_btnSave);
            btnSave.Click += BtnSave_Click;

            return view;
        }

        // פעולה שנקראת כאשר לוחצים על אחד הכפתורים של סוגי המרק
        private void BtnSoupType_Click(object sender, EventArgs e)
        {
            // מייצג את פקד הכתור הנלחץ
            Button button = (Button)sender;

            // בדיקה האם צבע טקסט הכפתור הנלחץ הוא לבן
            if (btnSoupType[Array.IndexOf(btnSoupType, button)].CurrentTextColor == Color.White)
            {
                // soupType אם כן ישנה בדיקה האם כבר נלחץ כפתור נוסף סוג מרק כבר שמור במשתנה 
                if (soupType == "")
                {
                    // אם המחרוזת ריקה ערכה עתה הוא סוג המרק הנבחר וצבע טקסט הכפתור משתנה לאדום
                    btnSoupType[Array.IndexOf(btnSoupType, button)].SetTextColor(Color.ParseColor("#ffd5284a"));
                    soupType = btnSoupType[Array.IndexOf(btnSoupType, button)].Text;
                }
            }
            else
            {
                // אם לא אזי צבע הכפתור הוא אדום, המחרוזת תהפוך לריקה וצבע טקסט הכפתור משתנה ללבן
                btnSoupType[Array.IndexOf(btnSoupType, button)].SetTextColor(Color.White);
                soupType = "";
            }
        }

        // פעולה שנקראת כאשר לוחצים על אחד הכפתורים של האם ארוך או קצר
        private void BtnDuration_Click(object sender, EventArgs e)
        {
            // מייצג את פקד הכתור הנלחץ
            Button button = (Button)sender;

            // בדיקה האם צבע טקסט הכפתור הנלחץ הוא לבן
            if (btnDuration[Array.IndexOf(btnDuration, button)].CurrentTextColor == Color.White)
            {
                // duration אם כן ישנה בדיקה האם כבר נלחץ כפתור נוסף ואפשרות כבר שמורה במשתנה 
                if (duration == "")
                {
                    // אם המחרוזת ריקה ערכה עתה הוא האפשרות הנבחרת וצבע טקסט הכפתור משתנה לאדום
                    btnDuration[Array.IndexOf(btnDuration, button)].SetTextColor(Color.ParseColor("#ffd5284a"));
                    duration = btnDuration[Array.IndexOf(btnDuration, button)].Text;
                }
            }
            else
            {
                // אם לא אזי צבע הכפתור הוא אדום, המחרוזת תהפוך לריקה וצבע טקסט הכפתור משתנה ללבן
                btnDuration[Array.IndexOf(btnDuration, button)].SetTextColor(Color.White);
                duration = "";
            }
        }

        // פעולה שנקראת כאשר לוחצים על כפתור השמירה
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // בדיקה שכל הנתונים תקינים ובכל נושא נלחץ כפתור אחד
            if (IsValid())
            {
                // מערך מחרוזות המציין את נתוני המידע שנבחרו
                string[] soupDetails;

                // בדיקות באיזה אקטיביטי מדובר להפעלת הפעולה הנכונה
                if (Activity is CreateRecipeActivity)
                {
                    // שליחת סוג המתכון, הכותרות והמידע המתאים בנוגע למרק 
                    string[] soupTitles = { "Soup Type", "Duration", "Boiling Time(minutes)", "Temperature(°C)" };
                    soupDetails = new string[] { soupType, duration, etBoilingTime.Text, etBoilingTemperature.Text };
                    // המציגה את המידע CreateRecipeActivityפעולה המופעלת ב
                    ((CreateRecipeActivity)Activity).ReceiveRecipeTypeDetails("Soup", soupTitles, soupDetails);
                }
                if (Activity is RecipePageActivity)
                {
                    // שליחת המידע המתאים בנוגע למרק 
                    soupDetails = new string[] { soupType, duration, etBoilingTime.Text, etBoilingTemperature.Text };
                    // המציגה את המידע בעדכון מתכון RecipePageActivityפעולה המופעלת ב
                    ((RecipePageActivity)Activity).ReceiveRecipeTypeDetails(soupDetails);
                }

                // באקטיביטי הבאים מדובר בסינון מתכונים
                if (Activity is HomepageActivity)
                {
                    // שליחת סוג המתכון והמידע המתאים בנוגע לסלט לסינון
                    soupDetails = new string[] { soupType, duration };
                    // המסננת את המתכונים לפי סוג מתכון והמידע עליו HomepageActivityפעולה המופעלת ב
                    ((HomepageActivity)Activity).ReceiveRecipeTypeDetails("Soup", soupDetails);
                }
                if (Activity is FavoriteRecipesActivity)
                {
                    // שליחת סוג המתכון והמידע המתאים בנוגע לסלט לסינון
                    soupDetails = new string[] { soupType, duration };
                    // המסננת את המתכונים לפי סוג מתכון והמידע עליו FavoriteRecipesActivityפעולה המופעלת ב
                    ((FavoriteRecipesActivity)Activity).ReceiveRecipeTypeDetails("Soup", soupDetails);
                }
                if (Activity is MyRecipesActivity)
                {
                    // שליחת סוג המתכון והמידע המתאים בנוגע לסלט לסינון
                    soupDetails = new string[] { soupType, duration };
                    // המסננת את המתכונים לפי סוג מתכון והמידע עליו MyRecipesActivityפעולה המופעלת ב
                    ((MyRecipesActivity)Activity).ReceiveRecipeTypeDetails("Soup", soupDetails);
                }
                Dismiss();
            }
            else
            {
                // הצגת הודעת שגיאה בהתאמה
                ShowAlertDialog();
            }
        }

        // פעולה שבודקת את תקינות הנתונים ושכל השדות נבחרו
        private bool IsValid()
        {
            // בדיקה שנבחר סוג מרק
            if (soupType == "")
            {
                errorText = "Choose a Soup Type";
                return false;
            }
            // בדיקה שנבחרה אפשרות ארוך או קצר
            if (duration == "")
            {
                errorText = "Choose between Long and Short Duration";
                return false;
            }
            // בדיקה שמדובר באקטיביטי הנ"ל כלומר לא מדובר בסינון
            if (Activity is RecipePageActivity || Activity is CreateRecipeActivity)
            {
                // בדיקה שהוקלד זמן הבישול
                if (etBoilingTime.Text == "")
                {
                    errorText = "Insert Boiling Time";
                    return false;
                }
                // בדיקה שהוקלדה טמפרטורת הבישול
                if (etBoilingTemperature.Text == "")
                {
                    errorText = "Insert Boiling Temperature";
                    return false;
                }
            }
            return true;
        }

        // כשהיא נקראת ומודיעה שישנה שגיאה AlertDialog פעולה אשר מציגה
        private void ShowAlertDialog()
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(Context);
            alertDialog.SetTitle("Error");
            alertDialog.SetMessage(errorText);
            alertDialog.SetCancelable(true);
            alertDialog.SetNeutralButton("CANCEL", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }
    }
}