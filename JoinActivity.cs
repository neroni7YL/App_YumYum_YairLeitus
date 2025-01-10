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
    [Activity(Label = "JoinActivity")]
    public class JoinActivity : Activity
    {
        // כפתור מעבר לדף ההתחברות
        private Button btnGotoLogin;

        // כפתור מעבר לדף ההרשמה
        private Button btnGotoSignup;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.join_layout);

            btnGotoLogin = FindViewById<Button>(Resource.Id.join_btnGotoLogin);
            btnGotoSignup = FindViewById<Button>(Resource.Id.join_btnGotoSignup);

            btnGotoLogin.Click += BtnGotoLogin_Click;
            btnGotoSignup.Click += BtnGotoSignup_Click;
        }

        // מעבר לדף ההתחברות בלחיצה על הכפתור
        private void BtnGotoLogin_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
            Finish();
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