using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Refractored.Controls;

namespace App_YumYum_YairLeitus
{
    public class SettingsDialogFragment : DialogFragment
    {
        // משתמש נוכחי להגדרות
        private User currentUser;

        // של שם פרטי TextView פקד
        private TextView tvName;

        // של שם משתמש TextView פקד
        private TextView tvUsername;

        // של אימייל TextView פקד
        private TextView tvEmail;

        // של תשובה על שאלת אבטחה TextView פקד
        private TextView tvSecurityQuestion;

        // כפתור לשינוי סיסמא
        private Button btnChangePassword;

        // לשינוי סיסמא EditText פקד
        private EditText etChangePassword;

        // לאישור שינוי סיסמא ImageButton פקד
        private ImageButton ibChangePassword;

        // להסתרת תוכן שינוי סיסמא LinearLayout
        private LinearLayout layoutChangePassword;

        // layoutChangePassword לפתיחה וסגירה של btnChangePassword סטטוס כפתור 
        private bool changePasswordStatus;

        // תמונה פרופיל עגולה של המשתמש
        private CircleImageView civUserImage;

        // כפתור להגדרת אישור הופעת הודעות
        private Button btnAllowNotification;

        // של כמות מתכונים אהובים TextView פקד
        private TextView tvFavoriteRecipes;

        // של כמות מתכונים שהמשתמש יצר TextView פקד
        private TextView tvMyRecipes;

        // משתנה השומר את ההודעה המתאימה במקרה של שגיאה
        private string errorText;

        // פעולה בונה המקבלת משתמש נוכחי
        public SettingsDialogFragment(User currentUser)
        {
            this.currentUser = currentUser;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            // כך שהוא יוצג על המסך DialogFragment ניפוח
            View view = inflater.Inflate(Resource.Layout.settingsDialogFragment_layout, container, false);

            tvName = view.FindViewById<TextView>(Resource.Id.settingsDialogFragment_tvName);
            tvUsername = view.FindViewById<TextView>(Resource.Id.settingsDialogFragment_tvUsername);
            tvEmail = view.FindViewById<TextView>(Resource.Id.settingsDialogFragment_tvEmail);
            tvSecurityQuestion = view.FindViewById<TextView>(Resource.Id.settingsDialogFragment_tvSecurityQuestion);
            btnChangePassword = view.FindViewById<Button>(Resource.Id.settingsDialogFragment_btnChangePassword);
            etChangePassword = view.FindViewById<EditText>(Resource.Id.settingsDialogFragment_etChangePassword);
            ibChangePassword = view.FindViewById<ImageButton>(Resource.Id.settingsDialogFragment_ibChangePassword);
            layoutChangePassword = view.FindViewById<LinearLayout>(Resource.Id.settingsDialogFragment_layoutChangePassword);
            civUserImage = view.FindViewById<CircleImageView>(Resource.Id.settingsDialogFragment_civAddUserImage);
            btnAllowNotification = view.FindViewById<Button>(Resource.Id.settingsDialogFragment_btnAllowNotification);
            tvFavoriteRecipes = view.FindViewById<TextView>(Resource.Id.settingsDialogFragment_tvFavoriteRecipesCount);
            tvMyRecipes = view.FindViewById<TextView>(Resource.Id.settingsDialogFragment_tvMyRecipesCount);

            // תחילה נתוני שינוי הסיסמא אינם מופיעים
            changePasswordStatus = false;

            // השמת פרטי המשתמש
            tvName.Text = "Welcome :  " + currentUser.name;
            tvUsername.Text = currentUser.username;
            tvEmail.Text = currentUser.email;
            tvSecurityQuestion.Text = currentUser.securityQuestion;
            // בדיקה אם למשתמש יש תמונת פרופיל
            if (currentUser.userImage != "")
            {
                // הצבת תמונת הפרופיל
                Bitmap btmImage = ImageManager.Base64ToBitmap(currentUser.userImage);
                civUserImage.SetImageBitmap(btmImage);
            }
            if (currentUser.allowNotification)
            {
                // טקסט ירוק כאשר המשתמש מרשה הודעות
                btnAllowNotification.SetTextColor(Color.SpringGreen);
            }
            else
            {
                // טקסט אדום כאשר המשתמש לא מרשה הודעות
                btnAllowNotification.SetTextColor(Color.DarkRed);
            }
            // הצגת מספר המתכונים האהובים והמתכונים שלי
            tvFavoriteRecipes.Text = "Favorite Recipes :   " + (currentUser.favoriteRecipesId.Split(',').Length - 1).ToString();
            tvMyRecipes.Text = "My Recipes :   " + (currentUser.myRecipesId.Split(',').Length - 1).ToString();
            
            btnChangePassword.Click += BtnChangePassword_Click;
            ibChangePassword.Click += IbChangePassword_Click;
            civUserImage.Click += CivUserImage_Click;
            btnAllowNotification.Click += BtnAllowNotification_Click;

            return view;
        }

