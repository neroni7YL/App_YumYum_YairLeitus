using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace App_YumYum_YairLeitus
{
    [Activity(Label = "HomepageActivity")]
    public class HomepageActivity : AppCompatActivity, AdapterView.IOnItemClickListener
    {
        // רשימה המכילה את כל המתכונים ששמורים במסד הנתונים או את המתכונים לפי הסינון 
        private List<Recipe> recipeList;

        // רשימת המתכונים האהובים המתעדכנת לפי סינון או חיפוש 
        private List<Recipe> currentRecipes;

        // currentRecipes -אדפטר של המתכונים הנוכחים לפי הרשימה ב 
        private RecipeAdapter recipeAdapter;

        // recipeAdapter המציג את המתכונים הנוכחים לפי ListView
        private ListView lvRecipes;

        // רשימה המכילה את שמות כל המתכונים או את שמות המתכונים לפי הסינון
        private List<string> recipeNameList;

        // אדפטר של רשימת שמות המתכונים אשר בעזרתו יוצגו מתחת לסרגל החיפוש בעת כתיבת טקסט
        private ArrayAdapter recipeNameAdapter;

        // סרגל החיפוש אשר ניתן לכתוב בו טקסט ומתחת יופיעו אפשרויות של מתכונים לפי הטקסט
        private AutoCompleteTextView actvSearch;

        // כפתור מחיקת חיפוש
        private Button btnClear;

        // כפתור חיפוש מתכונים על פי הטקסט שנכתב בסרגל החיפוש
        private Button btnSearch;

        // כפתור אשר פותח את התפריט של הדף הראשי
        private Button btnMenu;

        // BatteryBroadcastReceiver
        // אשר מראה את כמות הסוללה במכשיר TextView פקד
        private TextView tvBattery;

        // BatteryBroadcastReceiver אובייקט מטיפוס 
        private BatteryBroadcastReceiver batteryBroadcastReceiver;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.homepage_layout);

            // BatteryBroadcastReceiver
            tvBattery = FindViewById<TextView>(Resource.Id.homepage_tvBattery);
            // BatteryBroadcastReceiver יצירת מופע של
            batteryBroadcastReceiver = new BatteryBroadcastReceiver(tvBattery);

            lvRecipes = FindViewById<ListView>(Resource.Id.homepage_lvRecipes);
            lvRecipes.OnItemClickListener = this; // Click on view - recipe

            actvSearch = FindViewById<AutoCompleteTextView>(Resource.Id.homepage_actvSearch);
            btnClear = FindViewById<Button>(Resource.Id.homepage_btnClear);
            btnSearch = FindViewById<Button>(Resource.Id.homepage_btnSearch);
            btnMenu = FindViewById<Button>(Resource.Id.homepage_btnMenu);

            // מגדיר את הפקד כמפעיל את התפריט
            RegisterForContextMenu(btnMenu);

            btnClear.Click += BtnClear_Click;
            btnSearch.Click += BtnSearch_Click;
            btnMenu.Click += BtnMenu_Click;
        }

        /* actvSearch-פעולת עזר שמטרתה לעדכן את האדפטר של שמות המתכונים כך ש
         יראה את שמות המתכונים הרלוונטים כאשר ישנו סינון לפי קטגוריות או סוג מתכון ובפתיחת הדף */
        private void UpdateRecipeNameAdapter()
        {
            // על רשימה חדשה recipeNameList כל פעם שהפעולה מופעלת יצביע
            recipeNameList = new List<string>();

            // recipeNameList מעבר על כל המתכונים והוספת שמות המתכונים לרשימת   
            for (int i = 0; i < recipeList.Count; i++)
            {
                recipeNameList.Add(recipeList[i].recipeName);
            }

            // אשר מייצג אדפטר של שמות המתכונים ArrayAdapter יצירת 
            recipeNameAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, recipeNameList);

            // כך שסרגל החיפוש מראה את שמות המתכונים actvSearch.Adapterהשמת האדפטר הנ"ל ב
            actvSearch.Adapter = recipeNameAdapter;
        }

        // פעולה שמקבלת כפרמטר אובייקט מטיפוס תפריט מתאים ודואגת שיוצג
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);

            // זימון פעולת המערכת ש"מנפחת" ומציגה את התפריט
            MenuInflater.Inflate(Resource.Menu.homepage_menu, menu);

            // בדיקה שרק מנהל האפליקציה יוכל לגשת לרשימת המשתמשים דרך התפריט הראשי
            if (DatabaseManager.GetUser(SharedPreferencesManager.GetUsername()).isAdmin == false)
            {
                // הסרת עמודה שמפנה לתפריט הראשי כאשר המשתמש הוא לא מנהל
                menu.RemoveItem(Resource.Id.homepageMenu_userList);
            }
        }
        // תגובה על לחיצה על שורת תפריט כלשהי
        public override bool OnContextItemSelected(IMenuItem item)
        {
            // לבחירת סוג הסינון SortByDialogFragment פתיחת 
            if (item.ItemId == Resource.Id.homepageMenu_sortBy)
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                SortByDialogFragment sortByDialogFragment = new SortByDialogFragment();
                sortByDialogFragment.Show(transaction, "Sort By dialog fragment");
                return true;
            }

            // מעבר לדף הכנת מתכון
            if (item.ItemId == Resource.Id.homepageMenu_createRecipe)
            {
                Intent intent = new Intent(this, typeof(CreateRecipeActivity));
                StartActivity(intent);
                return true;
            }

            // מעבר לדף המתכונים האהובים
            if (item.ItemId == Resource.Id.homepageMenu_favoriteRecipes)
            {
                Intent intent = new Intent(this, typeof(FavoriteRecipesActivity));
                StartActivity(intent);
                return true;
            }

            // מעבר לדף המתכונים שלי
            if (item.ItemId == Resource.Id.homepageMenu_myRecipes)
            {
                Intent intent = new Intent(this, typeof(MyRecipesActivity));
                StartActivity(intent);
                return true;
            }

            // להצגת הגדרות SettingsDialogFragment פתיחת 
            if (item.ItemId == Resource.Id.homepageMenu_settings)
            {
                //Pull up the settingsFragmentLayout (dialog)
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                // פעולה בונה שקולטת את המשתמש המחובר
                SettingsDialogFragment settingsDialogFragment = new SettingsDialogFragment(DatabaseManager.GetUser(SharedPreferencesManager.GetUsername()));
                settingsDialogFragment.Show(transaction, "Settings dialog fragment");
                return true;
            }

            // התנתקות מהאפליקציה
            if (item.ItemId == Resource.Id.homepageMenu_logout)
            {
                // בעזרת מחלקת עזר ISharedPreferences איפוס נתוני המשתמש השמורים באמצעות 
                SharedPreferencesManager.ResetPreferences();

                // ביטול שירות הודעות הסטטוס בעת התנתקות
                Intent serviceIntent = new Intent(this, typeof(StatusNotificationService));
                StopService(serviceIntent);

                // וסגירת דף הבית Join מעבר לדף 
                Intent intent = new Intent(this, typeof(JoinActivity));
                StartActivity(intent);
                Finish();
                return true;
            }

            // מופיע רק כאשר מנהל מחובר ומפנה לדף רשימת המשתמשים
            if (item.ItemId == Resource.Id.homepageMenu_userList)
            {
                Intent intent = new Intent(this, typeof(UserListActivity));
                StartActivity(intent);
                return true;
            }
            return false;
        }

        /* OnCreaete() פעולה שמתרחשת לאחר  
         כך שאם המשתמש יצר, עדכן או מחק מתכון OnRestart() הפעולה נקראת לאחר 
         כשהוא יחזור לדף הוא יראה את השינוי */
        protected override void OnStart()
        {
            // makes the activity visible to the user, as the app prepares for the activity to enter the foreground and become interactive
            // this method is where the app initializes the code that maintains the UI
            base.OnStart();

            actvSearch.Text = "";

            // DatabaseManager קבלת כל המתכונים ממחלקת עזר  
            recipeList = DatabaseManager.GetAllRecipes();

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

        // ולפני הצגת הדף למשתמש OnStart() פעולה המופעלת אחרי 
        protected override void OnResume()
        {
            // This is the state in which the app interacts with the user. The app stays in this state until something happens to take focus away from the app
            base.OnResume();

            // מאזין לו BroadCast -שממנו ה IntentFilter -תוך שליחת המופע וה batteryBroadcastReceiver רישום של האובייקט 
            RegisterReceiver(batteryBroadcastReceiver, new IntentFilter(Intent.ActionBatteryChanged));
        }

        // כאשר הדף נמצא ברקע או ישנה יציאה מהאפליקציה ללא סגירתה
        protected override void OnPause()
        {
            // כאשר האקטיביטי נכנס לרקע batteryBroadcastReceiver ביטול הרישום של 
            UnregisterReceiver(batteryBroadcastReceiver);

            // When an interruptive event occurs, the activity enters the Paused state
            base.OnPause();
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
            recipeList = DatabaseManager.GetAllRecipes();

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
                // קבלת כל הסלטים  
                recipeList = DatabaseManager.GetAllSalads();
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
                // קבלת כל המרקים  
                recipeList = DatabaseManager.GetAllSoups();
                // סינון הרשימה על פי נתוני המרקים שנבחרו
                recipeList = (from recipe in recipeList
                              orderby recipe.recipeName
                              where ((Soup)recipe).soupType == details[0]
                              && ((Soup)recipe).duration == details[1]
                              select recipe).ToList();
            }
            if (recipeType == "Meat")
            {
                // קבלת כל הבשרים  
                recipeList = DatabaseManager.GetAllMeats();
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
                // קבלת כל המאפים  
                recipeList = DatabaseManager.GetAllPastries();
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

        // לחיצה על כפתור התפריט
        private void BtnMenu_Click(object sender, EventArgs e)
        {
            // כאשר הכפתור נלחץ הפעולה פותחת את התפריט
            OpenContextMenu((Button)sender);
        }

        /* AdapterView.IOnItemClickListener מימוש הפעולה מתוקף מימוש הממשק
         נלחץ מופעלת הפעולה ListView- כאשר מתכון/איבר ב */
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            // נלחץ ושליחת קוד המתכון כמחרוזת ListView-מעבר לדף המתכון כאשר מתכון כלשהו ב
            Intent intent = new Intent(this, typeof(RecipePageActivity));
            intent.PutExtra("RecipeId", recipeList[position].recipeId);
            StartActivity(intent);
        }
    }
}