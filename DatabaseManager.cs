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
    public static class DatabaseManager
    {
        // קבוע שמכיל את שם מסד הנתונים של האפליקציה
        private const string DATABASE_NAME = "DatabaseYumYum";

        // Path הפותח את מסד הנתונים לפי SQLite משתנה סטטי המייצג את ההתחברות למסד הנתונים 
        private static SQLiteConnection connection = new SQLiteConnection(Path());

        // פעולה שיוצרת את המסלול למסד הנתונים
        private static string Path()
        {
            // מביא את התיקייה של המסד
            string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), DATABASE_NAME);
            // ניגש לתיקייה פנימית בטלפון ומחזיר את הנתונים משם
            return path;
        }

        // יצירת טבלאות משתמשים ומתכונים במידה ואינם קיימים
        public static void CreateTables()
        {
            connection.CreateTable<User>();
            connection.CreateTable<Recipe>();
            connection.CreateTable<Salad>();
            connection.CreateTable<Soup>();
            connection.CreateTable<Meat>();
            connection.CreateTable<Pastry>();
        }

        // Recipe
        // פעולה המחזירה מתכון המצוי במסד הנתונים לפי קוד מתכון שאותו היא קולטת
        public static Recipe GetRecipe(string recipeId)
        {
            string strsql;

            // המתכון המוחזר
            Recipe recipe = null;

            /* בדיקה באיזה סוג מתכון קוד המתכון מתחיל 
             לאחר הבדיקה בוחר את המתכון לפי קוד המתכון בטבלת סוג המתכון המתאים 
             לאחר שאילתה ממסד הנתונים עצם מטיפוס מתכון מצביע עליו ומוחזר */
            if (recipeId.StartsWith("Salad"))
            {
                strsql = string.Format("SELECT * FROM Salad WHERE RecipeId='{0}'", recipeId);
                recipe = connection.Query<Salad>(strsql)[0];
            }
            if (recipeId.StartsWith("Soup"))
            {
                strsql = string.Format("SELECT * FROM Soup WHERE RecipeId='{0}'", recipeId);
                recipe = connection.Query<Soup>(strsql)[0];
            }
            if (recipeId.StartsWith("Meat"))
            {
                strsql = string.Format("SELECT * FROM Meat WHERE RecipeId='{0}'", recipeId);
                recipe = connection.Query<Meat>(strsql)[0];
            }
            if (recipeId.StartsWith("Pastry"))
            {
                strsql = string.Format("SELECT * FROM Pastry WHERE RecipeId='{0}'", recipeId);
                recipe = connection.Query<Pastry>(strsql)[0];
            }
            return recipe;
        }

        // פעולה המעדכנת מתכון המצוי במסד הנתונים לפי אובייקט מתכון שאותו היא קולטת
        public static void SetRecipe(Recipe recipe)
        {
            if (recipe is Salad)
            {
                connection.Update((Salad)recipe);
            }
            if (recipe is Soup)
            {
                connection.Update((Soup)recipe);
            }
            if (recipe is Meat)
            {
                connection.Update((Meat)recipe);
            }
            if (recipe is Pastry)
            {
                connection.Update((Pastry)recipe);
            }
        }

        // פעולה המוסיפה מתכון למסד הנתונים לפי אובייקט מתכון שאותו היא קולטת
        public static void AddRecipe(Recipe recipe)
        {
            if (recipe is Salad)
            {
                connection.Insert((Salad)recipe);
            }
            if (recipe is Soup)
            {
                connection.Insert((Soup)recipe);
            }
            if (recipe is Meat)
            {
                connection.Insert((Meat)recipe);
            }
            if (recipe is Pastry)
            {
                connection.Insert((Pastry)recipe);
            }
        }

        // פעולה המוחקת מתכון המצוי במסד הנתונים לפי אובייקט מתכון שאותו היא קולטת
        public static void DeleteRecipe(Recipe recipe)
        {
            if (recipe is Salad)
            {
                connection.Delete((Salad)recipe);
            }
            if (recipe is Soup)
            {
                connection.Delete((Soup)recipe);
            }
            if (recipe is Meat)
            {
                connection.Delete((Meat)recipe);
            }
            if (recipe is Pastry)
            {
                connection.Delete((Pastry)recipe);
            }
        }

        // פעולה בוליאנית הבודקת האם המתכון קיים במסד הנתונים לפי קוד המתכון שהיא קולטת 
        public static bool IsExistRecipe(string recipeId)
        {
            string strsql = "";

            /* ישנה בדיקה באיזה סוג מתכון קוד המתכון מתחיל
             לאחר מכן מתבצעת שאילתה 
             וישנה בדיקה האם המופע הנ"ל מופיע פעם אחת כלומר האם המתכון קיים במסד */
            if (recipeId.StartsWith("Salad"))
            {
                strsql = string.Format("SELECT * FROM Salad WHERE RecipeId='{0}'", recipeId);
                if (connection.Query<Salad>(strsql).Count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (recipeId.StartsWith("Soup"))
            {
                strsql = string.Format("SELECT * FROM Soup WHERE RecipeId='{0}'", recipeId);
                if (connection.Query<Soup>(strsql).Count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (recipeId.StartsWith("Meat"))
            {
                strsql = string.Format("SELECT * FROM Meat WHERE RecipeId='{0}'", recipeId);
                if (connection.Query<Meat>(strsql).Count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (recipeId.StartsWith("Pastry"))
            {
                strsql = string.Format("SELECT * FROM Pastry WHERE RecipeId='{0}'", recipeId);
                if (connection.Query<Pastry>(strsql).Count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /* מקבלת מחרוזת קודי מתכונים, ממירה את המחרזות לרשימה המכילה את הקודים ומחזירה את רשימת המתכונים על פי קוד הזיהוי המתאים לכל מתכון
         משמש לקבלת רשימת המתכונים האהובים והמתכונים שהועלו בלבד */
        public static List<Recipe> GetRecipesById(string recipesId)
        {
            /* המרת מחרוזת הקודים המופרדים בפסיקים לרשימה אשר מכילה את הקודים
             StringSplitOptions.RemoveEmptyEntries מאחר והתו אחרי הפסיק האחרון הוא מחרוזת ריקה אזי יש להשתמש ב
             על מנת שהרשימה תכיל את קודי המתכונים בלבד */
            List<string> recipeIdList = recipesId.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            // רשימת המתכונים שתכיל את המתכונים לפי קודי המתכונים  
            List<Recipe> recipeList = new List<Recipe>();
            string strsql;

            // משמש כאובייקט כללי למתכון
            Recipe recipe = null;

            /* לולאה שעוברת על רשימת קודי המתכונים ועבור כל קוד בודקת באיזה סוג מתכון קוד המתכון מתחיל 
             מבצעת שאילתה מהטבלה המתאימה במסד הנתונים ולפי קוד המתכון ומקבלת את המתכון הנ"ל 
             בסוף היא מוסיפה את המתכון לרשימת המתכונים */
            for (int i = 0; i < recipeIdList.Count; i++)
            {
                if (recipeIdList[i].StartsWith("Salad"))
                {
                    strsql = string.Format("SELECT * FROM Salad WHERE RecipeId='{0}'", recipeIdList[i]);
                    recipe = connection.Query<Salad>(strsql)[0];
                }
                if (recipeIdList[i].StartsWith("Soup"))
                {
                    strsql = string.Format("SELECT * FROM Soup WHERE RecipeId='{0}'", recipeIdList[i]);
                    recipe = connection.Query<Soup>(strsql)[0];
                }
                if (recipeIdList[i].StartsWith("Meat"))
                {
                    strsql = string.Format("SELECT * FROM Meat WHERE RecipeId='{0}'", recipeIdList[i]);
                    recipe = connection.Query<Meat>(strsql)[0];
                }
                if (recipeIdList[i].StartsWith("Pastry"))
                {
                    strsql = string.Format("SELECT * FROM Pastry WHERE RecipeId='{0}'", recipeIdList[i]);
                    recipe = connection.Query<Pastry>(strsql)[0];
                }
                recipeList.Add(recipe);
            }
            return recipeList;
        }

        // פעולה שמחזירה את כל המתכונים שבאפליקציה משומשת בדף הבית להצגת כל המתכונים
        public static List<Recipe> GetAllRecipes()
        {
            // רשימת כל המתכונים אשר מתווספים אליה ערכים בעזרת פעולות העזר הבאות
            List<Recipe> recipeList = new List<Recipe>();
            recipeList.AddRange(GetAllSalads());
            recipeList.AddRange(GetAllSoups());
            recipeList.AddRange(GetAllMeats());
            recipeList.AddRange(GetAllPastries());
            return recipeList;
        }

        // פעולה המחזירה את כל הסלטים שבמסד הנתונים ברשימה מטיפוס מתכון
        public static List<Recipe> GetAllSalads()
        {
            // רשימה מטיפוס מתכון שמייצגת את כל הסלטים
            List<Recipe> saladList = new List<Recipe>();

            /* מתבצעת שאילתה מטבלת הסלטים שבמסד הנתונים 
             saladList הסלטים נשמרים ברשימה מטיפוס סלט ואם ישנם סלטים הם מוספים לרשימה
             ישנה המרה כלפי מעלה מטיפוס סלט למתכון כאשר הסלטים מוספים לרשימה */
            string strsql = string.Format("SELECT * FROM Salad");
            List<Salad> salads = connection.Query<Salad>(strsql);
            if (salads.Count > 0)
            {
                foreach (Salad salad in salads)
                {
                    saladList.Add(salad);
                }
            }
            return saladList;
        }

        // פעולה המחזירה את כל המרקים שבמסד הנתונים ברשימה מטיפוס מתכון
        public static List<Recipe> GetAllSoups()
        {
            // רשימה מטיפוס מתכון שמייצגת את כל המרקים
            List<Recipe> soupList = new List<Recipe>();

            /* מתבצעת שאילתה מטבלת המרקים שבמסד הנתונים 
             soupList המרקים נשמרים ברשימה מטיפוס מרק ואם ישנם מרקים הם מוספים לרשימה
             ישנה המרה כלפי מעלה מטיפוס מרק למתכון כאשר המרקים מוספים לרשימה */
            string strsql = string.Format("SELECT * FROM Soup");
            List<Soup> soups = connection.Query<Soup>(strsql);
            if (soups.Count > 0)
            {
                foreach (Soup soup in soups)
                {
                    soupList.Add(soup);
                }
            }
            return soupList;
        }

        // פעולה המחזירה את כל הבשרים שבמסד הנתונים ברשימה מטיפוס מתכון
        public static List<Recipe> GetAllMeats()
        {
            // רשימה מטיפוס מתכון שמייצגת את כל הבשרים
            List<Recipe> meatList = new List<Recipe>();

            /* מתבצעת שאילתה מטבלת הבשרים שבמסד הנתונים 
             meatList הבשרים נשמרים ברשימה מטיפוס בשר ואם ישנם בשרים הם מוספים לרשימה
             ישנה המרה כלפי מעלה מטיפוס בשר למתכון כאשר הבשרים מוספים לרשימה */
            string strsql = string.Format("SELECT * FROM Meat");
            var meats = connection.Query<Meat>(strsql);
            if (meats.Count > 0)
            {
                foreach (Meat meat in meats)
                {
                    meatList.Add(meat);
                }
            }
            return meatList;
        }

        // פעולה המחזירה את כל המאפים שבמסד הנתונים ברשימה מטיפוס מתכון
        public static List<Recipe> GetAllPastries()
        {
            // רשימה מטיפוס מתכון שמייצגת את כל המאפים
            List<Recipe> pastryList = new List<Recipe>();

            /* מתבצעת שאילתה מטבלת המאפים שבמסד הנתונים 
             pastryList המאפים נשמרים ברשימה מטיפוס מאפה ואם ישנם מאפים הם מוספים לרשימה
             ישנה המרה כלפי מעלה מטיפוס מאפה למתכון כאשר המאפים מוספים לרשימה */
            string strsql = string.Format("SELECT * FROM Pastry");
            var pastries = connection.Query<Pastry>(strsql);
            if (pastries.Count > 0)
            {
                foreach (Pastry pastry in pastries)
                {
                    pastryList.Add(pastry);
                }
            }
            return pastryList;
        }

        // פעולה המחזירה את כל המתכונים האהובים של המשתמש המחובר, משומשת בדף המתכונים האהובים
        public static List<Recipe> GetAllFavoriteRecipes()
        {
            // SharedPreferencesManager קבלת המשתמש המחובר בעזרת פעולות עזר מהמחלקה הנ"ל ומחלקת 
            User loggedUser = GetUser(SharedPreferencesManager.GetUsername());
            /* GetRecipesById רשימת המתכונים האהובים, שימוש בפעולת העזר
             תוך שליחת קודי המתכונים האהובים של המשתמש המחובר */
            List<Recipe> recipeList = GetRecipesById(loggedUser.favoriteRecipesId);
            return recipeList;
        }

        // פעולה המחזירה את כל הסלטים האהובים של המשתמש המחובר
        public static List<Recipe> GetAllFavoriteSalads()
        {
            // רשימת המתכונים האהובים של המשתמש המחובר
            List<Recipe> recipeList = GetAllFavoriteRecipes();

            // saladList-מעבר על כל המתכונים האהובים והצבתם של סלטים ב 
            List<Recipe> saladList = new List<Recipe>();
            for (int i = 0; i < recipeList.Count; i++)
            {
                if (recipeList[i] is Salad)
                {
                    saladList.Add(recipeList[i]);
                }
            }
            return saladList;
        }

        // פעולה המחזירה את כל המרקים האהובים של המשתמש המחובר
        public static List<Recipe> GetAllFavoriteSoups()
        {
            // רשימת המתכונים האהובים של המשתמש המחובר
            List<Recipe> recipeList = GetAllFavoriteRecipes();

            // soupList-מעבר על כל המתכונים האהובים והצבתם של מרקים ב 
            List<Recipe> soupList = new List<Recipe>();
            for (int i = 0; i < recipeList.Count; i++)
            {
                if (recipeList[i] is Soup)
                {
                    soupList.Add(recipeList[i]);
                }
            }
            return soupList;
        }

        // פעולה המחזירה את כל מנות הבשר האהובים של המשתמש המחובר
        public static List<Recipe> GetAllFavoriteMeats()
        {
            // רשימת המתכונים האהובים של המשתמש המחובר
            List<Recipe> recipeList = GetAllFavoriteRecipes();

            // meatList-מעבר על כל המתכונים האהובים והצבתם של בשרים ב 
            List<Recipe> meatList = new List<Recipe>();
            for (int i = 0; i < recipeList.Count; i++)
            {
                if (recipeList[i] is Meat)
                {
                    meatList.Add(recipeList[i]);
                }
            }
            return meatList;
        }

        // פעולה המחזירה את כל המאפים האהובים של המשתמש המחובר
        public static List<Recipe> GetAllFavoritePastries()
        {
            // רשימת המתכונים האהובים של המשתמש המחובר
            List<Recipe> recipeList = GetAllFavoriteRecipes();

            // pastryList-מעבר על כל המתכונים האהובים והצבתם של מאפים ב 
            List<Recipe> pastryList = new List<Recipe>();
            for (int i = 0; i < recipeList.Count; i++)
            {
                if (recipeList[i] is Pastry)
                {
                    pastryList.Add(recipeList[i]);
                }
            }
            return pastryList;
        }

        // פעולה המחזירה את כל המתכונים שמשתמש המחובר יצר, משומשת בדף המתכונים שלי
        public static List<Recipe> GetAllMyRecipes()
        {
            // SharedPreferencesManager קבלת המשתמש המחובר בעזרת פעולות עזר מהמחלקה הנ"ל ומחלקת 
            User loggedUser = GetUser(SharedPreferencesManager.GetUsername());
            /*  GetRecipesById רשימת המתכונים שלי, שימוש בפעולת העזר
             תוך שליחת קודי המתכונים שמשתמש המחובר יצר */
            List<Recipe> recipeList = GetRecipesById(loggedUser.myRecipesId);
            return recipeList;
        }

        // פעולה המחזירה את כל הסלטים שמשתמש המחובר יצר
        public static List<Recipe> GetAllMySalads()
        {
            // רשימת המתכונים  שהמשתמש המחובר יצר
            List<Recipe> recipeList = GetAllMyRecipes();

            // saladList-מעבר על כל המתכונים שהמשתמש יצר והצבתם של סלטים ב 
            List<Recipe> saladList = new List<Recipe>();
            for (int i = 0; i < recipeList.Count; i++)
            {
                if (recipeList[i] is Salad)
                {
                    saladList.Add(recipeList[i]);
                }
            }
            return saladList;
        }

        // פעולה המחזירה את כל המרקים שמשתמש המחובר יצר
        public static List<Recipe> GetAllMySoups()
        {
            // רשימת המתכונים  שהמשתמש המחובר יצר
            List<Recipe> recipeList = GetAllMyRecipes();

            // soupList-מעבר על כל המתכונים שהמשתמש יצר והצבתם של מרקים ב 
            List<Recipe> soupList = new List<Recipe>();
            for (int i = 0; i < recipeList.Count; i++)
            {
                if (recipeList[i] is Soup)
                {
                    soupList.Add(recipeList[i]);
                }
            }
            return soupList;
        }

        // פעולה המחזירה את כל מנות הבשר שמשתמש המחובר יצר
        public static List<Recipe> GetAllMyMeats()
        {
            // רשימת המתכונים  שהמשתמש המחובר יצר
            List<Recipe> recipeList = GetAllMyRecipes();

            // meatList-מעבר על כל המתכונים שהמשתמש יצר והצבתם של בשרים ב 
            List<Recipe> meatList = new List<Recipe>();
            for (int i = 0; i < recipeList.Count; i++)
            {
                if (recipeList[i] is Meat)
                {
                    meatList.Add(recipeList[i]);
                }
            }
            return meatList;
        }

        // פעולה המחזירה את כל המאפים שמשתמש המחובר יצר
        public static List<Recipe> GetAllMyPastries()
        {
            // רשימת המתכונים  שהמשתמש המחובר יצר
            List<Recipe> recipeList = GetAllMyRecipes();

            // pastryList-מעבר על כל המתכונים שהמשתמש יצר והצבתם של מאפים ב 
            List<Recipe> pastryList = new List<Recipe>();
            for (int i = 0; i < recipeList.Count; i++)
            {
                if (recipeList[i] is Pastry)
                {
                    pastryList.Add(recipeList[i]);
                }
            }
            return pastryList;
        }

        // User
        // פעולה המחזירה משתמש המצוי במסד הנתונים לפי שם המשתמש שאותו היא קולטת
        public static User GetUser(string username)
        {
            string strsql = string.Format("SELECT * FROM User WHERE Username='{0}'", username);
            User user = connection.Query<User>(strsql)[0];
            return user;
        }

        // פעולה המעדכנת משתמש המצוי במסד הנתונים לפי אובייקט המשתמש שאותו היא קולטת
        public static void SetUser(User user)
        {
            connection.Update(user);
        }

        // פעולה המוסיפה משתמש למסד הנתונים לפי אובייקט משתמש שאותו היא קולטת
        public static void AddUser(User user)
        {
            connection.Insert(user);
        }

        // פעולה המוחקת משתמש המצוי במסד הנתונים לפי אובייקט המשתמש שאותו היא קולטת
        public static void DeleteUser(User user)
        {
            connection.Delete(user);
        }

        // פעולה המחזירה אמת במידה ושם המשתמש המוכנס קיים במסד נתוני המשתמשים, אחרת תחזיר שקר
        public static bool IsExistUsername(string username)
        {
            string strsql = string.Format("SELECT * FROM User WHERE Username='{0}'", username);
            if (connection.Query<User>(strsql).Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // פעולה המחזירה אמת במידה והאימייל המוכנס קיים במסד נתוני המשתמשים, אחרת תחזיר שקר
        public static bool IsExistEmail(string email)
        {
            string strsql = string.Format("SELECT * FROM User WHERE email='{0}'", email);
            if (connection.Query<User>(strsql).Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // פעולה המחזירה אמת במידה וקיים משתמש התואם את שם המשתמש והסיסמא, אחרת תחזיר שקר
        public static bool IsValidUser(string username, string password)
        {
            string strsql = string.Format("SELECT * FROM User WHERE Username='{0}' AND password='{1}'", username, password);
            if (connection.Query<User>(strsql).Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // פעולה המחזירה אמת במידה וקיים משתמש התואם את שם המשתמש, האימייל ותשובת שאלת ההבטחה; אחרת תחזיר שקר
        public static bool IsValidUser(string username, string email, string securityQuestion)
        {
            string strsql = string.Format("SELECT * FROM User WHERE Username='{0}' AND email='{1}' AND securityQuestion='{2}'", username, email, securityQuestion);
            if (connection.Query<User>(strsql).Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // פעולה שמחזירה את כל המשתמשים הרשומים לאפליקציה, משומשת בדף רשימת המשתמשים להצגת כל המתכונים
        public static List<User> GetAllUsers()
        {
            // רשימה שתכיל את כל המשתמשים
            List<User> userList = new List<User>();
            string strsql = string.Format("SELECT * FROM User");
            List<User> users = connection.Query<User>(strsql);
            if (users.Count > 0)
            {
                foreach (User user in users)
                {
                    userList.Add(user);
                }
            }
            return userList;
        }
    }
}