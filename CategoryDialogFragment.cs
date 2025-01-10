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
    public class CategoryDialogFragment : DialogFragment
    {
        // מערך המייצג את הכפתורים המכילים את הקטגוריות השונות
        private Button[] btnCategories;

        // כפתור שמירה
        private Button btnSave;

        // משתנה המציין את את כמות הקטגוריות שניבחרו
        private int categoryCount;

        // מספר המציין את כמות הקטגוריות המקסימלי שניתן לבחור
        private const int MAX_CATEGORY_COUNT = 3;

        // DialogFragment פעולה המופעלת בעת היווצרות 
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            // כך שהוא יוצג על המסך DialogFragment ניפוח
            View view = inflater.Inflate(Resource.Layout.categoryDialogFragment_layout, container, false);

            // קישור פקדי כפתורי הקטגוריות - סך הכל עשר קטגוריות
            btnCategories = new Button[10];
            btnCategories[0] = view.FindViewById<Button>(Resource.Id.categoryDialogFragment_btnCategory1);
            btnCategories[1] = view.FindViewById<Button>(Resource.Id.categoryDialogFragment_btnCategory2);
            btnCategories[2] = view.FindViewById<Button>(Resource.Id.categoryDialogFragment_btnCategory3);
            btnCategories[3] = view.FindViewById<Button>(Resource.Id.categoryDialogFragment_btnCategory4);
            btnCategories[4] = view.FindViewById<Button>(Resource.Id.categoryDialogFragment_btnCategory5);
            btnCategories[5] = view.FindViewById<Button>(Resource.Id.categoryDialogFragment_btnCategory6);
            btnCategories[6] = view.FindViewById<Button>(Resource.Id.categoryDialogFragment_btnCategory7);
            btnCategories[7] = view.FindViewById<Button>(Resource.Id.categoryDialogFragment_btnCategory8);
            btnCategories[8] = view.FindViewById<Button>(Resource.Id.categoryDialogFragment_btnCategory9);
            btnCategories[9] = view.FindViewById<Button>(Resource.Id.categoryDialogFragment_btnCategory10);
            btnSave = view.FindViewById<Button>(Resource.Id.categoryDialogFragment_btnSave);

            // מספר הקטגוריות ההתחלתי הוא 0
            categoryCount = 0;

            // לולאה אשר מטפלת בלחיצה על כל כפתור - סך הכל 10 כפתורים 
            for (int i = 0; i < btnCategories.Length; i++)
            {
                btnCategories[i].Click += BtnCategories_Click;
            }

            btnSave.Click += BtnSave_Click;

            return view;
        }

        // פעולה המופעלת בעת לחיצה על כפתור קטגוריה
        private void BtnCategories_Click(object sender, EventArgs e)
        {
            // מייצג את פקד הכתור הנלחץ
            Button category = (Button)sender;

            // בדיקה האם צבע טקסט הכפתור הנלחץ הוא לבן
            if (btnCategories[Array.IndexOf(btnCategories, category)].CurrentTextColor == Color.White)
            {
                // אם כן ישנה בדיקה האם כמות הקטגוריות שנבחרו כבר קטנה מהכמות המקסימלית המורשת
                if (categoryCount < MAX_CATEGORY_COUNT)
                {
                    // אם כן כמות הקטגוריות עולה באחד וצבע טקסט הכפתור משתנה לאדום
                    categoryCount++;
                    btnCategories[Array.IndexOf(btnCategories, category)].SetTextColor(Color.ParseColor("#ffd5284a"));
                }
            }
            else
            {
                // אם לא אזי צבע הכפתור הוא אדום, כמות הקטגוריות יורדת באחד וצבע טקסט הכפתור משתנה ללבן
                categoryCount--;
                btnCategories[Array.IndexOf(btnCategories, category)].SetTextColor(Color.White);
            }
        }

        // פעולה המופעלת כאשר כפתור השמירה נלחץ
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // בדיקה אם כמות הקטגוריות שונה מאפס כלומר בין 1 ל-3
            if (categoryCount != 0)
            {
                // רשימת מחרוזות אשר תכיל את שמות הקטגוריות שנבחרו
                List<string> categoryList = new List<string>();

                // לולאה אשר עוברת על כל כפתור ובודקת אם טקסט הכפתור אדום, כלומר הכפתור נלחץ ושמה את שם הקטגוריה ברשימה הנ"ל
                for (int i = 0; i < btnCategories.Length; i++)
                {
                    if (btnCategories[i].CurrentTextColor == Color.ParseColor("#ffd5284a"))
                    {
                        categoryList.Add(btnCategories[i].Text);
                    }
                }

                // שמות הקטגוריות מועברות למחרוזת אחת שמופרדת בפסיקים
                string categories = string.Join(",", categoryList);

                /* לשם פעולות שונות Activities מיושם בכמה CategoryDialogFragment
                 לכן ישנה בדיקה איזה אקטיביטי נמצא ברקע */
                if (Activity is CreateRecipeActivity)
                {
                    //שמציגה את הקטגוריות שנבחרו בעת הכנת מתכון CreateRecipeActivity-קריאה לפעולה שנמצאת ב
                    ((CreateRecipeActivity)Activity).ReceiveCategories(categories);
                }
                if (Activity is RecipePageActivity)
                {
                    // שמציגה את הקטגוריות שנבחרו בעת עדכון מתכון RecipePageActivity-קריאה לפעולה שנמצאת ב
                    ((RecipePageActivity)Activity).ReceiveCategories(categories);
                }

                if (Activity is HomepageActivity)
                {
                    // שמסננת את המתכונים המוצגים לפי הקטגוריות שנבחרו HomepageActivity-קריאה לפעולה שנמצאת ב
                    ((HomepageActivity)Activity).ReceiveCategories(categories);
                }
                if (Activity is FavoriteRecipesActivity)
                {
                    // שמסננת את המתכונים המוצגים לפי הקטגוריות שנבחרו FavoriteRecipesActivity-קריאה לפעולה שנמצאת ב
                    ((FavoriteRecipesActivity)Activity).ReceiveCategories(categories);
                }
                if (Activity is MyRecipesActivity)
                {
                    // שמסננת את המתכונים המוצגים לפי הקטגוריות שנבחרו MyRecipesActivity-קריאה לפעולה שנמצאת ב
                    ((MyRecipesActivity)Activity).ReceiveCategories(categories);
                }
                Dismiss();
            }
            else
            {
                // AlertDialog כאשר כמות הקטגוריות שווה לאפס ונלחץ כפתור השמירה תוצג שגיאה באמצעות 
                ShowAlertDialog();
            }
        }

        // כשהיא נקראת ומודיעה על כך שאף קטגוריה לא נלחצה AlertDialog פעולה אשר מציגה 
        private void ShowAlertDialog()
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(Context);
            alertDialog.SetTitle("Error");
            alertDialog.SetMessage("No categories were saved");
            alertDialog.SetCancelable(true);
            alertDialog.SetNeutralButton("CANCEL", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }
    }
}