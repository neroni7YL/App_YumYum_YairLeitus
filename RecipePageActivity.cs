using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Refractored.Controls;

namespace App_YumYum_YairLeitus
{
    [Activity(Label = "RecipePageActivity")]
    public class RecipePageActivity : Activity
    {
        // משתמש המייצג את המשתמש המחובר למערכת
        private User loggedUser;

        // משתמש המייצג את המשתמש שיצר את המתכון
        private User recipeUser;

        // המתכון המוצג בדף
        private Recipe recipe;

        // של שם המתכון - כותרת הדף TextView פקד 
        private TextView tvRecipeName;

        // פקד תמונה של תמונת המתכון - ניתן לעדכן
        private ImageView ivRecipeImage;

        // של שם המשתמש שיצר את המתכון TextView פקד 
        private TextView tvRecipeUsername;

        // פקד תמונה עגולה של תמונת פרופיל המשתמש של המתכון
        private CircleImageView civUserImage;

        // של קטגוריות המתכון - ניתן לעדכן TextView פקד 
        private TextView tvCategory;

        // של דקות ההכנה - ניתן לעדכן EditText פקד 
        private EditText etHours;

        // של דקות ההכנה - ניתן לעדכן EditText פקד 
        private EditText etMinutes;

        // של סוג המתכון TextView פקד 
        private TextView tvRecipeType;

        // של כותרות סוג המתכון TextView מערך פקדי 
        private TextView[] tvTypeTitles;

        // של מידע סוג המתכון - ניתן לעדכן TextView מערך פקדי 
        private TextView[] tvTypeDetails;

        // של תיאור המתכון - ניתן לעדכן EditText פקד 
        private EditText etDescription;

        // של המרכיבים - ניתן לעדכן EditText פקד 
        private EditText etIngredients;

        // של הוראות ההכנה - ניתן לעדכן EditText פקד 
        private EditText etInstructions;

        // של כמות הלייקים TextView פקד 
        private TextView tvLikeCount;

        // פקד עם תמונת לב המסמל אם המשתמש המחובר אוהב את המתכון
        private ImageView ivLike;

        // כפתור חזרה לדף הבית
        private Button btnGoBack;

        // כפתור מצב עדכון מתכון
        private Button btnEdit;

        // כפתור מחיקת מתכון
        private Button btnDelete;

        // כפתור שמירה לאחר עדכון מתכון
        private Button btnSave;

        // מחרוזת עם המידע המתאים ברגע אי תקינות נתונים בעת עדכון מתכון
        private string errorText;

        /* Interface for converting text key events into edit operations on an Editable class
         כאשר הדף מוצג EditText-שימוש במערך של ממשק לביטול תכונת עריכת טקסט ב
         כאשר הדף נמצא במצב עדכון מתכון EditText והחזרת תכונות העריכה של */
        IKeyListener[] keyListeners;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.recipePage_layout);

            tvRecipeName = FindViewById<TextView>(Resource.Id.recipePage_tvRecipeName);
            ivRecipeImage = FindViewById<ImageView>(Resource.Id.recipePage_ivRecipeImage);
            tvRecipeUsername = FindViewById<TextView>(Resource.Id.recipePage_tvRecipeUsername);
            civUserImage = FindViewById<CircleImageView>(Resource.Id.recipePage_civUserImage);
            tvCategory = FindViewById<TextView>(Resource.Id.recipePage_tvCategory);
            etHours = FindViewById<EditText>(Resource.Id.recipePage_etHours);
            etMinutes = FindViewById<EditText>(Resource.Id.recipePage_etMinutes);
            tvRecipeType = FindViewById<TextView>(Resource.Id.recipePage_tvRecipeType);
            etDescription = FindViewById<EditText>(Resource.Id.recipePage_etDescription);
            etIngredients = FindViewById<EditText>(Resource.Id.recipePage_etIngredients);
            etInstructions = FindViewById<EditText>(Resource.Id.recipePage_etInstructions);
            tvLikeCount = FindViewById<TextView>(Resource.Id.recipePage_tvLikeCount);
            ivLike = FindViewById<ImageView>(Resource.Id.recipePage_ivLike);
            btnEdit = FindViewById<Button>(Resource.Id.recipePage_btnEdit);
            btnDelete = FindViewById<Button>(Resource.Id.recipePage_btnDelete);
            btnSave = FindViewById<Button>(Resource.Id.recipePage_btnSave);
            btnGoBack = FindViewById<Button>(Resource.Id.recipePage_btnGoBack);

