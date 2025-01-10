using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App_YumYum_YairLeitus
{
    public class UserAdapter : BaseAdapter<User>
    {
        // האקטיביטי המשוייך לאדפטר
        private Activity activity;

        // רשימת המשתמשים של האדפטר
        private List<User> users;

        public UserAdapter(Activity activity, List<User> users)
        {
            this.activity = activity;
            this.users = users;
        }

        public void SetUsers(List<User> users)
        {
            this.users = users;
        }

        public List<User> GetUserList()
        {
            return users;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return users.Count; }
        }

        public override User this[int position]
        {
            get { return users[position]; }
        }

        /* הפעולה יודעת לרוץ כמספר האיברים ברשימה
         כל פעם היא מקבלת את המיקום המתאים */
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // activity ניפוח הרשומה בדף המשתמשים לפי
            LayoutInflater layoutInflater = activity.LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.userItem_layout, parent, false);

            // וקישורם view-הפרטים המופיעים ב
            TextView tvUsername = view.FindViewById<TextView>(Resource.Id.userItem_tvUsername);
            TextView tvPassword = view.FindViewById<TextView>(Resource.Id.userItem_tvPassword);
            TextView tvName = view.FindViewById<TextView>(Resource.Id.userItem_tvName);
            TextView tvEmail = view.FindViewById<TextView>(Resource.Id.userItem_tvEmail);

            // הכנסת הפרטים לפי פרטי המשתמש במיקום המתאים 
            tvUsername.Text = users[position].username;
            tvPassword.Text = users[position].password;
            tvName.Text = users[position].name;
            tvEmail.Text = users[position].email;

            // צבע התא של מנהל יהיה בגוון אדום
            if (users[position].isAdmin)
            {
                view.SetBackgroundColor(Color.ParseColor("#fffd0d29"));
            }
            
            return view;
        }
    }
}