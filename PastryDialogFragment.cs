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
    public class PastryDialogFragment : DialogFragment
    {
        // מערך המייצג את הכפתורים המכילים האם המאפה מכיל גלוטן כן/לא
        private Button[] btnIsGluten;

        // מחרוזת המציינת כן/לא מכיל גלוטן
        private string isGluten;

        // מערך המייצג את הכפתורים המכילים את סוגי הכשרות
        private Button[] btnKosher;

        // מחרוזת סוג הכשרות שנבחר
        private string kosher;

        // המציין את זמן האפייה בדקות EditText פקד
        private EditText etBakingTime;

        // המציין את טמפרטורת הבישול EditText פקד
        private EditText etBakingTemperature;

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
            View view = inflater.Inflate(Resource.Layout.pastryDialogFragment_layout, container, false);

            isGluten = "";
            // מערך כפתורי האם המאפה מכיל גלוטן מכיל 2 אפשרויות
            btnIsGluten = new Button[2];
            btnIsGluten[0] = view.FindViewById<Button>(Resource.Id.pastryDialogFragment_btnIsGlutenYes);
            btnIsGluten[1] = view.FindViewById<Button>(Resource.Id.pastryDialogFragment_btnIsGlutenNo);
            // לולאה המטפלת בכפתורים הנ"ל ברגע לחיצה
            for (int i = 0; i < btnIsGluten.Length; i++)
            {
                btnIsGluten[i].Click += BtnIsGluten_Click;
            }

            kosher = "";
            // מערך כפתורי סוגי הכשרות מכיל 3 סוגים
            btnKosher = new Button[3];
            btnKosher[0] = view.FindViewById<Button>(Resource.Id.pastryDialogFragment_btnKosherDairy);
            btnKosher[1] = view.FindViewById<Button>(Resource.Id.pastryDialogFragment_btnKosherMeat);
            btnKosher[2] = view.FindViewById<Button>(Resource.Id.pastryDialogFragment_btnKosherPareve);
            // לולאה המטפלת בכפתורים הנ"ל ברגע לחיצה
            for (int i = 0; i < btnKosher.Length; i++)
            {
                btnKosher[i].Click += BtnKosher_Click;
            }

            etBakingTime = view.FindViewById<EditText>(Resource.Id.pastryDialogFragment_etBakingTime);
            etBakingTemperature = view.FindViewById<EditText>(Resource.Id.pastryDialogFragment_etBakingTemperature);

            layoutSortGone = view.FindViewById<LinearLayout>(Resource.Id.pastryDialogFragment_layoutSortGone);
            // יעלמו layoutSortGoneאם האקטיביטי שברקע היא אחת משלושתם אזי מדובר בסינון ופקדי זמן וטמפרטורת הבישול שנמצאים ב
            if (Activity is HomepageActivity || Activity is FavoriteRecipesActivity || Activity is MyRecipesActivity)
            {
                layoutSortGone.Visibility = ViewStates.Gone;
            }

            btnSave = view.FindViewById<Button>(Resource.Id.pastryDialogFragment_btnSave);
            btnSave.Click += BtnSave_Click;

            return view;
        }

        // פעולה שנקראת כאשר לוחצים על אחד הכפתורים של האם מכיל גלוטן
        private void BtnIsGluten_Click(object sender, EventArgs e)
        {
            // מייצג את פקד הכתור הנלחץ
            Button button = (Button)sender;

            // בדיקה האם צבע טקסט הכפתור הנלחץ הוא לבן
            if (btnIsGluten[Array.IndexOf(btnIsGluten, button)].CurrentTextColor == Color.White)
            {
                // isGluten אם כן ישנה בדיקה האם כבר נלחץ כפתור נוסף ואפשרות כבר שמורה במשתנה 
                if (isGluten == "")
                {
                    // אם המחרוזת ריקה ערכה עתה הוא האפשרות הנבחרת וצבע טקסט הכפתור משתנה לאדום
                    btnIsGluten[Array.IndexOf(btnIsGluten, button)].SetTextColor(Color.ParseColor("#ffd5284a"));
                    isGluten = btnIsGluten[Array.IndexOf(btnIsGluten, button)].Text;
                }
            }
            else
            {
                // אם לא אזי צבע הכפתור הוא אדום, המחרוזת תהפוך לריקה וצבע טקסט הכפתור משתנה ללבן
                btnIsGluten[Array.IndexOf(btnIsGluten, button)].SetTextColor(Color.White);
                isGluten = "";
            }
        }

        // פעולה שנקראת כאשר לוחצים על אחד הכפתורים של סוגי הכשרות
        private void BtnKosher_Click(object sender, EventArgs e)
        {
            // מייצג את פקד הכתור הנלחץ
            Button button = (Button)sender;

            // בדיקה האם צבע טקסט הכפתור הנלחץ הוא לבן
            if (btnKosher[Array.IndexOf(btnKosher, button)].CurrentTextColor == Color.White)
            {
                // kosher אם כן ישנה בדיקה האם כבר נלחץ כפתור נוסף וסוג כשרות כבר שמור במשתנה 
                if (kosher == "")
                {
                    // אם המחרוזת ריקה ערכה עתה הוא סוג הכשרות הנבחר וצבע טקסט הכפתור משתנה לאדום
                    btnKosher[Array.IndexOf(btnKosher, button)].SetTextColor(Color.ParseColor("#ffd5284a"));
                    kosher = btnKosher[Array.IndexOf(btnKosher, button)].Text;
                }
            }
            else
            {
                // אם לא אזי צבע הכפתור הוא אדום, המחרוזת תהפוך לריקה וצבע טקסט הכפתור משתנה ללבן
                btnKosher[Array.IndexOf(btnKosher, button)].SetTextColor(Color.White);
                kosher = "";
            }
        }

        // פעולה שנקראת כאשר לוחצים על כפתור השמירה
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // בדיקה שכל הנתונים תקינים ובכל נושא נלחץ כפתור אחד
            if (IsValid())
            {
                // מערך מחרוזות המציין את נתוני המידע שנבחרו
                string[] pastryDetails;

                // בדיקות באיזה אקטיביטי מדובר להפעלת הפעולה הנכונה
                if (Activity is CreateRecipeActivity)
                {
                    // שליחת סוג המתכון, הכותרות והמידע המתאים בנוגע למאפה 
                    string[] pastryTitles = { "Contains Gluten", "Kosher", "Baking Time(minutes)", "Temperature(°C)" };
                    pastryDetails = new string[] { isGluten, kosher, etBakingTime.Text, etBakingTemperature.Text };
                    // המציגה את המידע CreateRecipeActivityפעולה המופעלת ב
                    ((CreateRecipeActivity)Activity).ReceiveRecipeTypeDetails("Pastry", pastryTitles, pastryDetails);
                }
                if (Activity is RecipePageActivity)
                {
                    // שליחת המידע המתאים בנוגע למאפה 
                    pastryDetails = new string[] { isGluten, kosher, etBakingTime.Text, etBakingTemperature.Text };
                    // המציגה את המידע בעדכון מתכון RecipePageActivityפעולה המופעלת ב
                    ((RecipePageActivity)Activity).ReceiveRecipeTypeDetails(pastryDetails);
                }

                // באקטיביטי הבאים מדובר בסינון מתכונים
                if (Activity is HomepageActivity)
                {
                    // שליחת סוג המתכון והמידע המתאים בנוגע למאפה לסינון
                    pastryDetails = new string[] { isGluten, kosher };
                    // המסננת את המתכונים לפי סוג מתכון והמידע עליו HomepageActivityפעולה המופעלת ב
                    ((HomepageActivity)Activity).ReceiveRecipeTypeDetails("Pastry", pastryDetails);
                }
                if (Activity is FavoriteRecipesActivity)
                {
                    // שליחת סוג המתכון והמידע המתאים בנוגע למאפה לסינון
                    pastryDetails = new string[] { isGluten, kosher };
                    // המסננת את המתכונים לפי סוג מתכון והמידע עליו FavoriteRecipesActivityפעולה המופעלת ב
                    ((FavoriteRecipesActivity)Activity).ReceiveRecipeTypeDetails("Pastry", pastryDetails);
                }
                if (Activity is MyRecipesActivity)
                {
                    // שליחת סוג המתכון והמידע המתאים בנוגע למאפה לסינון
                    pastryDetails = new string[] { isGluten, kosher };
                    // המסננת את המתכונים לפי סוג מתכון והמידע עליו MyRecipesActivityפעולה המופעלת ב
                    ((MyRecipesActivity)Activity).ReceiveRecipeTypeDetails("Pastry", pastryDetails);
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
            // בדיקה שנבחרה אפשרות כן או לא
            if (isGluten == "")
            {
                errorText = "Choose Yes or No";
                return false;
            }
            // בדיקה שנבחר סוג כשרות
            if (kosher == "")
            {
                errorText = "Choose between Dairy, Meat or Pareve";
                return false;
            }
            // בדיקה שמדובר באקטיביטי הנ"ל כלומר לא מדובר בסינון
            if (Activity is CreateRecipeActivity || Activity is RecipePageActivity)
            {
                // בדיקה שהוקלד זמן האפייה
                if (etBakingTime.Text == "")
                {
                    errorText = "Insert baking time";
                    return false;
                }
                // בדיקה שהוקלדה טמפרטורת האפייה
                if (etBakingTemperature.Text == "")
                {
                    errorText = "Insert baking temperature";
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