        // נלחץ btnChangePassword פעולה המתבצעת כאשר 
        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            // אם כבר לחצו על הכפתור - מסתיר את חלק שינוי הסיסמא
            if (changePasswordStatus)
            {
                layoutChangePassword.Visibility = ViewStates.Gone;
                etChangePassword.Text = "";
            }
            else
            {
                // אם לא לחצו על הכפתור - מראה את חלק שינוי הסיסמא
                layoutChangePassword.Visibility = ViewStates.Visible;
            }
            // שינוי סטטוס הכפתור
            changePasswordStatus = !changePasswordStatus;
        }

        // ibChangePassword פעולה המתבצעת כאשר לוחצים על הכפתור 
        private void IbChangePassword_Click(object sender, EventArgs e)
        {
            // בדיקה אם הסיסמא תקינה
            if (IsValidPassword())
            {
                // שינוי סיסמת המשתמש ועדכון במסד הנתונים
                currentUser.password = etChangePassword.Text;
                DatabaseManager.SetUser(currentUser);
                Toast.MakeText(Context, "Password changed successfully to: " + currentUser.password, ToastLength.Long).Show();

                // הוא דף רשימת המשתמשים כלומר מדובר במנהל Activityבדיקה שה
                if (Activity is UserListActivity)
                {
                    // פעולה שמעדכנת את רשימת המשתמשים בדף
                    ((UserListActivity)Activity).UpdatePassword(currentUser.username, currentUser.password);

                    // בדיקה שהמשתמש בהגדרות הוא לא המשתמש המחובר כדי שמנהל לא ישלח לעצמו מייל
                    if (currentUser.username != SharedPreferencesManager.GetUsername())
                    {
                        // במצב של שינוי סיסמא ע"י מנהל, המנהל רשאי לשלוח אימייל למשתמש
                        string subject = "YumYum :  Password has been changed by an admin.";
                        string text = string.Format("Hi, {0} your password was changed to {1}.", currentUser.name, currentUser.password);
                        SendEmail(currentUser, subject, text);
                    }
                }
                Dismiss();
            }
            else
            {
                // הצגת שגיאת תקינות של הסיסמא
                ShowAlertDialog();
            }
        }

        // בדיקות תקינות הסיסמא
        private bool IsValidPassword()
        {
            //Password Validation
            // סיסמא לא יכולה להיות ריקה
            if (etChangePassword.Text == "")
            {
                errorText = "Insert password";
                return false;
            }

            //יש בדיקה שהסיסמא לא תכיל יותר מ16 תווים axml בקובץ
            // סיסמא בין 8 ל16 תווים
            if (etChangePassword.Text.Length < 8)
            {
                errorText = "Password length must be between 8 - 16 letters";
                return false;
            }

            // בדיקה שהסיסמא מכילה רק ספרות ואותיות באנגלית
            // פעולה שעוברת על כל המחרוזת ובודקת שהיא מכילה רק מספרים ואותיות באנגלית
            if (!Regex.IsMatch(etChangePassword.Text, "^[a-zA-Z0-9]+$"))
            {
                errorText = "Password must contain English letters and numbers only";
                return false;
            }

            // בדיקה אם הסיסמא מכילה לפחות אות באנגלית אחת ומספר אחד
            // Match a single alphabetic character(a through z or A through Z) and numeric character
            if (!(Regex.IsMatch(etChangePassword.Text, "[a-zA-Z]") && Regex.IsMatch(etChangePassword.Text, "[0-9]")))
            {
                errorText = "Password must contain at least one English letter and one number";
                return false;
            }

            // בדיקה שהסיסמא החדשה לא שווה לסיסמא הישנה
            if (etChangePassword.Text == currentUser.password)
            {
                errorText = "An existing password";
                return false;
            }
            return true;
        }
        // errorText המודיע על שגיאה לפי המשתנה AlertDialog פעולה היוצרת
        private void ShowAlertDialog()
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(Context);
            alertDialog.SetTitle("Error");
            alertDialog.SetMessage(errorText);
            alertDialog.SetCancelable(true);
            alertDialog.SetNeutralButton("CANCEL", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }

