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
    [Table("Recipe")]
    public class Recipe
    {
        [PrimaryKey, Column("RecipeId")]
        public string recipeId { get; set; } // קוד מתכון
        [Column("recipeName")]
        public string recipeName { get; set; } // שם מתכון
        [Column("recipeUsername")]
        public string recipeUsername { get; set; } // שם משתמש יוצר המתכון
        [Column("recipeImage")]
        public string recipeImage { get; set; } // תמונת המתכון
        [Column("category")]
        public string category { get; set; } // קטגוריות
        [Column("hours")]
        public int hours { get; set; } // שעות הכנה
        [Column("minutes")]
        public int minutes { get; set; } // דקות הכנה
        [Column("description")]
        public string description { get; set; } // תיאור המתכון
        [Column("ingredients")]
        public string ingredients { get; set; } // מרכיבים
        [Column("instructions")]
        public string instructions { get; set; } // הוראות הכנה
        [Column("likeCount")]
        public int likeCount { get; set; } // מספר לייקים
        [Column("creationTime")]
        public DateTime creationTime { get; set; } // זמן העלאת המתכון

        public Recipe()
        {

        }

        public Recipe(string recipeId, string recipeName, string recipeUsername, string recipeImage, string category, int hours, int minutes, string description, string ingredients, string instructions)
        { 
            this.recipeId = recipeId;
            this.recipeName = recipeName;
            this.recipeUsername = recipeUsername;
            this.recipeImage = recipeImage;
            this.category = category;
            this.hours = hours;
            this.minutes = minutes;
            this.description = description;
            this.ingredients = ingredients;
            this.instructions = instructions;
            this.likeCount = 0;
            this.creationTime = DateTime.Now;
        }
    }
}