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
    [Activity(Label = "UserListActivity")]
    public class UserListActivity : Activity, AdapterView.IOnItemClickListener, AdapterView.IOnItemLongClickListener
    {
        // רשימת כל המשתמשים באפליקציה
        private List<User> userList;

        // רשימת משתמשים מתעדכנת לפי הטקסט בסרגל החיפוש
        private List<User> currentUsers;

        // currentUsers אדפטר של המתכונים לפי רשימת
        private UserAdapter userAdapter;

        // המכיל בתוכו את כל המשתמשים המוצגים במסך ListView 
        private ListView lvUsers;

        // usernameList רשימת שמות המשתמשים המתעדכנת לפי 
        private List<string> usernameList;

        // אדפטר של שמות המשתמשים שמאפשר הצגת שמות משתמשים ברגע החיפוש בהתאמה
        private ArrayAdapter usernameAdapter;

        // ומציג מתחתיו את שמות המשתמשים לפי החיפוש EditText המתפקד כמו AutoCompleteTextView פקד מטיפוס 
        private AutoCompleteTextView actvSearch;

        // כותרת טור שמות המשתמשים
        private TextView tvHeaderUsername;

        // משתנה בוליאני שמייצג הצגת משתמשים בצורה אלפביתית או הפוך
        private bool usernameAscending;

        // כפתור חזרה לדף הבית
        private Button btnGoBack;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.userList_layout);

            lvUsers = FindViewById<ListView>(Resource.Id.userList_lvUsers);
            tvHeaderUsername = FindViewById<TextView>(Resource.Id.userList_tvHeaderUsername);
            actvSearch = FindViewById<AutoCompleteTextView>(Resource.Id.userList_actvSearch);
            btnGoBack = FindViewById<Button>(Resource.Id.userList_btnGoBack);

            // קבלת כל המשתמשים ממחלקת עזר  
            userList = DatabaseManager.GetAllUsers();

            //סידור רשימת המשתמשים לפי הסדר האלפבתי והמספרי של המשתמשים
            userList = (from user in userList orderby user.username select user).ToList();

            //רשימה שתכיל את כל הערכים המתעדכנים כאשר מסננים את המשתמשים
            currentUsers = userList.ToList();

            // כך שהמשתמשים מוצגים למשתמש lvRecipes.Adapterיצירת אדפטר המשתמשים והשמתו ב
            userAdapter = new UserAdapter(this, currentUsers);
            lvUsers.Adapter = userAdapter;

            lvUsers.OnItemClickListener = this; // Click on view - open user settings
            lvUsers.OnItemLongClickListener = this; // Long click on view - delete or promote user

            // תחילה המשתמשים מסודרים בסדר אלפביתי ולכן המשתנה הבוליאני יראה אמת
            usernameAscending = true;

            // שיציג את כל המשתמשים בהתאמה actvSearch עדכון 
            UpdateUsernameAdapter();

            tvHeaderUsername.Click += TvHeaderUsername_Click;
            actvSearch.TextChanged += ActvSearch_TextChanged;
            btnGoBack.Click += delegate { Finish(); };
        }

        /* actvSearch-פעולת עזר שמטרתה לעדכן את האדפטר של שמות המשתמשים כך ש
         יראה את שמות המשתמשים הקיימים בפתיחת הדף וכאשר מנהל מוחק משתמש */
        private void UpdateUsernameAdapter()
        {
            // רשימת שמות כל המשתמשים
            usernameList = new List<string>();

            // לולאה שמכניסה את ערכי כל שמות המשתמשים לרשימה
            for (int i = 0; i < userList.Count; i++)
            {
                usernameList.Add(userList[i].username);
            }

            // אשר מייצג אדפטר של שמות כל המשתמשים ArrayAdapter יצירת 
            usernameAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, usernameList);

            // כך שסרגל החיפוש מראה את שמות המשתמשים actvSearch.Adapterהשמת האדפטר הנ"ל ב
            actvSearch.Adapter = usernameAdapter;
        }

        // פעולה שנקראת כשכותרת שמות המשתמשים נלחצת
        private void TvHeaderUsername_Click(object sender, EventArgs e)
        {
            // בדיקה אם המשתנה הבוליאני מראה אמת כלומר הרשימה מסודרת אלפביתית
            if (usernameAscending)
            {
                // סידור רשימת כל המשתמשים ורשימת המשתמשים המתעדכנת בסדר הפוך
                currentUsers = (from user in currentUsers orderby user.username descending select user).ToList();
                userList = (from user in userList orderby user.username descending select user).ToList();
            }
            else
            {
                // סידור רשימת כל המשתמשים ורשימת המשתמשים המתעדכנת בסדר אלפביתי
                currentUsers = (from user in currentUsers orderby user.username select user).ToList();
                userList = (from user in userList orderby user.username select user).ToList();
            }

            // המציג את המשתמשים ListView עדכון רשימת המשתמשים המתעדכנת באדפטר ועדכון 
            userAdapter.SetUsers(currentUsers);
            userAdapter.NotifyDataSetChanged();

            // שינוי הערך הבוליאני של המשתנה
            usernameAscending = !usernameAscending;
        }

        // פעולה הנקראת כאשר טקסט מוקלד בסרגל החיפוש
        private void ActvSearch_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            // foreeach variable in list it selects the right variables and puts them in a list
            // מצביעה על רשימה כך ששמות המשתמשים מתחילים בטקסט שהוקלד, אין הפרדה לאותיות קטנות וגדולות בחיפוש currentUsers
            currentUsers = (from user in userList where user.username.StartsWith(actvSearch.Text, StringComparison.OrdinalIgnoreCase) select user).ToList();

            // עדכון אדפטר המשתמשים ועדכון המשתמשים בדף
            userAdapter.SetUsers(currentUsers);
            userAdapter.NotifyDataSetChanged();
        }

        // צפייה בפרטי משתמש על ידי לחיצה על רשומת המשתמש
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            // פתיחת טופס הגדרות המשתמש
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            SettingsDialogFragment settingsDialogFragment = new SettingsDialogFragment(currentUsers[position]);
            settingsDialogFragment.Show(transaction, "Settings dialog fragment");
        }
        // לאחר עדכון סיסמא של משתמש ע"י מנהל במסד הנתונים SettingsDialogFragmentפעולה שממומשת ב 
        // הפעולה מקבלת שם משתמש וסיסמא חדשה
        public void UpdatePassword(string username, string newPassword)
        {
            // קבלת המשתמש ממסד הנתונים על פי שם המשתמש
            User currentUser = (from user in userList where user.username == username select user).Single();
            // ListViewעדכון שם המשתמש הנ"ל ברשומה ועדכון ה
            currentUser.password = newPassword;
            userAdapter.NotifyDataSetChanged();
        }

        // לחיצה ארוכה על רשומת משתמש פותחת דיאלוג עם אפשרות לקידום או מחיקת משתמש
        public bool OnItemLongClick(AdapterView parent, View view, int position, long id)
        {
            // קבלת המשתמש הנלחץ לפי מקומו ברשימה
            User currentUser = currentUsers[position];

            // מנהל לא יכול למחוק את עצמו או להוריד את עצמו לדרגת משתמש רגיל
            // בדיקה האם המשתמש הנלחץ הוא לא המשתמש המחובר כלומר המנהל
            if (currentUser.username != SharedPreferencesManager.GetUsername())
            {
                // יצירת דיאלוג למחיקת משתמש או העלאתו למנהל ולהפך 
                AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
                alertDialog.SetTitle("Delete User");
                alertDialog.SetMessage("Are you sure you want to delete: " + currentUser.username + "?");
                alertDialog.SetCancelable(true);

                // בדיקה האם המשתמש הנלחץ הוא לא מנהל
                if (currentUser.isAdmin == false)
                {
                    // מנהל יכול לקדם כל משתמש לדרגת מנהל
                    alertDialog.SetPositiveButton("ADD ADMIN", delegate
                    {
                        // עדכון התכונה בוליאנית המציינת האם המשתמש מנהל במסד הנתונים
                        // עתה המשתמש הנ"ל מנהל
                        currentUser.isAdmin = true;
                        DatabaseManager.SetUser(currentUser);
                        // עדכון הרשומה ככך שעתה צבע הרשומה הוא אדום כהה
                        userAdapter.NotifyDataSetChanged();
                    });

                    // מנהל יכול למחוק כל משתמש באפליקציה חוץ ממנהלים אחרים
                    alertDialog.SetNegativeButton("DELETE", delegate
                    {
                        /* המרת מחרוזת הקודים של המתכונים שהמשתמש הנמחק יצר המופרדים בפסיקים לרשימה אשר מכילה את הקודים
                         StringSplitOptions.RemoveEmptyEntries מאחר והתו אחרי הפסיק האחרון הוא מחרוזת ריקה אזי יש להשתמש ב
                         על מנת שהרשימה תכיל את קודי המתכונים בלבד */
                        List<string> myRecipeIdList = currentUser.myRecipesId.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                        // מעבר על כל המשתמשים הרשומים באפליקציה
                        for (int i = 0; i < userList.Count; i++)
                        {
                            // מעבר כל פעם על רשימת קודי המתכונים שהמשתמש הנמחק יצר
                            for (int j = 0; j < myRecipeIdList.Count; j++)
                            {
                                // תנאי במידה והמשתמש אכן אוהב את המתכון של המשתמש הנמחק
                                if (userList[i].favoriteRecipesId.Contains(myRecipeIdList[j]))
                                {
                                    // מחיקת קוד המתכון האהוב אצל משתמשים אחרים שאוהבים את המתכון של המשתמש הנמחק
                                    userList[i].favoriteRecipesId = userList[i].favoriteRecipesId.Replace(myRecipeIdList[j] + ",", "");
                                    // עדכון המשתמש במסד הנתונים
                                    DatabaseManager.SetUser(userList[i]);
                                }
                            }
                        }

                        // הורדת מספר הלייקים במתכונים שונים במידה והמשתמש הנמחק אהב אותם
                        // רשימת המתכונים האהובים של המשתמש הנמחק
                        List<Recipe> favoriteRecipeList = DatabaseManager.GetRecipesById(currentUser.favoriteRecipesId);
                        // לולאה העוברת על כל המתכונים האהובים של המשתמש הנמחק
                        for (int i = 0; i < favoriteRecipeList.Count; i++)
                        {
                            // הורדת הלייקים תעשה במתכונים שלא נוצרו ע"י המשתמש הנמחק
                            // בדיקה ששם יוצר המתכונים האהובים של המשתמש הנמחק הוא לא המשתמש עצמו
                            if (favoriteRecipeList[i].recipeUsername != currentUser.username)
                            {
                                // הורדת הלייקים באחד אצל המתכונים האהובים של המשתמש הנמחק ועדכון המתכון במסד הנתונים
                                favoriteRecipeList[i].likeCount--;
                                DatabaseManager.SetRecipe(favoriteRecipeList[i]);
                            }
                        }

                        // מחיקת המתכונים שהמשתמש הנמחק יצר
                        // לולאה העוברת על קודי המתכונים של המשתמש ובעזרת מחלקת עזר מוחקת כל מתכון שיצר
                        for (int i = 0; i < myRecipeIdList.Count; i++)
                        {
                            DatabaseManager.DeleteRecipe(DatabaseManager.GetRecipe(myRecipeIdList[i]));
                        }

                        // מחיקת המשתמש עצמו ממסד הנתונים
                        DatabaseManager.DeleteUser(currentUser);

                        // מתאימה Toast הצגת הודעת 
                        Toast.MakeText(this, "User: " + currentUser.username + " was deleted", ToastLength.Long).Show();
                        // מחיקת המשתמש ברשימת כל המשתמשים וברשימת המשתמשים המתעדכנת
                        userList.Remove(currentUser);
                        currentUsers.RemoveAt(position);

                        // עדכון אדפטר שמות המשתמשים של סרגל החיפוש 
                        UpdateUsernameAdapter();
                        // המשתמשים ListView עדכון אדפטר המשתמשים המעדכן את  
                        userAdapter.NotifyDataSetChanged();

                        // במצב של מחיקת משתמש המנהל רשאי לשלוח לו אימייל על כך
                        SendEmail(currentUser);
                    });
                }
                else
                {
                    // מנהל יכול להוריד כל מנהל אחר לדרגת משתמש רגיל
                    alertDialog.SetPositiveButton("REMOVE ADMIN", delegate
                    {
                        // עדכון התכונה בוליאנית המציינת האם המשתמש מנהל במסד הנתונים
                        // עתה המשתמש הנ"ל מורד מדרגת מנהל ע"י מנהל אחר
                        currentUser.isAdmin = false;
                        DatabaseManager.SetUser(currentUser);
                        // עדכון הרשומה ככך שעתה צבע הרשומה הוא כתום
                        userAdapter.NotifyDataSetChanged();
                    });
                }
                alertDialog.SetNeutralButton("CANCEL", delegate
                {
                    alertDialog.Dispose();
                });
                alertDialog.Show();
                return true;
            }
            else
            {
                // כאשר מנהל לוחץ לחיצה ארוכה על הרשומה שלו לא תהיה תגובה
                return false;
            }
        }

        // פעולה המקבלת משתמש ופותחת דף לשליחת אימייל לאחר מחיקתו
        private void SendEmail(User currentUser)
        {
            string[] email = { currentUser.email }; // אימייל המשתמש הנמחק
            Intent sendEmailIntent = new Intent(Intent.ActionSend);
            sendEmailIntent.SetType("message/rfc822"); // Opening email clients
            sendEmailIntent.PutExtra(Intent.ExtraEmail, currentUser.email); // נמען
            sendEmailIntent.PutExtra(Intent.ExtraSubject, "Your account was banned from YumYum because of terms of service violation!"); // כותרת
            sendEmailIntent.PutExtra(Intent.ExtraText, "Due to misbehavior your account was deleted from YumYum with all of your recipes!"); // תוכן
            // הצגת דף שליחת אימייל עם התכנים הרלוונטיים
            StartActivity(sendEmailIntent);
        }
    }
}