        // פעולה המתבצעת כאשר לוחצים על תמונת הפרופיל
        private void CivUserImage_Click(object sender, EventArgs e)
        {
            // שבו ניתן לבחור את העלת תמונת פרופיל באמצעות מצלמה או למחוק את תמונת הפרופיל AlertDialog בניית 
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(Context);
            alertDialog.SetTitle("Choose a user image");
            alertDialog.SetMessage("Take a profile image from camera or remove it");
            alertDialog.SetCancelable(true);
            alertDialog.SetPositiveButton("CAMERA", delegate 
            {
                // שמפנה למצלמת הטלפון Intent
                Intent cameraIntent = new Intent(MediaStore.ActionImageCapture);

                // הפעלת הפעולה הנ"ל אשר שולחת קוד 0 אשר מציין שהחזרה לאפליקציה היא מהמצלמה
                StartActivityForResult(cameraIntent, 0);
            });
            alertDialog.SetNegativeButton("REMOVE", delegate
            {
                // איפוס תמונת הפרופיל במסך 
                civUserImage.SetImageBitmap(null);
                civUserImage.SetImageResource(Resource.Drawable.addUserImage);
                // איפוס תמונת הפרופיל של המשתמש במסד הנתונים
                currentUser.userImage = "";
                DatabaseManager.SetUser(currentUser);

                // הוא דף רשימת המשתמשים כלומר מדובר במנהל Activityבדיקה שה
                // ובדיקה שדף ההגדרות הוא לא של המנהל עצמו
                if (Activity is UserListActivity && currentUser.username != SharedPreferencesManager.GetUsername())
                {
                    // במצב של הסרת תמונת משתמש ע"י מנהל, המנהל רשאי לשלוח אימייל למשתמש
                    string subject = "YumYum :  Profile image has been removed by an admin.";
                    string text = string.Format("Hi, {0} your profile image was changed, check it in the app.", currentUser.name);
                    SendEmail(currentUser, subject, text);
                }
            });
            alertDialog.SetNeutralButton("CANCEL", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }
        // פעולה שמתבצעת בעת החזרה לאפליקציה מהמצלמה
        public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // בדיקה האם החזרה בוצעה בהצלחה מהמצלמה
            if (resultCode == Result.Ok && requestCode == 0)
            {
                // Get the image bitmap from the intent extras
                // שומר את תוכן התמונה מהמצלמה, ומעדכן את תמונת הפרופיל בדף ובמסד הנתונים
                Bitmap imageBitmap = (Bitmap)data.Extras.Get("data");
                civUserImage.SetImageBitmap(imageBitmap);
                currentUser.userImage = ImageManager.BitmapToBase64(imageBitmap);
                DatabaseManager.SetUser(currentUser);
            }
        }

        // לחיצה על כפתור אישור הודעות
        private void BtnAllowNotification_Click(object sender, EventArgs e)
        {
            // לחיצה כאשר הכפתור ירוק
            if (btnAllowNotification.CurrentTextColor == Color.SpringGreen)
            {
                // הפיכת הכפתור לאדום וביטול אישור ההודעות
                btnAllowNotification.SetTextColor(Color.DarkRed);
                currentUser.allowNotification = false;
                DatabaseManager.SetUser(currentUser);
                // במידה והוא פועל StatusNotificationService הפסקת הסרוויס
                Intent serviceIntent = new Intent(Activity, typeof(StatusNotificationService));
                Activity.StopService(serviceIntent);
            }
            else
            {
                // לחיצה כאשר הכפתור אדום
                // הפיכת הכפתור לירוק ומתן אישור ההודעות
                btnAllowNotification.SetTextColor(Color.SpringGreen);
                currentUser.allowNotification = true;
                DatabaseManager.SetUser(currentUser);
            }
        }

        // פעולה המקבלת משתמש ופותחת דף לשליחת אימייל לאחר שינוי בידי מנהל
        private void SendEmail(User currentUser, string subject, string text)
        {
            string[] email = { currentUser.email };
            Intent sendEmailIntent = new Intent(Intent.ActionSend);
            sendEmailIntent.SetType("message/rfc822"); // Opening email clients
            sendEmailIntent.PutExtra(Intent.ExtraEmail, email); // נמען
            sendEmailIntent.PutExtra(Intent.ExtraSubject, subject); // כותרת  
            sendEmailIntent.PutExtra(Intent.ExtraText, text); // תוכן
            // הצגת דף שליחת אימייל עם התכנים הרלוונטיים
            StartActivity(sendEmailIntent);
        }
    }
}