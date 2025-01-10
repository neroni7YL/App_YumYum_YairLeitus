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
    [Table("Pastry")]
    public class Pastry : Recipe
    {
        [Column("isGluten")]
        public string isGluten { get; set; } // האם מכיל גלוטן
        [Column("kosher")]
        public string kosher { get; set; } // סוג כשרות
        [Column("bakingTime")]
        public int bakingTime { get; set; } // זמן אפייה בדקות
        [Column("bakingTemperature")]
        public int bakingTemperature { get; set; } // טמפרטורת אפייה
        
        public Pastry()
        {

        }

        public Pastry(string isGluten, string kosher, int bakingTime, int bakingTemperature, string recipeId, string recipeName, string recipeUsername, string recipeImage, string category, int hours, int minutes, string description, string ingredients, string instructions) : base(recipeId, recipeName, recipeUsername, recipeImage, category, hours, minutes, description, ingredients, instructions)
        {
            this.isGluten = isGluten;
            this.kosher = kosher;
            this.bakingTime = bakingTime;
            this.bakingTemperature = bakingTemperature;
        }
    }
}