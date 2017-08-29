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

using XFormsGeoBackground.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(XFormsGeoBackground.Droid.Services.GPSBackgroundDependencyService))]
namespace XFormsGeoBackground.Droid.Services
{
    public class GPSBackgroundDependencyService : IGPSBackgroundService
    {
        public static GPSBackgroundDependencyService Self;

        public event EventHandler ServiceStartedEvent;
        Intent serviceIntent;

        public void Start()
        {
            Self = this;
            serviceIntent = new Intent(Forms.Context, typeof(GPSBackgroundService));
            Forms.Context.StartService(serviceIntent);
        }

        internal void Started()
        {
            ServiceStartedEvent(this, null);
        }

        public void Stop()
        {
            Forms.Context.StopService(serviceIntent);
        }
    }

}