            // לכל סוג מתכון (סלט, מרק, מנת בשר ,מאפה) ארבעה כותרות
            tvTypeTitles = new TextView[4];
            tvTypeTitles[0] = FindViewById<TextView>(Resource.Id.recipePage_tvTypeTitle1);
            tvTypeTitles[1] = FindViewById<TextView>(Resource.Id.recipePage_tvTypeTitle2);
            tvTypeTitles[2] = FindViewById<TextView>(Resource.Id.recipePage_tvTypeTitle3);
            tvTypeTitles[3] = FindViewById<TextView>(Resource.Id.recipePage_tvTypeTitle4);

            // לכל סוג מתכון ארבעה שדות מידע המייצגים את נתוני תכונותיהם
            tvTypeDetails = new TextView[4];
            tvTypeDetails[0] = FindViewById<TextView>(Resource.Id.recipePage_tvTypeDetail1);
            tvTypeDetails[1] = FindViewById<TextView>(Resource.Id.recipePage_tvTypeDetail2);
            tvTypeDetails[2] = FindViewById<TextView>(Resource.Id.recipePage_tvTypeDetail3);
            tvTypeDetails[3] = FindViewById<TextView>(Resource.Id.recipePage_tvTypeDetail4);

            // קבלת המשתמש המחובר בעזרת שתי מחלקות עזר
            // אחת מקבלת את שם המשתמש בקובץ השמור בזכרון והשנייה מזהה משתמש מהמסד לפי שם המשתמש
            loggedUser = DatabaseManager.GetUser(SharedPreferencesManager.GetUsername());

            // קבלת קוד המתכון מדף האקטיביטי הקודם וזיהוי המתכון בעזרת מחלקת עזר
            recipe = DatabaseManager.GetRecipe(Intent.GetStringExtra("RecipeId"));
            // קבלת משתמש המתכון לפי תכונת המתכון
            recipeUser = DatabaseManager.GetUser(recipe.recipeUsername);

            // הצבת פרטי המידע השונים בשדות המתאימים לפי תכונות המתכון
            tvRecipeName.Text = recipe.recipeName; // שם המתכון
            tvRecipeUsername.Text = recipe.recipeUsername; // שם משתמש המתכון
            ivRecipeImage.SetImageBitmap(ImageManager.Base64ToBitmap(recipe.recipeImage)); // תמונת המתכון

            //בדיקה שאכן למשתמש יש תמונת פרופיל קיימת
            if (recipeUser.userImage != "")
            {
                // השמת תמונת פרופיל המשתמש של המתכון
                civUserImage.SetImageBitmap(ImageManager.Base64ToBitmap(recipeUser.userImage));
            }
            tvCategory.Text = recipe.category; // קטגוריות
            etHours.Text = recipe.hours.ToString(); // שעות הכנה
            etMinutes.Text = recipe.minutes.ToString(); // דקות הכנה
            etDescription.Text = recipe.description; // תיאור מתכון
            etIngredients.Text = recipe.ingredients; // מרכיבים
            etInstructions.Text = recipe.instructions; // הוראות הכנה

