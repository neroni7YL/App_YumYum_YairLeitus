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
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        // של שם המשתמש EditText פקד
        private EditText etUsername;

        // של הסיסמא EditText פקד
        private EditText etPassword;

        // לשמירת המשתמש במערכת בכניסה הבאה CheckBox פקד
        private CheckBox cbRememberMe;

        // פקד כפתור להתחברות
        private Button btnLogin;

        // פקד כפתור לשכחתי סיסמא
        private Button btnForgotPassword;

        // כפתור מעבר לדף ההרשמה
        private Button btnGotoSignup;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login_layout);

            etUsername = FindViewById<EditText>(Resource.Id.login_etUsername);
            etPassword = FindViewById<EditText>(Resource.Id.login_etPassword);
            cbRememberMe = FindViewById<CheckBox>(Resource.Id.login_cbRememberMe);
            btnLogin = FindViewById<Button>(Resource.Id.login_btnLogin);
            btnForgotPassword = FindViewById<Button>(Resource.Id.login_btnForgotPassword);
            btnGotoSignup = FindViewById<Button>(Resource.Id.login_btnGotoSignup);

            btnLogin.Click += BtnLogin_Click;
            btnForgotPassword.Click += BtnForgotPassword_Click;
            btnGotoSignup.Click += BtnGotoSignup_Click;
        }

        // לחיצה על כפתור התחברות
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // בדיקה האם במסד הנתונים ישנו משתמש שפרטי שם המשתמש והסיסמא תואמים
            if (DatabaseManager.IsValidUser(etUsername.Text, etPassword.Text))
            {
                // ISharedPreferences שמירת נתוני שם המשתמש וסטטוס המשתמש באמצעות 
                // CheckBoxסטטוס המשתמש נקבע על פי ה
                SharedPreferencesManager.SetUsername(etUsername.Text);
                SharedPreferencesManager.SetRememberMe(cbRememberMe.Checked);

                // StatusNotificationService
                // בדיקה שהמשתמש מרשה הודעות
                if (DatabaseManager.GetUser(etUsername.Text).allowNotification)
                {
                    // הפעלת שירות הודעות לאחר התחברות
                    StartNotificationService();
                }
                
                // סגירת דף ההתחברות ומעבר לדף הבית
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
            alertDialog.SetMessage("Incorrect username or password");
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
            // ע"י שליחת תוכן הודעה מתאים לסרוויס CheckBoxהפעלת שירות ההודעות על פי ה 
            Intent serviceIntent = new Intent(this, typeof(StatusNotificationService));
            string[] loginDetails;
            // וזכור במערכת, תוכן ההודעה יופיע בהתאמה CheckBoxבדיקה האם המשתמש לחץ על ה
            if (cbRememberMe.Checked)
            {
                loginDetails = new string[] { "Login Completed Successfully", etUsername.Text + ", you are remembered in the system" };
            }
            else
            {
                loginDetails = new string[] { "Login Completed Ssuccessfully", etUsername.Text + ", you are not remembered in the system" };
            }
            serviceIntent.PutExtra("StatusNotification_Details", loginDetails);
            StartService(serviceIntent);
        }

        // להצגת טופס שינוי סיסמא ForgotPasswordDialogFragment פתיחת 
        private void BtnForgotPassword_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            ForgotPasswordDialogFragment forgotPasswordDialogFragment = new ForgotPasswordDialogFragment();
            forgotPasswordDialogFragment.Show(transaction, "Forgot Password dialog fragment");
        }

        // מעבר לדף ההרשמה בלחיצה על הכפתור
        private void BtnGotoSignup_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SignupActivity));
            StartActivity(intent);
            Finish();
        }
    }
}