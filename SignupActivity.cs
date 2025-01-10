using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App_YumYum_YairLeitus
{
    [Activity(Label = "SignupActivity")]
    public class SignupActivity : Activity
    {
        // של שם פרטי EditText פקד 
        private EditText etName;

        // של אימייל EditText פקד 
        private EditText etEmail;

        // של שם המשתמש EditText פקד 
        private EditText etUsername;

        // של הסיסמא EditText פקד 
        private EditText etPassword;

        // של אישור הסיסמא EditText פקד 
        private EditText etConfirmPassword;

        // של תשובה לשאלת האבטחה EditText פקד 
        private EditText etSecurityQuestion;

        // פקד כפתור להרשמה
        private Button btnSignup;

        // כפתור מעבר לדף ההתחברות
        private Button btnGotoLogin;

        // מחרוזת עם המידע המתאים ברגע אי תקינות הנתונים
        private string errorText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.signup_layout);

            etName = FindViewById<EditText>(Resource.Id.signup_etName);
            etEmail = FindViewById<EditText>(Resource.Id.signup_etEmail);
            etUsername = FindViewById<EditText>(Resource.Id.signup_etUsername);
            etPassword = FindViewById<EditText>(Resource.Id.signup_etPassword);
            etConfirmPassword = FindViewById<EditText>(Resource.Id.signup_etConfirmPassword);
            etSecurityQuestion = FindViewById<EditText>(Resource.Id.signup_etSecurityQuestion);
            btnSignup = FindViewById<Button>(Resource.Id.signup_btnSignup);
            btnGotoLogin = FindViewById<Button>(Resource.Id.signup_btnGotoLogin);

            btnSignup.Click += BtnSignup_Click;
            btnGotoLogin.Click += BtnGotoLogin_Click;
        }

        // פעולה בוליאנית הבודקת אם הנתונים תקינים
        private bool IsValid()
        {
            // Name Validation
            if (!IsValidName())
            {
                return false;
            }

            // Email Validation
            if (!IsValidEmail())
            {
                return false;
            }

            // Username Validation
            if (!IsValidUsername())
            {
                return false;
            }

            // Password Validation
            if (!IsValidPassword())
            {
                return false;
            }

            // Security Question Validation
            if (!IsValidSecurityQuestion())
            {
                return false;
            }
            return true;
        }

        // בדיקת תקינות לשם פרטי
        private bool IsValidName()
        {
            // Name Validation
            // בדיקה שהם לא ריק
            if (etName.Text == "")
            {
                errorText = "Insert name";
                return false;
            }

            // בדיקה אם השם מכיל אך ורק אותיות לועזיות
            // פעולה שעוברת על המחרוזת ובודקת שהיא מכילה רק אותיות באנגלית
            if (!Regex.IsMatch(etName.Text, "^[a-zA-Z]+$"))
            {
                errorText = "Name must contain English letters only";
                return false;
            }
            return true;
        }

        // בדיקת תקינות לאימייל
        private bool IsValidEmail()
        {
            // Email Validation
            // בדיקה שהאימייל לא ריק
            if (etEmail.Text == "")
            {
                errorText = "Insert email";
                return false;
            }

            // בדיקה שהאימייל לא תפוס באפליקציה
            if (DatabaseManager.IsExistEmail(etEmail.Text))
            {
                errorText = "Email is already taken";
                return false;
            }

            // בדיקה בסיסית אם האימייל תקין מבחינת דקדוקית
            try
            {
                // MailAddress בעזרת מחלקת עזר מובניתי יוצר טיפוס 
                MailAddress addr = new MailAddress(etEmail.Text);
                // אם המייל תקין, כאשר לא מתקיים יוצר קריסה true מחזיר ערך בוליאני
                return addr.Address == etEmail.Text;
            }
            // catchו try במקרה של אי תקינות גורם לקריסה ולכן יש להשתמש ב
            catch
            {
                errorText = "Invalid email";
                return false;
            }
            
        }

        // בדיקת תקינות לשם משתמש
        private bool IsValidUsername()
        {
            // Username Validation
            // בדיקה ששם המשתמש לא ריק
            if (etUsername.Text == "")
            {
                errorText = "Insert username";
                return false;
            }

            // יש בדיקה ששם המשתמש לא יכיל יותר מ15 תווים axml בקובץ  
            // בדיקה שאורך שם המשתמש בין 3 ל15 תווים
            if (etUsername.Text.Length < 3)
            {
                errorText = "Username length must be between 3 - 15 letters";
                return false;
            }

            // בדיקה ששם המשתמש יכיל אך ורק אותיות ומספרים
            // פעולה שעוברת על כל המחרוזת ובודקת שהיא מכילה רק מספרים ואותיות באנגלית
            if (!Regex.IsMatch(etUsername.Text, "^[a-zA-Z0-9]+$"))
            {
                errorText = "Username must contain English letters and numbers only";
                return false;
            }

            // בדיקה אם שם המשתמש מכיל לפחות אות באנגלית אחת
            // Match a single alphabetic character(a through z or A through Z)
            if (!Regex.IsMatch(etUsername.Text, "[a-zA-Z]"))
            {
                errorText = "Username must contain at least one English letter";
                return false;
            }

            // בדיקה בעזרת מחלקת עזר ששם המשתמש לא קיים במערכת
            if (DatabaseManager.IsExistUsername(etUsername.Text))
            {
                errorText = "Username already exists";
                return false;
            }
            return true;
        }

        // בדיקת תקינות לסיסמא
        private bool IsValidPassword()
        {
            // Password Validation
            // בדיקה שהסיסמא לא ריקה
            if (etPassword.Text == "")
            {
                errorText = "Insert password";
                return false;
            }

            // יש בדיקה שהסיסמא לא תכיל יותר מ16 תווים axml בקובץ
            // בדיקה שאורך הסיסמא בין 8 ל16 תווים
            if (etPassword.Text.Length < 8)
            {
                errorText = "Password length must be between 8 - 16 letters";
                return false;
            }

            // בדיקה שהסיסמא תכיל אך ורק אותיות ומספרים
            // פעולה שעוברת על כל המחרוזת ובודקת שהיא מכילה רק מספרים ואותיות באנגלית 
            if (!Regex.IsMatch(etPassword.Text, "^[a-zA-Z0-9]+$"))
            {
                errorText = "Password must contain English letters and numbers only";
                return false;
            }

            // בדיקה אם הסיסמא מכילה לפחות אות באנגלית אחת ומספר אחד
            // Match a single alphabetic character(a through z or A through Z) and numeric character
            if (!(Regex.IsMatch(etPassword.Text, "[a-zA-Z]") && Regex.IsMatch(etPassword.Text, "[0-9]")))
            {
                errorText = "Password must contain at least one English letter and one number";
                return false;
            }

            // Password Confirmation Validation
            // בדיקה שאישור הסיסמא תואם לסיסמא
            if (!etPassword.Text.Equals(etConfirmPassword.Text))
            {
                errorText = "Invalid password confirmation";
                return false;
            }
            return true;
        }

        // בדיקת תקינות לתשובה על שאלת האבטחה
        private bool IsValidSecurityQuestion()
        {
            // Security Question Validation
            // בדיקה שהתשובה לא ריקה
            if (etSecurityQuestion.Text == "")
            {
                errorText = "Answer the security question";
                return false;
            }

            // יש בדיקה שהתשובה לא תכיל יותר מ20 תווים axml בקובץ
            // בדיקה שאורך הסיסמא בין 8 ל16 תווים
            if (etSecurityQuestion.Text.Length < 3)
            {
                errorText = "Security question answer must contain at least 3 letters";
                return false;
            }

            // בדיקה שהסיסמא תכיל אך ורק אותיות ומספרים
            // פעולה שעוברת על כל המחרוזת ובודקת שהיא מכילה רק מספרים ואותיות באנגלית 
            if (!Regex.IsMatch(etSecurityQuestion.Text, "^[a-zA-Z]+$"))
            {
                errorText = "Security question answer must contain English letters only";
                return false;
            }
            return true;
        }

        private void BtnSignup_Click(object sender, EventArgs e)
        {
            // בדיקה האם נתוני ההרשמה תקינים
            if (IsValid())
            {
                // עצם משתמש שיצביע על משתמש חדש שנוצר
                User user;

                // בדיקה האם מספר המשתמשים באפליקציה שווה ל0
                if (DatabaseManager.GetAllUsers().Count == 0)
                {
                    // תהיה אמת והוא יהווה כמנהל isAdminאם כן יווצר משתמש ראשון שתכונת ה
                    user = new User(etUsername.Text, etPassword.Text, etName.Text, etEmail.Text, etSecurityQuestion.Text, true);
                }
                else
                {
                    // תהיה שקר והוא יהווה כמשתמש רגיל isAdminאם כן יווצר משתמש שתכונת ה
                    user = new User(etUsername.Text, etPassword.Text, etName.Text, etEmail.Text, etSecurityQuestion.Text, false);
                }

                // הכנסת המשתמש למסד הנתונים
                DatabaseManager.AddUser(user);

                // ISharedPreferences שמירת נתוני שם המשתמש וסטטוס המשתמש באמצעות
                SharedPreferencesManager.SetUsername(etUsername.Text);
                // לאחר הרשמה המערכת זוכרת את המשתמש
                SharedPreferencesManager.SetRememberMe(true);

                // StatusNotificationService
                // התחלת שירות ההודעות לאחר הרשמה
                StartNotificationService();

                // סגירת דף ההרשמה ומעבר לדף הבית
                Intent intent = new Intent(this, typeof(HomepageActivity));
                StartActivity(intent);
                Finish();
            }
            else
            {
                // הצגת הודעת שגיאה מתאימה
                ShowAlertDialog();
            }
        }
        // כשהיא נקראת ומודיעה על שגיאה AlertDialog פעולה אשר מציגה 
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
            // הפעלת שירות ההודעות ושליחת הודעה מתאימה להרשמה
            Intent serviceIntent = new Intent(this, typeof(StatusNotificationService));
            // שליחת מערך מחרוזות שמייצג את כותרת ותוכן ההודעה
            string[] signupDetails = { "Successful Registration", etUsername.Text + ", you have successfully registered to YumYum" };
            serviceIntent.PutExtra("StatusNotification_Details", signupDetails);
            StartService(serviceIntent);
        }

        // מעבר לדף ההתחברות בלחיצה על הכפתור
        private void BtnGotoLogin_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
            Finish();
        }
    }
}