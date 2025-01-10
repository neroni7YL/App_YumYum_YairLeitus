using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AlertDialog = Android.App.AlertDialog;
using Android.Provider;

namespace App_YumYum_YairLeitus
{
    [Activity(Label = "CreateRecipeActivity")]
    public class CreateRecipeActivity : Activity
    {
        // של תמונת המתכון ImageView פקד
        private ImageView ivImage;

        // משתנה שמציין האם הוכנסה תמונת מתכון
        private bool validImage;

        // של שם המתכון EditText פקד
        private EditText etRecipeName;

        // של מספר שעות הכנת המתכון EditText פקד
        private EditText etHours;

        // של מספר דקות הכנת המתכון EditText פקד
        private EditText etMinutes;

        // של סוג המתכון TextView פקד
        private TextView tvRecipeType;

        // של קטגוריית המתכון TextView פקד
        private TextView tvCategory;

        // של תיאור המתכון EditText פקד
        private EditText etDescription;

        // של מרכיבי המתכון EditText פקד
        private EditText etIngredients;

        // של הוראות הכנת המתכון EditText פקד
        private EditText etInstructions;

        // כפתור סיום ושמירת המתכון במערכת Button פקד
        private Button btnCreateRecipe;

        // כפתור חזרה לדף הבית Button פקד
        private Button btnGoBack;

        // משתנה השומר את ההודעה המתאימה במקרה של שגיאה
        private string errorText;

        // שכולל בתוכו את הכותרות והמידע של סוג המתכון LinearLayout
        private LinearLayout layoutRecipeType;

        // של כותרות המידע של סוג המתכון TextView מערך
        private TextView[] tvTypeTitles;

        // של המידע של סוג המתכון TextView מערך
        private TextView[] tvTypeDetails;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.createRecipe_layout);

            // מאחר ולא הוכנה תמונת מתכון false תחילה המשתנה יהיה
            validImage = false;

            ivImage = FindViewById<ImageView>(Resource.Id.createRecipe_ivAddRecipeImage);
            etRecipeName = FindViewById<EditText>(Resource.Id.createRecipe_etRecipeName);
            tvCategory = FindViewById<TextView>(Resource.Id.createRecipe_tvCategory);
            etHours = FindViewById<EditText>(Resource.Id.createRecipe_etHours);
            etMinutes = FindViewById<EditText>(Resource.Id.createRecipe_etMinutes);
            tvRecipeType = FindViewById<TextView>(Resource.Id.createRecipe_tvRecipeType);
            etDescription = FindViewById<EditText>(Resource.Id.createRecipe_etDescription);
            etIngredients = FindViewById<EditText>(Resource.Id.createRecipe_etIngredients);
            etInstructions = FindViewById<EditText>(Resource.Id.createRecipe_etInstructions);
            btnCreateRecipe = FindViewById<Button>(Resource.Id.createRecipe_btnCreateRecipe);
            btnGoBack = FindViewById<Button>(Resource.Id.createRecipe_btnGoBack);

            layoutRecipeType = FindViewById<LinearLayout>(Resource.Id.createRecipe_layoutRecipeType);

            // לכל סוג מתכון יש 4 כותרות ו4 מקומות למידע הרלוונטי לסוג המתכון
            tvTypeTitles = new TextView[4];
            tvTypeTitles[0] = FindViewById<TextView>(Resource.Id.createRecipe_tvTypeTitle1);
            tvTypeTitles[1] = FindViewById<TextView>(Resource.Id.createRecipe_tvTypeTitle2);
            tvTypeTitles[2] = FindViewById<TextView>(Resource.Id.createRecipe_tvTypeTitle3);
            tvTypeTitles[3] = FindViewById<TextView>(Resource.Id.createRecipe_tvTypeTitle4);

            tvTypeDetails = new TextView[4];
            tvTypeDetails[0] = FindViewById<TextView>(Resource.Id.createRecipe_tvTypeDetail1);
            tvTypeDetails[1] = FindViewById<TextView>(Resource.Id.createRecipe_tvTypeDetail2);
            tvTypeDetails[2] = FindViewById<TextView>(Resource.Id.createRecipe_tvTypeDetail3);
            tvTypeDetails[3] = FindViewById<TextView>(Resource.Id.createRecipe_tvTypeDetail4);

