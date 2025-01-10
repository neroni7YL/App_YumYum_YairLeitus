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
    // BroadCastReceiver -רישום אוטומטי לקובץ המניפסט כ
    [BroadcastReceiver(Label = "BatteryBroadcastReceiver")]
    // ממנו נתוני הסוללה מואזנים כאשר יש שינוי בסוללה IntentFilter הצהרה על 
    [IntentFilter(new[] { Intent.ActionBatteryChanged })]
    public class BatteryBroadcastReceiver : BroadcastReceiver
    {
        //המצוי בדף הבית המראה את כמות הסוללה TextView פקד
        private TextView tvBattery;

        // מחייב פעולה בונה ברירת מחדל בגלל הרישום האוטומטי למניפסט BroadcastReceiver
        public BatteryBroadcastReceiver()
        {

        }

        // מדף הבית TextView פעולה בונה המקבלת 
        public BatteryBroadcastReceiver(TextView tvBattery)
        {
            this.tvBattery = tvBattery;
        }

        // מתעדכן BroadcastReceiver-פעולה המופעלת כאשר ה
        public override void OnReceive(Context context, Intent intent)
        {
            // קבלת מידע על אחוז הסוללה בכל רגע שאחוז הסוללה משתנה
            int battery = intent.GetIntExtra("level", 0);

            // TextView -הצגת אחוז הסוללה ב
            tvBattery.Text = "Battery: " + battery + "%";
            if (battery < 15)
            {
                // בדף הבית במידה וסוללת המכשיר נמוכה מ15% Toast הצגת הודעת 
                Toast.MakeText(context, "Battery is low - " + battery + "%, charge your phone in order to continue viewing our wide variety of recipes!", ToastLength.Long).Show();
            }
        }
    }
}