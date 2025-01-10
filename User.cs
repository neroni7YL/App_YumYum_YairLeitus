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
    [Table("User")]
    public class User
    {
        [PrimaryKey, Column("Username")]
        public string username { get; set; } // שם המשתמש
        [Column("password")]
        public string password { get; set; } // סיסמא
        [Column("name")]
        public string name { get; set; } // שם פרטי
        [Column("email")]
        public string email { get; set; } // אימייל המשתמש
        [Column("securityQuestion")]
        public string securityQuestion { get; set; } // תשובה על שאלת אבטחה
        [Column("userImage")]
        public string userImage { get; set; } // תמונת הפרופיל
        [Column("favoriteRecipesId")]
        public string favoriteRecipesId { get; set; } // מחרוזת קודי המתכונים האהובים
        [Column("myRecipesId")]
        public string myRecipesId { get; set; } // מחרוזת קודי המתכונים שהמשתמש יצר
        [Column("allowNotification")]
        public bool allowNotification { get; set; } // משתנה בוליאני שמציין אם המשתמש מעוניין בשירות ההודעות
        [Column("isAdmin")]
        public bool isAdmin { get; set; } // משתנה בוליאני שמציין אם המשתמש הוא מנהל

        public User()
        {

        }

        public User(string username, string password, string name, string email, string securityQuestion, bool isAdmin)
        {
            this.username = username;
            this.password = password;
            this.name = name;
            this.email = email;
            this.securityQuestion = securityQuestion;
            this.isAdmin = isAdmin;
            this.userImage = "";
            this.favoriteRecipesId = "";
            this.myRecipesId = "";
            this.allowNotification = true;
        }
    }
}