            ivImage.Click += IvImage_Click;
            tvCategory.Click += TvCategory_Click;
            tvRecipeType.Click += TvRecipeType_Click;
            btnCreateRecipe.Click += BtnCreateRecipe_Click;
            btnGoBack.Click += delegate { Finish(); };
        }

        // פעולה שמתרחשת בעת לחיצה על תמונת המתכון ובאפשרותה לבחור תמונה למתכון
        private void IvImage_Click(object sender, EventArgs e)
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

                // Bitmap שינוי תמונת המתכון באמצעות 
                ivImage.SetImageBitmap(imageBitmap);

                // true כאשר המשתמש שם תמונה משלו המשתנה יהיה
                validImage = true;
            }
        }

        // נלחץ tvCategory כאשר פקד CategoryDialogFragment פעולה הפותחת את 
        private void TvCategory_Click(object sender, EventArgs e)
        {
            // Pull up the categoryFragmentLayout (dialog)
            // בכדי לאפשר את פתיחתו FragmentTransaction ואובייקט מטיפוס CategoryDialogFragment יצירת אובייקט מטיפוס 
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

        // נלחץ tvRecipeType כאשר פקד RecipeTypeDialogFragment פעולה הפותחת את 
        private void TvRecipeType_Click(object sender, EventArgs e)
        {
            // Pull up the recipeTypeDialogFragment (dialog)
            // בכדי לאפשר את פתיחתו FragmentTransaction ואובייקט מטיפוס RecipeTypeDialogFragment יצירת אובייקט מטיפוס 
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            RecipeTypeDialogFragment recipeTypeDialogFragment = new RecipeTypeDialogFragment();
            recipeTypeDialogFragment.Show(transaction, "Recipe Type dialog fragment");
        }
        /* PastryDialogFragment או MeatDialogFragment, SoupDialogFragment ,SaladDialogFragment-פעולת עזר שמופעלת ב
         לאחר סיום בחירת פרטי סוג המתכון, הפעולה מקבלת מחרוזת המכילה את סוג המתכון 
         מערך מחרוזות המכיל את כותרות המידע ומערך מחרוזות המכיל פרטי המידע של סוג המתכון, */
        public void ReceiveRecipeTypeDetails(string recipeType, string[] titles, string[] details)
        {
            // הצבת סוג המתכון בפקד
            tvRecipeType.Text = recipeType;

            //פרטי סוג המתכון לאחר בחירת הפרטים מופיעים
            layoutRecipeType.Visibility = ViewStates.Visible;

            //הצבת הכותרות והפרטים
            tvTypeTitles[0].Text = titles[0];
            tvTypeTitles[1].Text = titles[1];
            tvTypeTitles[2].Text = titles[2];
            tvTypeTitles[3].Text = titles[3];

            tvTypeDetails[0].Text = details[0];
            tvTypeDetails[1].Text = details[1];
            tvTypeDetails[2].Text = details[2];
            tvTypeDetails[3].Text = details[3];
        }

        // פעולה כללית הבודקת האם הפרטים שיש למלא תקינים
        private bool IsValid()
        {
            if (!IsValidRecipeImage())
            {
                return false;
            }

            if (!IsValidRecipeName())
            {
                return false;
            }

            if (!IsValidCategory())
            {
                return false;
            }

            if (!IsValidHours())
            {
                return false;
            }

            if (!IsValidMinutes())
            {
                return false;
            }

            if (!IsValidRecipeType())
            {
                return false;
            }

            if (!IsValidDescription())
            {
                return false;
            }

            if (!IsValidIngredients())
            {
                return false;
            }

            if (!IsValidInstructions())
            {
                return false;
            }
            return true;
        }

        // לפי השגיאה errorText בדיקות התקינות הבאות מעדכנות את המשתנה 
        // validImage בדיקת תקינות של תמונת המתכון שנבדק לפי המשתנה
        private bool IsValidRecipeImage()
        {
            // Recipe Image Validation
            if (!validImage)
            {
                errorText = "Choose recipe image";
                return false;
            }
            return true;
        }
        // בדיקת תקינות של שם המתכון
        private bool IsValidRecipeName()
        {
            // Recipe Name Validation
            // בדיקה שהשם לא ריק
            if (etRecipeName.Text == "")
            {
                errorText = "Insert recipe name";
                return false;
            }

            // IsMatch בעלת פעולה Regex בדיקת תקינות באמצעות מחלקה סטטית
            // בדיקת תקינות שהמחרוזת של שם המתכון מתחילתו ועד סופו מכיל רק אותיות באנגלית
            if (!Regex.IsMatch(etRecipeName.Text, "^[a-zA-Z ]+$"))
            {
                errorText = "Recipe name must contain English letters only";
                return false;
            }
            return true;
        }
        // בדיקת תקינות הקטגוריות
        private bool IsValidCategory()
        {
            // Category Validation
            // בדיקה שנבחרו קטגוריות למתכון
            if (tvCategory.Text == "Category(ies)")
            {
                errorText = "Must choose at least one category";
                return false;
            }
            return true;
        }
        // בדיקת תקינות שעות ההכנה
        private bool IsValidHours()
        {
            //Hours Validation
            if (etHours.Text == "")
            {
                errorText = "Insert hours";
                return false;
            }
            return true;
        }
        // בדיקת תקינות דקות ההכנה
        private bool IsValidMinutes()
        {
            //Minutes Validation
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
            return true;
        }
        // בדיקת תקינות שנבחר סוג מתכון
        private bool IsValidRecipeType()
        {
            //Recipe Type Validation
            if (tvRecipeType.Text == "Recipe Type")
            {
                errorText = "Must choose a recipe type";
                return false;
            }
            return true;
        }
        // בדיקת תקינות תיאור המתכון
        private bool IsValidDescription()
        {
            if (etDescription.Text == "")
            {
                errorText = "Insert Description";
                return false;
            }
            return true;
        }
        // בדיקת תקינות מרכיבי המתכון
        private bool IsValidIngredients()
        {
            if (etDescription.Text == "")
            {
                errorText = "Insert Ingredients";
                return false;
            }
            return true;
        }
        // בדיקת תקינות הוראות המתכון
        private bool IsValidInstructions()
        {
            if (etDescription.Text == "")
            {
                errorText = "Insert Instructions";
                return false;
            }
            return true;
        }

        // פעולה הנקראת לאחר לחיצה על כפתור יצירת מתכון
        private void BtnCreateRecipe_Click(object sender, EventArgs e)
        {
            // בדיקה האם הפרטים תקינים
            if (IsValid())
            {
                // SharedPreferencesManager קבלת שם המשתמש המחובר באמצעות מחלקת העזר
                string recipeUsername = SharedPreferencesManager.GetUsername();

                // יצירת קוד מתכון אשר מורכב מסוג המתכון, שם המתכון ושם המשתמש, אשר מופרדים בנקודותיים
                string recipeId = tvRecipeType.Text + ":" + etRecipeName.Text + ":" + recipeUsername;

                // האם המתכון קיים לפי קוד המתכון הנ"ל DatabaseManager בדיקה באמצעות מחלקת העזר 
                if (!DatabaseManager.IsExistRecipe(recipeId))
                {
                    // שמצביע על ביטמפ של פקד תמונת המתכון Bitmap יצירת אובייקט 
                    Bitmap imageBitmap = ((BitmapDrawable)ivImage.Drawable).Bitmap;

                    // שממירה ביטמפ למחרוזת ImageManager מחרוזת המכילה את תוכן התמונה באמצעות שימוש במחלקת עזר
                    string recipeImage = ImageManager.BitmapToBase64(imageBitmap);

                    // בדיקה איזה סוג מתכון נבחר
                    if (tvRecipeType.Text == "Salad")
                    {
                        // DatabaseManager יצירת אובייקט סלט והצבתו במסד הנתונים באמצעות מחלקת העזר    
                        Salad salad = new Salad(tvTypeDetails[0].Text, tvTypeDetails[1].Text, tvTypeDetails[2].Text, tvTypeDetails[3].Text, recipeId, etRecipeName.Text, recipeUsername, recipeImage, tvCategory.Text, int.Parse(etHours.Text), int.Parse(etMinutes.Text), etDescription.Text, etIngredients.Text, etInstructions.Text);
                        DatabaseManager.AddRecipe(salad);
                    }
                    if (tvRecipeType.Text == "Soup")
                    {
                        // DatabaseManager יצירת אובייקט מרק והצבתו במסד הנתונים באמצעות מחלקת העזר    
                        Soup soup = new Soup(tvTypeDetails[0].Text, tvTypeDetails[1].Text, int.Parse(tvTypeDetails[2].Text), int.Parse(tvTypeDetails[3].Text), recipeId, etRecipeName.Text, recipeUsername, recipeImage, tvCategory.Text, int.Parse(etHours.Text), int.Parse(etMinutes.Text), etDescription.Text, etIngredients.Text, etInstructions.Text);
                        DatabaseManager.AddRecipe(soup);
                    }
                    if (tvRecipeType.Text == "Meat")
                    {
                        // DatabaseManager יצירת אובייקט מנת בשר והצבתו במסד הנתונים באמצעות מחלקת העזר    
                        Meat meat = new Meat(tvTypeDetails[0].Text, tvTypeDetails[1].Text, tvTypeDetails[2].Text, int.Parse(tvTypeDetails[3].Text), recipeId, etRecipeName.Text, recipeUsername, recipeImage, tvCategory.Text, int.Parse(etHours.Text), int.Parse(etMinutes.Text), etDescription.Text, etIngredients.Text, etInstructions.Text);
                        DatabaseManager.AddRecipe(meat);
                    }
                    if (tvRecipeType.Text == "Pastry")
                    {
                        // DatabaseManager יצירת אובייקט מאפה והצבתו במסד הנתונים באמצעות מחלקת העזר    
                        Pastry pastry = new Pastry(tvTypeDetails[0].Text, tvTypeDetails[1].Text, int.Parse(tvTypeDetails[2].Text), int.Parse(tvTypeDetails[3].Text),  recipeId, etRecipeName.Text, recipeUsername, recipeImage, tvCategory.Text, int.Parse(etHours.Text), int.Parse(etMinutes.Text), etDescription.Text, etIngredients.Text, etInstructions.Text);
                        DatabaseManager.AddRecipe(pastry);
                    }

                    // עדכון קודי המתכונים של המשתמש במסד הנתונים 
                    User currentUser = DatabaseManager.GetUser(recipeUsername);

                    // קוד המתכון הנ"ל מתווסף למחרוזת קודי המתכונים של המשתמש המופרדים בפסיקים
                    currentUser.myRecipesId += recipeId + ",";
                    DatabaseManager.SetUser(currentUser);

                    // StatusNotificationService
                    // בדיקה שהמשתמש מרשה הודעות
                    if (currentUser.allowNotification)
                    {
                        // הפעלת שירות הודעות לאחר יצירת מתכון
                        StartNotificationService();
                    }

                    // סיום וסגירת האקטיביטי וחזרה לדף הבית 
                    Finish();
                }
                else
                {
                    // אם המתכון קיים במסד הנתונים תוצג הודעה מתאימה
                    errorText = "Recipe already exists";
                    ShowAlertDialog();
                }
            }
            else
            {
                // במידה ולא, תופיע הודעת שגיאה מתאימה
                ShowAlertDialog();
            }
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

        // פעולה המתחילה את שירות ההודעות ושולחת תוכן הודעה מתאים
        private void StartNotificationService()
        {
            // הפעלת שירות ההודעות ושליחת הודעה מתאימה לאחר הכנת מתכון
            Intent serviceIntent = new Intent(this, typeof(StatusNotificationService));
            // שליחת מערך מחרוזות שמייצג את כותרת ותוכן ההודעה
            string[] createRecipeDetails = { "A New Recipe Was Created", etRecipeName.Text + " was successfully created" };
            serviceIntent.PutExtra("StatusNotification_Details", createRecipeDetails);
            StartService(serviceIntent);
        }
    }
}