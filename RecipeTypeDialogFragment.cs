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
    public class RecipeTypeDialogFragment : DialogFragment
    {
        // כפתור פתיחת טופס סלט
        private Button btnSalad;

        // כפתור פתיחת טופס מרק
        private Button btnSoup;

        // כפתור פתיחת טופס מנת בשר
        private Button btnMeat;

        // כפתור פתיחת טופס מאפה
        private Button btnPastry;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            // כך שהוא יוצג על המסך DialogFragment ניפוח
            View view = inflater.Inflate(Resource.Layout.recipeTypeDialogFragment_layout, container, false);

            btnSalad = view.FindViewById<Button>(Resource.Id.recipeTypeDialogFragment_btnSalad);
            btnSoup = view.FindViewById<Button>(Resource.Id.recipeTypeDialogFragment_btnSoup);
            btnMeat = view.FindViewById<Button>(Resource.Id.recipeTypeDialogFragment_btnMeat);
            btnPastry = view.FindViewById<Button>(Resource.Id.recipeTypeDialogFragment_btnPastry);

            // כפתור סינון לפי סלט פותח טופס שעל פיו ניתן לסנן
            btnSalad.Click += delegate
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                SaladDialogFragment saladDialogFragment = new SaladDialogFragment();
                saladDialogFragment.Show(transaction, "Salad dialog fragment");
                Dismiss();
            };

            // כפתור סינון לפי מרק פותח טופס שעל פיו ניתן לסנן
            btnSoup.Click += delegate
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                SoupDialogFragment soupDialogFragment = new SoupDialogFragment();
                soupDialogFragment.Show(transaction, "Soup dialog fragment");
                Dismiss();
            };

            // כפתור סינון לפי מנת בשר פותח טופס שעל פיו ניתן לסנן
            btnMeat.Click += delegate
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                MeatDialogFragment meatDialogFragment = new MeatDialogFragment();
                meatDialogFragment.Show(transaction, "Meat dialog fragment");
                Dismiss();
            };

            // כפתור סינון לפי מאפה פותח טופס שעל פיו ניתן לסנן
            btnPastry.Click += delegate
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                PastryDialogFragment pastryDialogFragment = new PastryDialogFragment();
                pastryDialogFragment.Show(transaction, "Pastry dialog fragment");
                Dismiss();
            };

            return view;
        }
    }
}