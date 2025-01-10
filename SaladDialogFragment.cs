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
    public class SaladDialogFragment : DialogFragment
    {
        // מערך המייצג את הכפתורים המכילים האם מדובר בסלט פירות/ירקות
        private Button[] btnIsFruit;

        // מחרוזת המציינת פירות/ירקות
        private string isFruit;

        // מערך המייצג את הכפתורים המכילים את סוגי הסלטים
        private Button[] btnSaladType;

        // מחרוזת סוג הסלט שנבחר
        private string saladType;

        // מערך המייצג את הכפתורים המכילים האם הסלט כולל בשר
        private Button[] btnIsMeat;

        // מחרוזת המציינת כן/לא מכיל בשר
        private string isMeat;

        // מערך המייצג את הכפתורים המכילים האם הסלט כולל טיבול
        private Button[] btnIsFlavoring;

        // מחרוזת המציינת כן/לא מכיל טיבול
        private string isFlavoring;

        // כפתור שמירה בסיום הטופס
        private Button btnSave;

        // משתנה השומר את ההודעה המתאימה במקרה של שגיאה
        private string errorText;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            // כך שהוא יוצג על המסך DialogFragment ניפוח 
            View view = inflater.Inflate(Resource.Layout.saladDialogFragment_layout, container, false);

            isFruit = "";
            // מערך כפתורי האם הסלט מכיל פירות/ירקות מכיל 2 אפשרויות
            btnIsFruit = new Button[2];
            btnIsFruit[0] = view.FindViewById<Button>(Resource.Id.saladDialogFragment_btnIsFruit1);
            btnIsFruit[1] = view.FindViewById<Button>(Resource.Id.saladDialogFragment_btnIsFruit2);
            // לולאה המטפלת בכפתורים הנ"ל ברגע לחיצה
            for (int i = 0; i < btnIsFruit.Length; i++)
            {
                btnIsFruit[i].Click += BtnIsFruit_Click;
            }

            saladType = "";
            // מערך כפתורי סוגי סלט מכיל 4 סוגים
            btnSaladType = new Button[4];
            btnSaladType[0] = view.FindViewById<Button>(Resource.Id.saladDialogFragment_btnSaladType1);
            btnSaladType[1] = view.FindViewById<Button>(Resource.Id.saladDialogFragment_btnSaladType2);
            btnSaladType[2] = view.FindViewById<Button>(Resource.Id.saladDialogFragment_btnSaladType3);
            btnSaladType[3] = view.FindViewById<Button>(Resource.Id.saladDialogFragment_btnSaladType4);
            // לולאה המטפלת בכפתורים הנ"ל ברגע לחיצה
            for (int i = 0; i < btnSaladType.Length; i++)
            {
                btnSaladType[i].Click += BtnSaladType_Click;
            }

            isMeat = "";
            // מערך כפתורי האם הסלט מכיל בשר מכיל 2 אפשרויות
            btnIsMeat = new Button[2];
            btnIsMeat[0] = view.FindViewById<Button>(Resource.Id.saladDialogFragment_btnIsMeatYes);
            btnIsMeat[1] = view.FindViewById<Button>(Resource.Id.saladDialogFragment_btnIsMeatNo);
            // לולאה המטפלת בכפתורים הנ"ל ברגע לחיצה
            for (int i = 0; i < btnIsMeat.Length; i++)
            {
                btnIsMeat[i].Click += BtnIsMeat_Click;
            }

            isFlavoring = "";
            // מערך כפתורי האם הסלט מכיל טיבול מכיל 2 אפשרויות
            btnIsFlavoring = new Button[2];
            btnIsFlavoring[0] = view.FindViewById<Button>(Resource.Id.saladDialogFragment_btnIsFlavoringYes);
            btnIsFlavoring[1] = view.FindViewById<Button>(Resource.Id.saladDialogFragment_btnIsFlavoringNo);
            // לולאה המטפלת בכפתורים הנ"ל ברגע לחיצה
            for (int i = 0; i < btnIsFlavoring.Length; i++)
            {
                btnIsFlavoring[i].Click += BtnIsFlavoring_Click;
            }

            btnSave = view.FindViewById<Button>(Resource.Id.saladDialogFragment_btnSave);
            btnSave.Click += BtnSave_Click;

            return view;
        }

        // פעולה שנקראת כאשר לוחצים על אחד הכפתורים של האם פירות או ירקות
        private void BtnIsFruit_Click(object sender, EventArgs e)
        {
            // מייצג את פקד הכתור הנלחץ
            Button button = (Button)sender;

            // בדיקה האם צבע טקסט הכפתור הנלחץ הוא לבן
            if (btnIsFruit[Array.IndexOf(btnIsFruit, button)].CurrentTextColor == Color.White)
            {
                // isFruit אם כן ישנה בדיקה האם כבר נלחץ כפתור נוסף ואפשרות כבר שמורה במשתנה 
                if (isFruit == "")
                {
                    // אם המחרוזת ריקה ערכה עתה הוא האפשרות הנבחרת וצבע טקסט הכפתור משתנה לאדום
                    btnIsFruit[Array.IndexOf(btnIsFruit, button)].SetTextColor(Color.ParseColor("#ffd5284a"));
                    isFruit = btnIsFruit[Array.IndexOf(btnIsFruit, button)].Text;
                }
            }
            else
            {
                // אם לא אזי צבע הכפתור הוא אדום, המחרוזת תהפוך לריקה וצבע טקסט הכפתור משתנה ללבן
                btnIsFruit[Array.IndexOf(btnIsFruit, button)].SetTextColor(Color.White);
                isFruit = "";
            }
        }

        // פעולה שנקראת כאשר לוחצים על אחד הכפתורים של סוגי הסלט
        private void BtnSaladType_Click(object sender, EventArgs e)
        {
            // מייצג את פקד הכתור הנלחץ
            Button button = (Button)sender;

            // בדיקה האם צבע טקסט הכפתור הנלחץ הוא לבן
            if (btnSaladType[Array.IndexOf(btnSaladType, button)].CurrentTextColor == Color.White)
            {
                // saladType אם כן ישנה בדיקה האם כבר נלחץ כפתור נוסף וסוג סלט כבר שמור במשתנה 
                if (saladType == "")
                {
                    // אם המחרוזת ריקה ערכה עתה הוא סוג הסלט הנבחר וצבע טקסט הכפתור משתנה לאדום
                    btnSaladType[Array.IndexOf(btnSaladType, button)].SetTextColor(Color.ParseColor("#ffd5284a"));
                    saladType = btnSaladType[Array.IndexOf(btnSaladType, button)].Text;
                }
            }
            else
            {
                // אם לא אזי צבע הכפתור הוא אדום, המחרוזת תהפוך לריקה וצבע טקסט הכפתור משתנה ללבן
                btnSaladType[Array.IndexOf(btnSaladType, button)].SetTextColor(Color.White);
                saladType = "";
            }
        }

        // פעולה שנקראת כאשר לוחצים על אחד הכפתורים של האם מכיל בשר
        private void BtnIsMeat_Click(object sender, EventArgs e)
        {
            // מייצג את פקד הכתור הנלחץ
            Button button = (Button)sender;

            // בדיקה האם צבע טקסט הכפתור הנלחץ הוא לבן
            if (btnIsMeat[Array.IndexOf(btnIsMeat, button)].CurrentTextColor == Color.White)
            {
                // isMeat אם כן ישנה בדיקה האם כבר נלחץ כפתור נוסף ואפשרות כבר שמורה במשתנה 
                if (isMeat == "")
                {
                    // אם המחרוזת ריקה ערכה עתה הוא האפשרות הנבחרת וצבע טקסט הכפתור משתנה לאדום
                    btnIsMeat[Array.IndexOf(btnIsMeat, button)].SetTextColor(Color.ParseColor("#ffd5284a"));
                    isMeat = btnIsMeat[Array.IndexOf(btnIsMeat, button)].Text;
                }
            }
            else
            {
                // אם לא אזי צבע הכפתור הוא אדום, המחרוזת תהפוך לריקה וצבע טקסט הכפתור משתנה ללבן
                btnIsMeat[Array.IndexOf(btnIsMeat, button)].SetTextColor(Color.White);
                isMeat = "";
            }
        }

        // פעולה שנקראת כאשר לוחצים על אחד הכפתורים של האם מכיל טיבול
        private void BtnIsFlavoring_Click(object sender, EventArgs e)
        {
            // מייצג את פקד הכתור הנלחץ
            Button button = (Button)sender;

            // בדיקה האם צבע טקסט הכפתור הנלחץ הוא לבן
            if (btnIsFlavoring[Array.IndexOf(btnIsFlavoring, button)].CurrentTextColor == Color.White)
            {
                // isFlavoring אם כן ישנה בדיקה האם כבר נלחץ כפתור נוסף ואפשרות כבר שמורה במשתנה 
                if (isFlavoring == "")
                {
                    // אם המחרוזת ריקה ערכה עתה הוא האפשרות הנבחרת וצבע טקסט הכפתור משתנה לאדום
                    btnIsFlavoring[Array.IndexOf(btnIsFlavoring, button)].SetTextColor(Color.ParseColor("#ffd5284a"));
                    isFlavoring = btnIsFlavoring[Array.IndexOf(btnIsFlavoring, button)].Text;
                }
            }
            else
            {
                // אם לא אזי צבע הכפתור הוא אדום, המחרוזת תהפוך לריקה וצבע טקסט הכפתור משתנה ללבן
                btnIsFlavoring[Array.IndexOf(btnIsFlavoring, button)].SetTextColor(Color.White);
                isFlavoring = "";
            }
        }

        // פעולה שנקראת כאשר לוחצים על כפתור השמירה
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // בדיקה שכל הנתונים תקינים ובכל נושא נלחץ כפתור אחד
            if (IsValid())
            {
                // מערך מחרוזות המציין את נתוני המידע שנבחרו
                string[] saladDetails;

                // בדיקות באיזה אקטיביטי מדובר להפעלת הפעולה הנכונה
                if (Activity is CreateRecipeActivity)
                {
                    // שליחת סוג המתכון, הכותרות והמידע המתאים בנוגע לסלט 
                    string[] saladTitles = { "Vegetables/Fruits", "Salad Type", "Contains Meat", "Contains Flavoring" };
                    saladDetails = new string[] { isFruit, saladType, isMeat, isFlavoring };
                    // המציגה את המידע CreateRecipeActivityפעולה המופעלת ב
                    ((CreateRecipeActivity)Activity).ReceiveRecipeTypeDetails("Salad", saladTitles, saladDetails);
                }
                if (Activity is RecipePageActivity)
                {
                    // שליחת המידע המתאים בנוגע לסלט 
                    saladDetails = new string[] { isFruit, saladType, isMeat, isFlavoring };
                    // המציגה את המידע בעדכון מתכון RecipePageActivityפעולה המופעלת ב
                    ((RecipePageActivity)Activity).ReceiveRecipeTypeDetails(saladDetails);
                }

                // באקטיביטי הבאים מדובר בסינון מתכונים
                if (Activity is HomepageActivity)
                {
                    // שליחת סוג המתכון והמידע המתאים בנוגע לסלט לסינון
                    saladDetails = new string[] { isFruit, saladType, isMeat, isFlavoring };
                    // המסננת את המתכונים לפי סוג מתכון והמידע עליו HomepageActivityפעולה המופעלת ב
                    ((HomepageActivity)Activity).ReceiveRecipeTypeDetails("Salad", saladDetails);
                }
                if (Activity is FavoriteRecipesActivity)
                {
                    // שליחת סוג המתכון והמידע המתאים בנוגע לסלט לסינון
                    saladDetails = new string[] { isFruit, saladType, isMeat, isFlavoring };
                    // המסננת את המתכונים לפי סוג מתכון והמידע עליו FavoriteRecipesActivityפעולה המופעלת ב
                    ((FavoriteRecipesActivity)Activity).ReceiveRecipeTypeDetails("Salad", saladDetails);
                }
                if (Activity is MyRecipesActivity)
                {
                    // שליחת סוג המתכון והמידע המתאים בנוגע לסלט לסינון
                    saladDetails = new string[] { isFruit, saladType, isMeat, isFlavoring };
                    // המסננת את המתכונים לפי סוג מתכון והמידע עליו MyRecipesActivityפעולה המופעלת ב
                    ((MyRecipesActivity)Activity).ReceiveRecipeTypeDetails("Salad", saladDetails);
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
            // בדיקה שנבחרה אפשרות פירות או ירקות
            if (isFruit == "")
            {
                errorText = "Choose between Vegetables and Fruits";
                return false;
            }
            // בדיקה שנבחר סוג סלט
            if (saladType == "")
            {
                errorText = "Choose a Salad Type";
                return false;
            }
            // בדיקה שנבחרה אפשרות כן או לא
            if (isMeat == "" || isFlavoring == "")
            {
                errorText = "Choose Yes or No";
                return false;
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