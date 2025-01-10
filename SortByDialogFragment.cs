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
    public class SortByDialogFragment : DialogFragment
    {
        // רשימת המתכונים המיעדת לסינון
        private List<Recipe> recipeList;

        // פקד כפתור המציין סינון אלפביתי
        private Button btnAlphabetically;

        // פקד כפתור המציין סינון לפי פופולריות
        private Button btnPopular;

        // פקד כפתור המציין סינון לפי קטגוריות
        private Button btnCategories;

        // פקד כפתור המציין סינון לפי מתכונים חדשים
        private Button btnNew;

        // פקד כפתור המציין סינון לפי סוג מתכון
        private Button btnRecipeType;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            // כך שהוא יוצג על המסך DialogFragment ניפוח
            View view = inflater.Inflate(Resource.Layout.sortByDialogFragment_layout, container, false);

            // הנוכחי Activityסינון בהתאם ל
            if (Activity is HomepageActivity)
            {
                // הרשימה מצביעה על כל המתכונים
                recipeList = DatabaseManager.GetAllRecipes();
            }
            if (Activity is FavoriteRecipesActivity)
            {
                // הרשימה מצביעה על כל המתכונים האהובים של המשתמש המחובר
                recipeList = DatabaseManager.GetAllFavoriteRecipes();
            }
            if (Activity is MyRecipesActivity)
            {
                // הרשימה מצביעה על כל המתכונים שהמשתמש המחובר יצר
                recipeList = DatabaseManager.GetAllMyRecipes();
            }

            btnAlphabetically = view.FindViewById<Button>(Resource.Id.sortByDialogFragment_alphabetically);
            btnPopular = view.FindViewById<Button>(Resource.Id.sortByDialogFragment_popular);
            btnCategories = view.FindViewById<Button>(Resource.Id.sortByDialogFragment_categories);
            btnNew = view.FindViewById<Button>(Resource.Id.sortByDialogFragment_new);
            btnRecipeType = view.FindViewById<Button>(Resource.Id.sortByDialogFragment_recipeType);
            
            // לחיצה על כפתור סידור אלפביתי
            btnAlphabetically.Click += delegate
            {
                // סידור רשימת המתכונים לפי הסדר האלפבתי של המתכונים
                recipeList = (from recipe in recipeList orderby recipe.recipeName select recipe).ToList();
                ArrangeRecipesByActivity();
                Dismiss();
            };

            // לחיצה על כפתור סידור לפי פופולריות
            btnPopular.Click += delegate
            {
                // סידור רשימת המתכונים לפי כמות הלייקים של המתכונים מהגבוה לנמוך
                recipeList = (from recipe in recipeList orderby recipe.likeCount descending select recipe).ToList();
                ArrangeRecipesByActivity();
                Dismiss();
            };

            // לחיצה על כפתור סידור לפי קטגוריות
            btnCategories.Click += delegate
            {
                // המהווה טופס בחירה של קטגוריות DialogFragment פותח 
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                CategoryDialogFragment categoryDialogFragment = new CategoryDialogFragment();
                categoryDialogFragment.Show(transaction, "Category dialog fragment");
                Dismiss();
            };

            // לחיצה על כפתור סידור לפי מתכונים חדשים
            btnNew.Click += delegate
            {
                // סידור רשימת המתכונים לפי תאריך העלאתו מחדש לישן
                recipeList = (from recipe in recipeList orderby recipe.creationTime descending select recipe).ToList();
                ArrangeRecipesByActivity();
                Dismiss();
            };

            // לחיצה על כפתור סידור לפי סוג מתכון
            btnRecipeType.Click += delegate
            {
                // המהווה טופס בחירה של המידע הרצוי לגבי סוג המתכון הנבחר DialogFragment פותח 
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                RecipeTypeDialogFragment recipeTypeDialogFragment = new RecipeTypeDialogFragment();
                recipeTypeDialogFragment.Show(transaction, "Recipe Type dialog fragment");
                Dismiss();
            };

            return view;
        }

        // פעולת עזר המזהה את האקטיביטי הנוכחי ושולחת את את הרשימה המסוננת לאקטיביטי
        private void ArrangeRecipesByActivity()
        {
            if (Activity is HomepageActivity)
            {
                // פעולה המופעלת בדף הבית 
                ((HomepageActivity)Activity).ArrangeRecipes(recipeList);
            }
            if (Activity is FavoriteRecipesActivity)
            {
                // פעולה המופעלת בדף המתכונים האהובים 
                ((FavoriteRecipesActivity)Activity).ArrangeRecipes(recipeList);
            }
            if (Activity is MyRecipesActivity)
            {
                // פעולה המופעלת בדף המתכונים שהמשתמש יצר 
                ((MyRecipesActivity)Activity).ArrangeRecipes(recipeList);
            }
        }
    }
}