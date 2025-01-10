using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ForgotPasswordDialogFragment : DialogFragment
    {
        // של שם המשתמש EditText פקד
        private EditText etUsername;

        // של אימייל EditText פקד
        private EditText etEmail;

        // של תשובת שאלת האבטחה EditText פקד
        private EditText etSecurityQuestion;

        // פקד כפתור שליחה לאחר מילוי הטופס
        private Button btnSend;

        // המכיל את פרטי הסיסמא שיש להקליד LinearLayout
        private LinearLayout layoutChangePassword;

        // של הסיסמא החדשה EditText פקד 
        private EditText etChangePassword;

        // פקד כפתור שליחה לאישור הסיסמא החדשה
        private Button btnChangePassword;

        // מחרוזת עם המידע המתאים ברגע אי תקינות הנתונים
        private string errorText;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            // כך שהוא יוצג על המסך DialogFragment ניפוח 
            View view = inflater.Inflate(Resource.Layout.forgotPasswordDialogFragment_layout, container, false);

            etUsername = view.FindViewById<EditText>(Resource.Id.forgotPasswordDialogFragment_etUsername);
            etEmail = view.FindViewById<EditText>(Resource.Id.forgotPasswordDialogFragment_etEmail);
            etSecurityQuestion = view.FindViewById<EditText>(Resource.Id.forgotPasswordDialogFragment_etSecurityQuestion);
            btnSend = view.FindViewById<Button>(Resource.Id.forgotPasswordDialogFragment_btnSend);
            layoutChangePassword = view.FindViewById<LinearLayout>(Resource.Id.forgotPasswordDialogFragment_layoutChangePassword);
            etChangePassword = view.FindViewById<EditText>(Resource.Id.forgotPasswordDialogFragment_etChangePassword);
            btnChangePassword = view.FindViewById<Button>(Resource.Id.forgotPasswordDialogFragment_btnChangePassword);

            btnSend.Click += BtnSend_Click;
            btnChangePassword.Click += BtnChangePassword_Click;

            return view;
        }

        // פעולה המופעלת לאחר לחיצה על כפתור השליחה בתום מילוי הנתונים
        private void BtnSend_Click(object sender, EventArgs e)
        {
            // בדיקה האם נתוני שם המשתמש, האימייל והתשובה על שאלת האבטחה תואמים
            if (DatabaseManager.IsValidUser(etUsername.Text, etEmail.Text, etSecurityQuestion.Text))
            {
                // מוצג למשתמש layoutChangePasswordבמידה וכן הפקדים מושבתים ו
                etUsername.Enabled = false;
                etEmail.Enabled = false;
                etSecurityQuestion.Enabled = false;
                // מאפשר למשתמש לשנות סיסמא
                layoutChangePassword.Visibility = ViewStates.Visible;
            }
            else
            {
                // הצגת הודעת שגיאה מתאימה
                errorText = "Details are not valid and matched or security question answer is wrong!";
                ShowAlertDialog();
            }
        }

        // פעולה הנקראת כאשר כפתור אישור שינוי סיסמא נלחץ
        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            // בדיקה האם נתוני הסיסמא תקינים
            if (IsValidPassword())
            {
                // שינוי הסיסמא אצל המשתמש ועדכון הסיסמא במסד הנתונים
                User user = DatabaseManager.GetUser(etUsername.Text);
                user.password = etChangePassword.Text;
                DatabaseManager.SetUser(user);

                // הצגת הודעה מתאימה
                Toast.MakeText(Context, "Password changed successfully to: " + user.password, ToastLength.Long).Show();

                // ISharedPreferences שמירת נתוני שם המשתמש וסטטוס המשתמש באמצעות 
                SharedPreferencesManager.SetUsername(etUsername.Text);
                SharedPreferencesManager.SetRememberMe(true);

                // ודף ההתחברות DialogFragmentמעבר לדף הבית, סגירת ה 
                Intent intent = new Intent(Activity, typeof(HomepageActivity));
                StartActivity(intent);
                Dismiss();
                Activity.Finish();
            }
            else
            {
                // במידה ולא תופיע שגיאה
                ShowAlertDialog();
            }
        }
        // בדיקת תקינות הסיסמא
        private bool IsValidPassword()
        {
            // Password Validation
            // בדיקת חובת הזנת סיסמא
            if (etChangePassword.Text == "")
            {
                errorText = "Insert password";
                return false;
            }

            // יש בדיקה שהסיסמא לא תכיל יותר מ16 תווים axml בקובץ
            // בדיקה שגודל הסיסמא בין 8 ל16 תווים
            if (etChangePassword.Text.Length < 8)
            {
                errorText = "Password length must be between 8 - 16 letters";
                return false;
            }

            // בדיקה שהסיסמא תכיל אך ורק אותיות ומספרים
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
            return true;
        }
        // כשהיא נקראת ומודיעה על שגיאה AlertDialog פעולה אשר מציגה 
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
    }
}