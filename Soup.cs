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
    [Table("Soup")]
    public class Soup : Recipe
    {
        [Column("soupType")]
        public string soupType { get; set; } // סוג מרק
        [Column("duration")]
        public string duration { get; set; } // בישול ארוך או קצר
        [Column("boilingTime")]
        public int boilingTime { get; set; } // זמן בישול בדקות
        [Column("temperature")]
        public int boilingTemperature { get; set; } // טמפרטורת בישול

        public Soup()
        {

        }

        public Soup(string soupType, string duration, int boilingTime, int boilingTemperature, string recipeId, string recipeName, string recipeUsername, string recipeImage, string category, int hours, int minutes, string description, string ingredients, string instructions) : base(recipeId, recipeName, recipeUsername, recipeImage, category, hours, minutes, description, ingredients, instructions)
        {
            this.soupType = soupType;
            this.duration = duration;
            this.boilingTime = boilingTime;
            this.boilingTemperature = boilingTemperature;
        }
    }
}