            // בדיקה איזה סוג מתכון המתכון הנ"ל והצבת כותרות ומידע מתאים
            if (recipe is Salad)
            {
                tvRecipeType.Text = "Salad";
                tvTypeTitles[0].Text = "Vegetables/Fruit";
                tvTypeTitles[1].Text = "Salad Type";
                tvTypeTitles[2].Text = "Contains Meat";
                tvTypeTitles[3].Text = "Contains Flavoring";
                tvTypeDetails[0].Text = ((Salad)recipe).isFruit;
                tvTypeDetails[1].Text = ((Salad)recipe).saladType;
                tvTypeDetails[2].Text = ((Salad)recipe).isMeat;
                tvTypeDetails[3].Text = ((Salad)recipe).isFlavoring;
            }
            if (recipe is Soup)
            {
                tvRecipeType.Text = "Soup";
                tvTypeTitles[0].Text = "Soup Type";
                tvTypeTitles[1].Text = "Cooking Duration";
                tvTypeTitles[2].Text = "Boiling Time";
                tvTypeTitles[3].Text = "Temperature";
                tvTypeDetails[0].Text = ((Soup)recipe).soupType;
                tvTypeDetails[1].Text = ((Soup)recipe).duration;
                tvTypeDetails[2].Text = ((Soup)recipe).boilingTime.ToString();
                tvTypeDetails[3].Text = ((Soup)recipe).boilingTemperature.ToString();
            }
            if (recipe is Meat)
            {
                tvRecipeType.Text = "Meat";
                tvTypeTitles[0].Text = "Meat Type";
                tvTypeTitles[1].Text = "Cooking Method";
                tvTypeTitles[2].Text = "Meat Doneness";
                tvTypeTitles[3].Text = "Cooking Time";
                tvTypeDetails[0].Text = ((Meat)recipe).meatType;
                tvTypeDetails[1].Text = ((Meat)recipe).method;
                tvTypeDetails[2].Text = ((Meat)recipe).doneness;
                tvTypeDetails[3].Text = ((Meat)recipe).cookingTime.ToString();
            }
            if (recipe is Pastry)
            {
                tvRecipeType.Text = "Pastry";
                tvTypeTitles[0].Text = "Contains Gluten";
                tvTypeTitles[1].Text = "Kosher";
                tvTypeTitles[2].Text = "Baking Time";
                tvTypeTitles[3].Text = "Temperature";
                tvTypeDetails[0].Text = ((Pastry)recipe).isGluten;
                tvTypeDetails[1].Text = ((Pastry)recipe).kosher;
                tvTypeDetails[2].Text = ((Pastry)recipe).bakingTime.ToString();
                tvTypeDetails[3].Text = ((Pastry)recipe).bakingTemperature.ToString();
            }

            // אם המשתמש אוהב את המתכון צבע הלב יהיה אדום אחרת יהיה לבן
            if (loggedUser.favoriteRecipesId.Contains(recipe.recipeId))
            {
                ivLike.SetColorFilter(Color.ParseColor("#ffd5284a")); // צבע אדום
            }
            else
            {
                ivLike.SetColorFilter(Color.ParseColor("#ffeeeeee")); // צבע לבן

            }
            // כתיבת טקסט מתאים עבור מספר הלייקים
            if (recipe.likeCount == 1)
            {
                // הודעה עבור לייק אחד
                tvLikeCount.Text = recipe.likeCount.ToString() + " person liked this recipe";
            }
            else
            {
                // הודעה על מספר לייקים שונה
                tvLikeCount.Text = recipe.likeCount.ToString() + " people liked this recipe";
            }

            // אם המשתמש המחובר הוא יוצר המתכון או מנהל האפליקציה אזי תהיה לו גישה לכפתורי עדכון ומחיקת המתכון
            if (recipeUser.username == loggedUser.username || loggedUser.isAdmin)
            {
                btnEdit.Visibility = ViewStates.Visible;
                btnDelete.Visibility = ViewStates.Visible;
            }

            ivLike.Click += IvLike_Click;
            ivRecipeImage.Click += IvRecipeImage_Click;
            tvCategory.Click += TvCategory_Click;
            tvRecipeType.Click += TvRecipeType_Click;
            btnEdit.Click += BtnEdit_Click;
            btnSave.Click += BtnSave_Click;
            btnDelete.Click += BtnDelete_Click;
            btnGoBack.Click += delegate { Finish(); };

            // EditText השומר את תכונות העריכה של keyListeners מערך 
            keyListeners = new IKeyListener[5];
            keyListeners[0] = etHours.KeyListener;
            keyListeners[1] = etMinutes.KeyListener;
            keyListeners[2] = etDescription.KeyListener;
            keyListeners[3] = etIngredients.KeyListener;
            keyListeners[4] = etInstructions.KeyListener;

