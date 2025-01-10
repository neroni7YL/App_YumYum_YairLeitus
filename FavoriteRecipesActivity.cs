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
    [Activity(Label = "FavoriteRecipesActivity")]
    public class FavoriteRecipesActivity : Activity, AdapterView.IOnItemClickListener
    {
        // רשימת כל המתכונים האהובים של המשתמש
        private List<Recipe> recipeList;

        // רשימת המתכונים האהובים המתעדכנת לפי סינון או חיפוש
        private List<Recipe> currentRecipes;

        // currentRecipes אדפטר של המתכונים לפי רשימת
        private RecipeAdapter recipeAdapter;

        // המכיל בתוכו את כל המתכונים שהמשתמש רואה במסך ListView 
        private ListView lvRecipes;

        // currentRecipes רשימת שמות מתכונים המתעדכנת לפי 
        private List<string> recipeNameList;

        // אדפטר של שמות המתכונים שמאפשר הצגת שמות המתכונים ברגע החיפוש בהתאמה
        private ArrayAdapter recipeNameAdapter;

        // ומציג מתחתיו את שמות המתכונים לפי החיפוש EditText המתפקד כמו AutoCompleteTextView פקד מטיפוס 
        private AutoCompleteTextView actvSearch;

        // actvSearch כפתור לניקוי טקסט בסרגל החיפוש
        private Button btnClear;

        // כפתור לחיפוש המתכונים בהתאם לטקסט שבסרגל החיפוש
        private Button btnSearch;

        // שמכיל אפשרויות סינון DialogFragmnet כפתור שפותח 
        private Button btnSort;

        // כפתור חזרה לדף הבית
        private Button btnGoBack;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.favoriteRecipes_layout);

            lvRecipes = FindViewById<ListView>(Resource.Id.favoriteRecipes_lvRecipes);
            lvRecipes.OnItemClickListener = this; // Click on view - recipe

            actvSearch = FindViewById<AutoCompleteTextView>(Resource.Id.favoriteRecipes_actvSearch);
            btnClear = FindViewById<Button>(Resource.Id.favoriteRecipes_btnClear);
            btnSearch = FindViewById<Button>(Resource.Id.favoriteRecipes_btnSearch);
            btnSort = FindViewById<Button>(Resource.Id.favoriteRecipes_btnSort);
            btnGoBack = FindViewById<Button>(Resource.Id.favoriteRecipes_btnGoBack);

            btnClear.Click += BtnClear_Click;
            btnSearch.Click += BtnSearch_Click;
            btnSort.Click += BtnSort_Click;
            btnGoBack.Click += delegate { Finish(); };
        }

        /* actvSearch-פעולת עזר שמטרתה לעדכן את האדפטר של שמות המתכונים כך ש
         יראה את שמות המתכונים הרלוונטים כאשר ישנו סינון לפי קטגוריות או סוג מתכון ובפתיחת הדף */
        private void UpdateRecipeNameAdapter()
        {
            // על רשימה חדשה recipeNameList כל פעם שהפעולה מופעלת יצביע
            recipeNameList = new List<string>();

            // recipeNameList מעבר על כל המתכונים האהובים והוספת שמות המתכונים לרשימת   
            for (int i = 0; i < recipeList.Count; i++)
            {
                recipeNameList.Add(recipeList[i].recipeName);
            }

            // אשר מייצג אדפטר של שמות המתכונים ArrayAdapter יצירת 
            recipeNameAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, recipeNameList);

            // כך שסרגל החיפוש מראה את שמות המתכונים actvSearch.Adapterהשמת האדפטר הנ"ל ב
            actvSearch.Adapter = recipeNameAdapter;
        }

        /* OnCreaete() פעולה שמתרחשת לאחר  
         כך שאם המשתמש עדכן או מחק מתכון אהוב OnRestart() הפעולה נקראת לאחר 
         כשהוא יחזור לדף הוא יראה את השינוי */
        protected override void OnStart()
        {
            // makes the activity visible to the user, as the app prepares for the activity to enter the foreground and become interactive
            // this method is where the app initializes the code that maintains the UI
            base.OnStart();

            actvSearch.Text = "";

            // קבלת כל המתכונים האהובים ממחלקת עזר  
            recipeList = DatabaseManager.GetAllFavoriteRecipes();

            // סידור המתכונים לפי סדר אלפיביתי בכל פתיחה חדשה של הדף
            recipeList = (from recipe in recipeList orderby recipe.recipeName select recipe).ToList();

            // הרשימה המתעדכנת מכילה תחילה את כל המתכונים
            currentRecipes = recipeList.ToList();

            // כך שהמתכונים מוצגים למשתמש lvRecipes.Adapterיצירת אדפטר המתכונים והשמתו ב
            recipeAdapter = new RecipeAdapter(this, currentRecipes);
            lvRecipes.Adapter = recipeAdapter;
            
            // שיציג את כל המתכונים בהתאמה actvSearch עדכון
            UpdateRecipeNameAdapter();
        }

        /* פעולת עזר שמסדרת את המתכונים שבדף לפי רשימת סינון מתכונים היא קולטת
         מאחר וישנו שימוש בה במחלקות אחרות public */
        public void ArrangeRecipes(List<Recipe> sortedRecipes)
        {
            actvSearch.Text = "";

            // רשימת כל המתכונים מצביעה על רשימת המתכונים המסוננת
            recipeList = sortedRecipes;
            currentRecipes = recipeList.ToList();

            // שינוי האדפטר לפי רשימת המתכונים המתעדכנים החדשה ועדכון המתכונים המופיעים בדף
            recipeAdapter.SetRecipes(currentRecipes);
            recipeAdapter.NotifyDataSetChanged();

            // שינוי שמות המתכונים בסרגל החיפוש לפי המתכונים המסוננים
            UpdateRecipeNameAdapter();
        }

        /* לאחר בחירת קטגוריות לסינון CategoryDialogFragment פעולה שנקראת ב  
         הפעולה מקבלת את כל המתכונים האהובים ומסננת כך שרשימת המתכונים
         תכיל מתכונים בעלי אותם קטגוריות כפי שקלטה */
        public void ReceiveCategories(string categories)
        {
            recipeList = DatabaseManager.GetAllFavoriteRecipes();

            /* סינון הרשימה לרשימה חדשה כך שתכונת הקטגוריות של כל מתכון
             תכלול את מחרוזת הקטגוריות שהפעולה קלטה
             כלומר המתכונים יכילו רק את הקטגוריות שבמחרוזת הנ"ל, */
            recipeList = (from recipe in recipeList orderby recipe.recipeName where recipe.category.Contains(categories) select recipe).ToList();
            recipeList = (from recipe in recipeList orderby recipe.recipeName select recipe).ToList();
            // עדכון המתכונים המוצגים בדף
            ArrangeRecipes(recipeList);
        }

        /* פעולה הנקראת לאחר סיום בחירת נתוני סוג המתכון למטרת סינון
         PastryDialogFragment או MeatDialogFragment ,SoupDialogFragment ,SaladDialogFragment הפעולה מופעלת ב
         הפעולה מקבלת את סוג המתכון ומערך עם המידע על סוג המתכון לסינון */
        public void ReceiveRecipeTypeDetails(string recipeType, string[] details)
        {
            // בדיקה איזו סוג מתכון נבחר
            if (recipeType == "Salad")
            {
                // קבלת כל הסלטים האהובים 
                recipeList = DatabaseManager.GetAllFavoriteSalads();
                // סינון הרשימה על פי נתוני הסלטים שנבחרו
                recipeList = (from recipe in recipeList
                              orderby recipe.recipeName
                              where ((Salad)recipe).isFruit == details[0]
                              && ((Salad)recipe).saladType == details[1]
                              && ((Salad)recipe).isMeat == details[2]
                              && ((Salad)recipe).isFlavoring == details[3]
                              select recipe).ToList();
            }
            if (recipeType == "Soup")
            {
                // קבלת כל המרקים האהובים 
                recipeList = DatabaseManager.GetAllFavoriteSoups();
                // סינון הרשימה על פי נתוני המרקים שנבחרו
                recipeList = (from recipe in recipeList
                              orderby recipe.recipeName
                              where ((Soup)recipe).soupType == details[0]
                              && ((Soup)recipe).duration == details[1]
                              select recipe).ToList();
            }
            if (recipeType == "Meat")
            {
                // קבלת כל הבשרים האהובים 
                recipeList = DatabaseManager.GetAllFavoriteMeats();
                // סינון הרשימה על פי נתוני הבשרים שנבחרו
                recipeList = (from recipe in recipeList
                              orderby recipe.recipeName
                              where ((Meat)recipe).meatType == details[0]
                              && ((Meat)recipe).method == details[1]
                              && ((Meat)recipe).doneness == details[2]
                              select recipe).ToList();
            }
            if (recipeType == "Pastry")
            {
                // קבלת כל המאפים האהובים 
                recipeList = DatabaseManager.GetAllFavoritePastries();
                // סינון הרשימה על פי נתוני המאפים שנבחרו
                recipeList = (from recipe in recipeList
                              orderby recipe.recipeName
                              where ((Pastry)recipe).isGluten == details[0]
                              && ((Pastry)recipe).kosher == details[1]
                              select recipe).ToList();
            }
            recipeList = (from recipe in recipeList orderby recipe.recipeName select recipe).ToList();
            // עדכון המתכונים המוצגים בדף
            ArrangeRecipes(recipeList);
        }

        // פעולה שנקראת לאחר לחיצה על כפתור ניקוי סרגל החיפוש
        private void BtnClear_Click(object sender, EventArgs e)
        {
            // מחיקת טקסט המופיע בסרגל החיפוש
            actvSearch.Text = "";

            // מצביעה על רשימה עם כל המתכונים currentRecipes
            currentRecipes = recipeList.ToList();

            // עדכון אדפטר המתכונים ועדכון המתכונים בדף
            recipeAdapter.SetRecipes(currentRecipes);
            recipeAdapter.NotifyDataSetChanged();
        }

        // פעולה שנקראת לאחר לחיצה על כפתור חיפוש מתכונים
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            // foreeach variable in list it selects the right variables and puts them in a list
            // מצביעה על רשימה כך ששמות המתכונים מתחילים בטקסט שהוקלד, אין הפרדה לאותיות קטנות וגדולות בחיפוש currentRecipes
            currentRecipes = (from recipe in recipeList where recipe.recipeName.StartsWith(actvSearch.Text, StringComparison.OrdinalIgnoreCase) select recipe).ToList();

            // עדכון אדפטר המתכונים ועדכון המתכונים בדף
            recipeAdapter.SetRecipes(currentRecipes);
            recipeAdapter.NotifyDataSetChanged();
        }

        // המכיל את אפשרויות הסינון SortByDialogFragment פתיחת 
        private void BtnSort_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            SortByDialogFragment sortByDialogFragment = new SortByDialogFragment();
            sortByDialogFragment.Show(transaction, "Sort By dialog fragment");
        }

        // מעבר לדף המתכון ע"י לחיצה על מתכון כלשהו
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            Intent intent = new Intent(this, typeof(RecipePageActivity));
            // לחילוץ נתונים על המתכון PutExtra שליחת קוד המתכון באמצעות
            intent.PutExtra("RecipeId", recipeList[position].recipeId);
            StartActivity(intent);
        }
    }
}