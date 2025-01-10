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
using SQLite;

namespace App_YumYum_YairLeitus
{
    [Table("Meat")]
    public class Meat : Recipe
    {
        [Column("meatType")]
        public string meatType { get; set; } // סוג בשר
        [Column("method")]
        public string method { get; set; } // אופן הכנה
        [Column("doneness")]
        public string doneness { get; set; } // רמת צלייה
        [Column("cookingTime")]
        public int cookingTime { get; set; } // זמן בישול בדקות

        public Meat()
        {

        }

        public Meat(string meatType, string method, string doneness, int cookingTime, string recipeId, string recipeName, string recipeUsername, string recipeImage, string category, int hours, int minutes, string description, string ingredients, string instructions) : base(recipeId, recipeName, recipeUsername, recipeImage, category, hours, minutes, description, ingredients, instructions)
        {
            this.meatType = meatType;
            this.method = method;
            this.doneness = doneness;
            this.cookingTime = cookingTime;
        }
    }
}