            // EditText ביטול תכונות העריכה של שדות 
            etHours.KeyListener = null;
            etMinutes.KeyListener = null;
            etDescription.KeyListener = null;
            etIngredients.KeyListener = null;
            etInstructions.KeyListener = null;

            // פקדים המשמשים בעדכון כפקד לחיצה, במצב רגיל לא ניתן ללחוץ עליהם
            ivRecipeImage.Clickable = false;
            tvCategory.Clickable = false;
            tvRecipeType.Clickable = false;
        }

        // פעולה שנקראת כאשר לוחצים על תמונת אייקון הלב
        private void IvLike_Click(object sender, EventArgs e)
        {
            // מערך מחרוזות המכיל את תכני ההודעה שתשלח בהתאמה
            string[] recipePageDetails;
            
            // בדיקה האם המשתמש המחובר אוהב את המתכון
            if (loggedUser.favoriteRecipesId.Contains(recipe.recipeId))
            {
                // בלחיצה על הלב מספר הלייקים של המתכון יורד באחד
                recipe.likeCount--;
                // קוד המתכון מוסר ממחרוזת קודי המתכונים של המשתמש המחובר
                loggedUser.favoriteRecipesId = loggedUser.favoriteRecipesId.Replace(recipe.recipeId + ",", "");
                // צבע הלב משתנה ללבן
                ivLike.SetColorFilter(Color.ParseColor("#ffeeeeee"));
                // מחרוזת המציינת את כותרת ותוכן ההודעה בהתאמה
                recipePageDetails = new string[] { "A Recipe Was Deleted From Favorite Recipes", recipe.recipeName + " was successfully deleted" };
            }
            else
            {
                // בלחיצה על הלב מספר הלייקים של המתכון עולה באחד
                recipe.likeCount++;
                // קוד המתכון מתווסף למחרוזת קודי המתכונים של המשתמש המחובר
                loggedUser.favoriteRecipesId += recipe.recipeId + ",";
                // צבע הלב משתנה לאדום
                ivLike.SetColorFilter(Color.ParseColor("#ffd5284a"));
                // מחרוזת המציינת את כותרת ותוכן ההודעה בהתאמה
                recipePageDetails = new string[] { "A New Favorite Recipe Was Added", recipe.recipeName + " was entered successfully" };
            }

            // בדיקה שהמשתמש מרשה שירות הודעות
            if (loggedUser.allowNotification)
            {
                // הפעלת שירות הודעות לאחר לחיצה על הלב בהתאמה
                StartNotificationService(recipePageDetails);
            }

            // עדכון המתכון והמשתמש שאהב אותו במסד הנתונים
            DatabaseManager.SetRecipe(recipe);
            DatabaseManager.SetUser(loggedUser);

            // עדכון טקסט כמות הלייקים של המתכון
            if (recipe.likeCount == 1)
            {
                tvLikeCount.Text = recipe.likeCount.ToString() + " person liked this recipe";
            }
            else
            {
                tvLikeCount.Text = recipe.likeCount.ToString() + " people liked this recipe";
            }
        }

        // פעולה הנקראת כאשר לוחצים על כפתור העדכון
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            // יצירת דיאלוג לאישור תחילת מצב העדכון
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle("Edit Recipe");
            alertDialog.SetMessage("Are you sure you want to edit: " + recipe.recipeName + "?");
            alertDialog.SetCancelable(true);
            // אישור התחלת עדכון מתכון
            alertDialog.SetPositiveButton("EDIT", delegate
            {
                // עיצוב הפקדים השונים כך שניתן לדעת שמדובר במצב עדכון מתכון
                ivRecipeImage.SetBackgroundColor(Color.ParseColor("#ffff6064"));
                tvCategory.SetBackgroundResource(Resource.Drawable.buttonStyle);
                tvRecipeType.SetBackgroundResource(Resource.Drawable.buttonStyle);
                etHours.SetBackgroundResource(Resource.Drawable.editTextStyle);
                etMinutes.SetBackgroundResource(Resource.Drawable.editTextStyle);
                etDescription.SetBackgroundResource(Resource.Drawable.editTextStyle);
                etIngredients.SetBackgroundResource(Resource.Drawable.editTextStyle);
                etInstructions.SetBackgroundResource(Resource.Drawable.editTextStyle);

                // מתן אפשרות ללחיצה על הפקדים השונים
                // לחיצה עליהם תפתח הודעה מתאימה
                ivRecipeImage.Clickable = true;
                tvCategory.Clickable = true;
                tvRecipeType.Clickable = true;

                // EditTextהחזרת תכונות עריכה לפקדי ה
                etHours.KeyListener = keyListeners[0];
                etMinutes.KeyListener = keyListeners[1];
                etDescription.KeyListener = keyListeners[2];
                etIngredients.KeyListener = keyListeners[3];
                etInstructions.KeyListener = keyListeners[4];

                // הסתרת כפתורי עדכון ומחיקת מתכון והצגת כפתור לשמירת העדכון
                btnEdit.Visibility = ViewStates.Gone;
                btnDelete.Visibility = ViewStates.Gone;
                btnSave.Visibility = ViewStates.Visible;
               
            });
            alertDialog.SetNeutralButton("CANCEL", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }

        // פעולה שנקראת לאחר לחיצה על תמונת המתכון בעת עדכון
        private void IvRecipeImage_Click(object sender, EventArgs e)
        {
            // שבו ניתן לבחור את האמצעי להעלת תמונת המתכון, מצלמה או גלריית התמונות AlertDialog בניית 
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle("Choose a recipe image");
            alertDialog.SetMessage("Choose between Gallery and Camera");
            alertDialog.SetCancelable(true);
            alertDialog.SetPositiveButton("GALLERY", delegate
            {
                // Photos-ו Gallery שמפנה לגלריית התמונות, ישנה בחירה בין  Intent
                Intent galleryIntent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);

                // הפעלת הפעולה הנ"ל אשר שולחת קוד 0 אשר מציין שהחזרה לאפליקציה היא מהגלרייה
                StartActivityForResult(Intent.CreateChooser(galleryIntent, "Select an Image"), 0);
            });
            alertDialog.SetNegativeButton("CAMERA", delegate
            {
                // שמפנה למצלמת הטלפון Intent
                Intent cameraIntent = new Intent(MediaStore.ActionImageCapture);

                // הפעלת הפעולה הנ"ל אשר שולחת קוד 1 אשר מציין שהחזרה לאפליקציה היא מהמצלמה
                StartActivityForResult(cameraIntent, 1);
            });
            alertDialog.SetNeutralButton("CANCEL", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }
        // פעולה שמתבצעת בעת החזרה לאפליקציה מהגלרייה או מהמצלמה
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // אשר בתוכו נשמר תוכן התמונה שנבחרה Bitmap עצם מטיפוס
            Bitmap imageBitmap = null;
            // בדיקה האם החזרה בוצעה בהצלחה
            if (resultCode == Result.Ok)
            {
                // Choose from gallery
                if (requestCode == 0)
                {
                    // שומר את תוכן התמונה מהגלרייה
                    imageBitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, data.Data);
                }
                // Take a photo
                else if (requestCode == 1)
                {
                    // Get the image bitmap from the intent extras
                    // שומר את תוכן התמונה מהמצלמה
                    imageBitmap = (Bitmap)data.Extras.Get("data");
                }

                // החלפת תמונת המתכון לתמונה החדשה
                ivRecipeImage.SetImageBitmap(imageBitmap);
            }
        }

        // נלחץ בזמן עדכון tvCategory כאשר פקד CategoryDialogFragment פעולה הפותחת את 
        private void TvCategory_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            CategoryDialogFragment categoryDialogFragment = new CategoryDialogFragment();
            categoryDialogFragment.Show(transaction, "Category dialog fragment");
        }
        /* לאחר סיום בחירת הקטגוריות CategoryDialogFragment-פעולת עזר שמופעלת ב
         tvCategory הפעולה מקבלת מחרוזת המכילה את הקטגוריות המופרדות בפסיקים ומציגה את המחרוזת בפקד */
        public void ReceiveCategories(string categories)
        {
            tvCategory.Text = categories;
        }

        // נלחץ בזמן עדכון tvRecipeType הפעולה נקראת כאשר פקד 
        private void TvRecipeType_Click(object sender, EventArgs e)
        {
            // בדיקה באיזה סוג מתכון מדובר
            if (recipe is Salad)
            {
                // אם מדובר בסלט פתיחת טופס שכולל את המידע על הסלט
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                SaladDialogFragment saladDialogFragment = new SaladDialogFragment();
                saladDialogFragment.Show(transaction, "Salad dialog fragment");
            }
            if (recipe is Soup)
            {
                // אם מדובר במרק פתיחת טופס שכולל את המידע על המרק
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                SoupDialogFragment soupDialogFragment = new SoupDialogFragment();
                soupDialogFragment.Show(transaction, "Soup dialog fragment");

            }
            if (recipe is Meat)
            {
                // אם מדובר במנת בשר פתיחת טופס שכולל את המידע על הבשר
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                MeatDialogFragment meatDialogFragment = new MeatDialogFragment();
                meatDialogFragment.Show(transaction, "Meat dialog fragment");
            }
            if (recipe is Pastry)
            {
                // אם מדובר במאפה פתיחת טופס שכולל את המידע על המאפה
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                PastryDialogFragment pastryDialogFragment = new PastryDialogFragment();
                pastryDialogFragment.Show(transaction, "Pastry dialog fragment");
            }
        }
        /* PastryDialogFragment או MeatDialogFragment, SoupDialogFragment ,SaladDialogFragment-פעולת עזר שמופעלת ב
         לאחר סיום בחירת פרטי המידע על סוג המתכון, הפעולה מקבלת מחרוזת המכילה את הפרטים ומציגה אותם בדף המתכון */
        public void ReceiveRecipeTypeDetails(string[] details)
        {
            tvTypeDetails[0].Text = details[0];
            tvTypeDetails[1].Text = details[1];
            tvTypeDetails[2].Text = details[2];
            tvTypeDetails[3].Text = details[3];
        }

        // פעולה הנקראת כאשר לוחצים על כפתור השמירה לאחר העדכון
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // יצירת דיאלוג לאישור העדכון
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle("Save Recipe");
            alertDialog.SetMessage("Are you sure you want to save: " + recipe.recipeName + "?");
            alertDialog.SetCancelable(true);
            // אישור עדכון מתכון
            alertDialog.SetPositiveButton("SAVE", delegate
            {
                // בדיקה האם הנתונים המעודכנים תקינים
                if (IsValid())
                {
                    // עדכון אובייקט המתכון הנ"ל לפי השדות המעודכנים
                    Bitmap imageBitmap = ((BitmapDrawable)ivRecipeImage.Drawable).Bitmap;
                    recipe.recipeImage = ImageManager.BitmapToBase64(imageBitmap);
                    recipe.category = tvCategory.Text;
                    recipe.hours = int.Parse(etHours.Text);
                    recipe.minutes = int.Parse(etMinutes.Text);
                    recipe.description = etDescription.Text;
                    recipe.ingredients = etIngredients.Text;
                    recipe.instructions = etInstructions.Text;
                    // בדיקה איזה סוג מתכון, ועדכון פרטים לפי סוג המתכון בהתאמה
                    if (recipe is Salad)
                    {
                        ((Salad)recipe).isFruit = tvTypeDetails[0].Text;
                        ((Salad)recipe).saladType = tvTypeDetails[1].Text;
                        ((Salad)recipe).isMeat = tvTypeDetails[2].Text;
                        ((Salad)recipe).isFlavoring = tvTypeDetails[3].Text;
                    }
                    if (recipe is Soup)
                    {
                        ((Soup)recipe).soupType = tvTypeDetails[0].Text;
                        ((Soup)recipe).duration = tvTypeDetails[1].Text;
                        ((Soup)recipe).boilingTime = int.Parse(tvTypeDetails[2].Text);
                        ((Soup)recipe).boilingTemperature = int.Parse(tvTypeDetails[3].Text);
                    }
                    if (recipe is Meat)
                    {
                        ((Meat)recipe).meatType = tvTypeDetails[0].Text;
                        ((Meat)recipe).method = tvTypeDetails[1].Text;
                        ((Meat)recipe).doneness = tvTypeDetails[2].Text;
                        ((Meat)recipe).cookingTime = int.Parse(tvTypeDetails[3].Text);
                    }
                    if (recipe is Pastry)
                    {
                        ((Pastry)recipe).isGluten = tvTypeDetails[0].Text;
                        ((Pastry)recipe).kosher = tvTypeDetails[1].Text;
                        ((Pastry)recipe).bakingTime = int.Parse(tvTypeDetails[2].Text);
                        ((Pastry)recipe).bakingTemperature = int.Parse(tvTypeDetails[3].Text);
                    }

                    // עדכון המתכון במסד הנתונים
                    DatabaseManager.SetRecipe(recipe);

                    // StatusNotificationService
                    // בדיקה שיוצר המתכון מאשר את שירות ההודעות וגם המשתמש המחובר הוא גם יוצר המתכון - משתמש רגיל מחובר
                    // או האם המשתמש המחובר מאשר הודעות - כלומר מדובר במנהל שמעדכן מתכון של משתמש
                    if ((recipeUser.allowNotification && recipeUser.username == loggedUser.username) || loggedUser.allowNotification)
                    {
                        // מחרוזת המציינת את כותרת ותוכן ההודעה לאחר עדכון מתכון
                        string[] recipePageDetails = { "A Recipe Was Edited", recipe.recipeName + " was successfully edited" };
                        // הפעלת שירות הודעות לאחר עדכון מתכון
                        StartNotificationService(recipePageDetails);
                    }

                    // טעינה מחדש של הדף עם אנימציה רגילה של פתיחת דף, למניעת מסך שחור
                    // ופותחת את הדף מחדש מלמטה StartActivity מופעלת לאחר 
                    OverridePendingTransition(0, 0);
                    // סגירת הדף ופתיחתו מחדש
                    Finish();
                    StartActivity(this.Intent);
                }
                else
                {
                    // במידה ולא, תופיע הודעת שגיאה מתאימה
                    ShowAlertDialog();
                }
            });
            alertDialog.SetNeutralButton("CANCEL", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }

        // פעולה כללית הבודקת האם הפרטים המעודכנים תקינים
        private bool IsValid()
        {
            // Hours Validation
            if (etHours.Text == "")
            {
                errorText = "Insert hours";
                return false;
            }

            // Minutes Validation
            if (etMinutes.Text == "")
            {
                errorText = "Insert minutes";
                return false;
            }

            // בדיקה שהדקות בין 0 ל59
            if (int.Parse(etMinutes.Text) > 59)
            {
                errorText = "Minutes' range must be between 0 - 59 minutes";
                return false;
            }

            // Description Validation
            if (etDescription.Text == "")
            {
                errorText = "Insert Description";
                return false;
            }

            // Ingredients Validation
            if (etIngredients.Text == "")
            {
                errorText = "Insert Ingredients";
                return false;
            }

            // Instructions Validation
            if (etInstructions.Text == "")
            {
                errorText = "Insert Instructions";
                return false;
            }
            return true;
        }
        // errorText המודיע על שגיאה לפי המשתנה AlertDialog פעולה היוצרת
        private void ShowAlertDialog()
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle("Error");
            alertDialog.SetMessage(errorText);
            alertDialog.SetCancelable(true);
            alertDialog.SetNeutralButton("CANCEL", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }

        // פעולה הנקראת כאשר לוחצים על כפתור מחיקת המתכון
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // יצירת דיאלוג לאישור המחיקה
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle("Delete Recipe");
            alertDialog.SetMessage("Are you sure you want to delete: " + recipe.recipeName + "?");
            alertDialog.SetCancelable(true);
            // מחיקת מתכון
            alertDialog.SetPositiveButton("DELETE", delegate
            {
                // מחיקת קוד המתכון במחרוזת קודי המתכונים של המשתמש שמתכונו נמחק ועדכון המשתמש
                recipeUser.myRecipesId = recipeUser.myRecipesId.Replace(recipe.recipeId + ",", "");
                DatabaseManager.SetUser(recipeUser);

                //מעבר על כל המשתמשים הרשומים ומחיקת קוד המתכון ברשימת קודי המתכונים האהובים שלהם במידה ואהבו את המתכון
                // רשימה המכילה את כל המשתמשים
                List<User> userList = DatabaseManager.GetAllUsers();
                // לולאה העוברת על כל המשתמשים
                for (int i = 0; i < userList.Count; i++)
                {
                    // בדיקה האם המשתמשים אוהבים את המתכון הנ"ל
                    if (userList[i].favoriteRecipesId.Contains(recipe.recipeId))
                    {
                        // אם כן מחיקת קוד המתכון הנ"ל במחרוזת המתכונים האהובים של כל משתמש ועדכון המשתמש במסד הנתונים
                        userList[i].favoriteRecipesId = userList[i].favoriteRecipesId.Replace(recipe.recipeId + ",", "");
                        DatabaseManager.SetUser(userList[i]);
                    }
                }

                // מחיקת המתכון הנ"ל ממסד הנתונים
                DatabaseManager.DeleteRecipe(recipe);

                // StatusNotificationService
                // בדיקה שיוצר המתכון מאשר את שירות ההודעות וגם המשתמש המחובר הוא גם יוצר המתכון - משתמש רגיל מחובר
                // או האם המשתמש המחובר מאשר הודעות - כלומר מדובר במנהל שמעדכן מתכון של משתמש
                if ((recipeUser.allowNotification && recipeUser.username == loggedUser.username) || loggedUser.allowNotification)
                {
                    // מחרוזת המציינת את כותרת ותוכן ההודעה לאחר מחיקת מתכון
                    string[] recipePageDetails = { "A Recipe Was Deleted", recipe.recipeName + " was successfully deleted" };
                    // הפעלת שירות הודעות לאחר מחיקת מתכון
                    StartNotificationService(recipePageDetails);
                }
                
                // במידה והמוחק הוא מנהל יש לו אפשרות לשלוח אימייל למשתמש שמתכונו נמחק
                // בדיקה שהמשתמש המוחק הוא מנהל ושהוא לא ישלח אימייל לעצמו
                if (loggedUser.isAdmin && recipeUser.username != loggedUser.username)
                {
                    // במצב של מחיקת מתכון של משתמש המנהל רשאי לשלוח לו אימייל על כך
                    SendEmail();
                }
                Finish();
            });
            alertDialog.SetNeutralButton("CANCEL", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }

        // פעולה המתחילה את שירות ההודעות ושולחת תוכן הודעה מתאים
        // הפעולה מקבלת מערך מחרוזות המציין את כותרת ותוכן ההודעה
        private void StartNotificationService(string[] recipePageDetails)
        {
            // הפעלת שירות ההודעות ושליחת הודעה מתאימה להרשמה
            Intent serviceIntent = new Intent(this, typeof(StatusNotificationService));
            // שליחת מערך מחרוזות שמייצג את כותרת ותוכן ההודעה
            serviceIntent.PutExtra("StatusNotification_Details", recipePageDetails);
            StartService(serviceIntent);
        }

        // פעולה הפותחת דף לשליחת אימייל לאחר מחיקת מתכון בידי מנהל
        private void SendEmail()
        {
            string[] email = { DatabaseManager.GetUser(tvRecipeUsername.Text).email }; // אימייל משתמש המתכון
            Intent sendEmailIntent = new Intent(Intent.ActionSend);
            sendEmailIntent.SetType("message/rfc822"); // Opening email clients
            sendEmailIntent.PutExtra(Intent.ExtraEmail, email); // נמען
            sendEmailIntent.PutExtra(Intent.ExtraSubject, string.Format("The recipe: {0} was deleted by an admin.", tvRecipeName.Text)); // כותרת  
            sendEmailIntent.PutExtra(Intent.ExtraText, string.Format("Content")); // תוכן
            // הצגת דף שליחת אימייל עם התכנים הרלוונטיים
            StartActivity(sendEmailIntent);
        }
    }
}