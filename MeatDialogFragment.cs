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
    public class MeatDialogFragment : DialogFragment
    {
        // מערך המייצג את הכפתורים המכילים את סוגי הבשר השונים
        private Button[] btnMeatType;

        // מחרוזת סוג הבשר שנבחר
        private string meatType;

        // מערך המייצג את הכפתורים המכילים את דרכי ההכנה השונות
        private Button[] btnMethod;

        // מחרוזת דרך ההכנה שנבחרה
        private string method;

        // מערך המייצג את הכפתורים המכילים את רמות הצלייה השונות
        private Button[] btnDoneness;

        // מחרוזת רמת הצלייה שנבחרה
        private string doneness;

        // המציין את זמן הבישול בדקות EditText פקד
        private EditText etCookingTime;

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
            View view = inflater.Inflate(Resource.Layout.meatDialogFragment_layout, container, false);

            meatType = "";
            // מערך כפתורי סוגי הבשר מכיל 4 סוגים
            btnMeatType = new Button[4];
            btnMeatType[0] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnMeatType1);
            btnMeatType[1] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnMeatType2);
            btnMeatType[2] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnMeatType3);
            btnMeatType[3] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnMeatType4);
            // לולאה המטפלת בכפתורים הנ"ל ברגע לחיצה
            for (int i = 0; i < btnMeatType.Length; i++)
            {
                btnMeatType[i].Click += BtnMeatType_Click;
            }

            method = "";
            // מערך כפתורי דרכי ההכנה מכיל 5 דרכים
            btnMethod = new Button[5];
            btnMethod[0] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnMethod1);
            btnMethod[1] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnMethod2);
            btnMethod[2] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnMethod3);
            btnMethod[3] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnMethod4);
            btnMethod[4] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnMethod5);
            // לולאה המטפלת בכפתורים הנ"ל ברגע לחיצה
            for (int i = 0; i < btnMethod.Length; i++)
            {
                btnMethod[i].Click += BtnMethod_Click;
            }

            doneness = "";
            // מערך כפתורי רמות הצלייה מכיל 7 רמות
            btnDoneness = new Button[7];
            btnDoneness[0] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnDoneness1);
            btnDoneness[1] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnDoneness2);
            btnDoneness[2] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnDoneness3);
            btnDoneness[3] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnDoneness4);
            btnDoneness[4] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnDoneness5);
            btnDoneness[5] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnDoneness6);
            btnDoneness[6] = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnDoneness7);
            // לולאה המטפלת בכפתורים הנ"ל ברגע לחיצה
            for (int i = 0; i < btnDoneness.Length; i++)
            {
                btnDoneness[i].Click += BtnDoneness_Click;
            }

            etCookingTime = view.FindViewById<EditText>(Resource.Id.meatDialogFragment_btnCookingTime);

            layoutSortGone = view.FindViewById<LinearLayout>(Resource.Id.meatDialogFragment_layoutSortGone);
            // יעלם layoutSortGoneאם האקטיביטי שברקע היא אחת משלושתם אזי מדובר בסינון ופקד זמן הבישול שנמצא ב
            if (Activity is HomepageActivity || Activity is FavoriteRecipesActivity || Activity is MyRecipesActivity)
            {
                layoutSortGone.Visibility = ViewStates.Gone;
            }

            btnSave = view.FindViewById<Button>(Resource.Id.meatDialogFragment_btnSave);
            btnSave.Click += BtnSave_Click;

            return view;
        }

        // פעולה שנקראת כאשר לוחצים על אחד הכפתורים של סוגי הבשר
        private void BtnMeatType_Click(object sender, EventArgs e)
        {
            // מייצג את פקד הכתור הנלחץ
            Button button = (Button)sender;

            // בדיקה האם צבע טקסט הכפתור הנלחץ הוא לבן
            if (btnMeatType[Array.IndexOf(btnMeatType, button)].CurrentTextColor == Color.White)
            {
                // meatType אם כן ישנה בדיקה האם כבר נלחץ כפתור נוסף וסוג בשר כבר שמור במשתנה 
                if (meatType == "")
                {
                    // אם המחרוזת ריקה ערכה עתה הוא סוג הבשר הנבחר וצבע טקסט הכפתור משתנה לאדום
                    btnMeatType[Array.IndexOf(btnMeatType, button)].SetTextColor(Color.ParseColor("#ffd5284a"));
                    meatType = btnMeatType[Array.IndexOf(btnMeatType, button)].Text;
                }
            }
            else
            {
                // אם לא אזי צבע הכפתור הוא אדום, המחרוזת תהפוך לריקה וצבע טקסט הכפתור משתנה ללבן
                btnMeatType[Array.IndexOf(btnMeatType, button)].SetTextColor(Color.White);
                meatType = "";
            }
        }

        // פעולה שנקראת כאשר לוחצים על אחד הכפתורים של דרכי ההכנה
        private void BtnMethod_Click(object sender, EventArgs e)
        {
            // מייצג את פקד הכתור הנלחץ
            Button button = (Button)sender;

            // בדיקה האם צבע טקסט הכפתור הנלחץ הוא לבן
            if (btnMethod[Array.IndexOf(btnMethod, button)].CurrentTextColor == Color.White)
            {
                // method אם כן ישנה בדיקה האם כבר נלחץ כפתור נוסף ודרך הכנה כבר שמורה במשתנה 
                if (method == "")
                {
                    // אם המחרוזת ריקה ערכה עתה הוא דרך ההכנה הנבחרץ וצבע טקסט הכפתור משתנה לאדום
                    btnMethod[Array.IndexOf(btnMethod, button)].SetTextColor(Color.ParseColor("#ffd5284a"));
                    method = btnMethod[Array.IndexOf(btnMethod, button)].Text;
                }
            }
            else
            {
                // אם לא אזי צבע הכפתור הוא אדום, המחרוזת תהפוך לריקה וצבע טקסט הכפתור משתנה ללבן
                btnMethod[Array.IndexOf(btnMethod, button)].SetTextColor(Color.White);
                method = "";
            }
        }

        // פעולה שנקראת כאשר לוחצים על אחד הכפתורים של רמות הצלייה
        private void BtnDoneness_Click(object sender, EventArgs e)
        {
            // מייצג את פקד הכתור הנלחץ
            Button button = (Button)sender;

            // בדיקה האם צבע טקסט הכפתור הנלחץ הוא לבן
            if (btnDoneness[Array.IndexOf(btnDoneness, button)].CurrentTextColor == Color.White)
            {
                // doneness אם כן ישנה בדיקה האם כבר נלחץ כפתור נוסף ורמת צלייה כבר שמורה במשתנה 
                if (doneness == "")
                {

                    btnDoneness[Array.IndexOf(btnDoneness, button)].SetTextColor(Color.ParseColor("#ffd5284a"));
                    doneness = btnDoneness[Array.IndexOf(btnDoneness, button)].Text;
                }
            }
            else
            {
                // אם לא אזי צבע הכפתור הוא אדום, המחרוזת תהפוך לריקה וצבע טקסט הכפתור משתנה ללבן
                btnDoneness[Array.IndexOf(btnDoneness, button)].SetTextColor(Color.White);
                doneness = "";
            }
        }

        // פעולה שנקראת כאשר לוחצים על כפתור השמירה
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // בדיקה שכל הנתונים תקינים ובכל נושא נלחץ כפתור אחד
            if (IsValid())
            {
                // מערך מחרוזות המציין את נתוני המידע שנבחרו
                string[] meatDetails;

                // בדיקות באיזה אקטיביטי מדובר להפעלת הפעולה הנכונה
                if (Activity is CreateRecipeActivity)
                {
                    // שליחת סוג המתכון, הכותרות והמידע המתאים בנוגע למנת הבשר 
                    string[] meatTitles = { "Meat Type", "Cooking Method", "Meat Doneness", "Cooking Time" };
                    meatDetails = new string[] { meatType, method, doneness, etCookingTime.Text };
                    // המציגה את המידע CreateRecipeActivityפעולה המופעלת ב
                    ((CreateRecipeActivity)Activity).ReceiveRecipeTypeDetails("Meat", meatTitles, meatDetails);
                }
                if (Activity is RecipePageActivity)
                {
                    // שליחת המידע המתאים בנוגע למנת הבשר 
                    meatDetails = new string[] { meatType, method, doneness, etCookingTime.Text };
                    // המציגה את המידע בעדכון מתכון RecipePageActivityפעולה המופעלת ב
                    ((RecipePageActivity)Activity).ReceiveRecipeTypeDetails(meatDetails);
                }

                // באקטיביטי הבאים מדובר בסינון מתכונים
                if (Activity is HomepageActivity)
                {
                    // שליחת סוג המתכון והמידע המתאים בנוגע למנת הבשר לסינון
                    meatDetails = new string[] { meatType, method, doneness };
                    // המסננת את המתכונים לפי סוג מתכון והמידע עליו HomepageActivityפעולה המופעלת ב
                    ((HomepageActivity)Activity).ReceiveRecipeTypeDetails("Meat", meatDetails);
                }
                if (Activity is FavoriteRecipesActivity)
                {
                    // שליחת סוג המתכון והמידע המתאים בנוגע למנת הבשר לסינון
                    meatDetails = new string[] { meatType, method, doneness };
                    // המסננת את המתכונים לפי סוג מתכון והמידע עליו FavoriteRecipesActivityפעולה המופעלת ב
                    ((FavoriteRecipesActivity)Activity).ReceiveRecipeTypeDetails("Meat", meatDetails);
                }
                if (Activity is MyRecipesActivity)
                {
                    // שליחת סוג המתכון והמידע המתאים בנוגע למנת הבשר לסינון
                    meatDetails = new string[] { meatType, method, doneness };
                    // המסננת את המתכונים לפי סוג מתכון והמידע עליו MyRecipesActivityפעולה המופעלת ב
                    ((MyRecipesActivity)Activity).ReceiveRecipeTypeDetails("Meat", meatDetails);
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
            // בדיקה שנבחר סוג בשר
            if (meatType == "")
            {
                errorText = "Choose a Meat Type";
                return false;
            }
            // בדיקה שנבחרה דרך הכנה
            if (method == "")
            {
                errorText = "Choose a Method";
                return false;
            }
            // בדיקה שנבחרה רמת צלייה
            if (doneness == "")
            {
                errorText = "Choose a Doneness";
                return false;
            }
            // בדיקה שמדובר באקטיביטי הנ"ל כלומר לא מדובר בסינון
            if (Activity is CreateRecipeActivity || Activity is RecipePageActivity)
            {
                // בדיקה שהוקלד זמן הבישול
                if (etCookingTime.Text == "")
                {
                    errorText = "Insert Cooking Time";
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