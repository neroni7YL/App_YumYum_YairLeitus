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
    [Table("Salad")]
    public class Salad : Recipe
    {
        [Column("isFruit")]
        public string isFruit { get; set; } // סלט ירקות או פירות
        [Column("saladType")]
        public string saladType { get; set; } // סוג סלט
        [Column("isMeat")]
        public string isMeat { get; set; } // האם מכיל בשר
        [Column("isFlavoring")]
        public string isFlavoring { get; set; } // האם מכיל טיבול

        public Salad()
        {

        }

        public Salad(string isFruit, string saladType, string isMeat, string isFlavoring, string recipeId, string recipeName, string recipeUsername, string recipeImage, string category, int hours, int minutes, string description, string ingredients, string instructions) : base(recipeId, recipeName, recipeUsername, recipeImage, category, hours, minutes, description, ingredients, instructions)
        {
            this.isFruit = isFruit;
            this.saladType = saladType;
            this.isMeat = isMeat;
            this.isFlavoring = isFlavoring;
        }
    }
}