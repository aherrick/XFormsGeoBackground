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

namespace XFormsGeoBackground.Droid.Services
{
    [Service]
    public class GPSBackgroundService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        // https://stackoverflow.com/questions/30213726/pass-data-from-android-service-to-contentpage-in-xamarin-form-based-application

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            var builder = new Android.Support.V4.App.NotificationCompat.Builder(this);

            var newIntent = new Intent(this, typeof(MainActivity));
            newIntent.PutExtra("tracking", true);
            newIntent.AddFlags(ActivityFlags.ClearTop);
            newIntent.AddFlags(ActivityFlags.SingleTop);

            var pendingIntent = PendingIntent.GetActivity(this, 0, newIntent, 0);
            var notification = builder.SetContentIntent(pendingIntent)
               // .SetSmallIcon(Resource.Drawable.ic_)
                .SetAutoCancel(false)
                .SetTicker("App is recording.")
                .SetContentTitle("App")
                .SetContentText("App is recording your current trip.")
                .Build();

            StartForeground((int)NotificationFlags.ForegroundService, notification);

            DoWork();

            return StartCommandResult.Sticky;
        }

        public void DoWork()
        {
            if (GPSBackgroundDependencyService.Self != null)
            {
                GPSBackgroundDependencyService.Self.Started();
            }      
        }
    }
}