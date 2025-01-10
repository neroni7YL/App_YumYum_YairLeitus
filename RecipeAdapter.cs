using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App_YumYum_YairLeitus
{
    public class RecipeAdapter : BaseAdapter<Recipe>
    {
        // האקטיביטי המשוייך לאדפטר
        private Activity activity;

        // רשימת המתכונים של האדפטר
        private List<Recipe> recipes;

        public RecipeAdapter(Activity activity, List<Recipe> recipes)
        {
            this.activity = activity;
            this.recipes = recipes;
        }

        public void SetRecipes(List<Recipe> recipes)
        {
            this.recipes = recipes;
        }

        public List<Recipe> GetRecipeList()
        {
            return recipes;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return recipes.Count; }
        }

        public override Recipe this[int position]
        {
            get { return recipes[position]; }
        }

        /* הפעולה יודעת לרוץ כמספר האיברים ברשימה
         כל פעם היא מקבלת את המיקום המתאים */
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // activity-ניפוח הרשומה בדף המתאים לפי ה
            LayoutInflater layoutInflater = activity.LayoutInflater; // Same adapter in three activities
            View view = layoutInflater.Inflate(Resource.Layout.recipeItem_layout, parent, false);

            // וקישורם view-הפרטים המופיעים ב
            TextView tvRecipeName = view.FindViewById<TextView>(Resource.Id.recipeItem_tvRecipeName);
            TextView tvRecipeType = view.FindViewById<TextView>(Resource.Id.recipeItem_tvRecipeType);
            TextView tvCategory = view.FindViewById<TextView>(Resource.Id.recipeItem_tvCategory);
            ImageView ivImage = view.FindViewById<ImageView>(Resource.Id.recipeItem_ivRecipeImage);

            // הכנסת הפרטים לפי פרטי המתכון במיקום המתאים 
            tvRecipeType.Text = "Type: ";
            if (recipes[position] is Salad)
            {
                tvRecipeType.Text += "Salad";
            }
            if (recipes[position] is Soup)
            {
                tvRecipeType.Text += "Soup";
            }
            if (recipes[position] is Meat)
            {
                tvRecipeType.Text += "Meat";
            }
            if (recipes[position] is Pastry)
            {
                tvRecipeType.Text += "Pastry";
            }
            tvRecipeName.Text = "Name: " + recipes[position].recipeName;
            tvCategory.Text = "Categories: " + recipes[position].category;
            Bitmap imageBitmap = ImageManager.Base64ToBitmap(recipes[position].recipeImage);
            ivImage.SetImageBitmap(imageBitmap);

            return view;
        }
    }
}