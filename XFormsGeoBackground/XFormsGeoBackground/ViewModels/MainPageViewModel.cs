using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFormsGeoBackground.Interfaces;
using XFormsGeoBackground.Utils;

namespace XFormsGeoBackground.ViewModels
{
    public class MainPageViewModel
    {
        IGPSBackgroundService AndroidGPSBackgroundService { get; }

        public MainPageViewModel()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                AndroidGPSBackgroundService = DependencyService.Get<IGPSBackgroundService>();
            }
        }

        #region Start Tracking

        private Command startTrackingTripCommand;
        public Command StartTrackingTripCommand
        {
            get
            {
                return startTrackingTripCommand ??
                (startTrackingTripCommand = new Command(async () =>
                {
                    await ExecuteStartTrackingTripCommandAsync();
                }));
            }
        }

        public async Task ExecuteStartTrackingTripCommandAsync()
        {
            if (CrossGeolocator.Current.IsListening)
            {
                await App.Current.MainPage.DisplayAlert("Message", "Location tracking already occuring.", "Ok");
                return;
            }

            var hasPermission = await AppUtils.CheckPermissions(Permission.Location);
            if (!hasPermission)
            {
                return;
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                // handle android specially via background service
                AndroidGPSBackgroundService.ServiceStartedEvent += async (sender, e) =>
                {
                    await InitGps();
                };
            }
            else
            {
                await InitGps();
            }
        }

        #endregion

        #region Stop Tracking

        private Command stopTrackingTripCommand;
        public Command StopTrackingTripCommand
        {
            get
            {
                return stopTrackingTripCommand ??
                (stopTrackingTripCommand = new Command(async () =>
                {
                    await ExecuteStopTrackingTripCommandAsync();
                }));
            }
        }

        public async Task ExecuteStopTrackingTripCommandAsync()
        {
            if (!CrossGeolocator.Current.IsListening)
            {
                return;
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                AndroidGPSBackgroundService.Stop();
            }

            CrossGeolocator.Current.PositionChanged -= Geolocator_PositionChanged;
            await CrossGeolocator.Current.StopListeningAsync();
        }

        #endregion

        #region Private Methods

        async Task InitGps()
        {
            CrossGeolocator.Current.PositionChanged += Geolocator_PositionChanged;

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(30), 350, false, new ListenerSettings
            {
                ActivityType = ActivityType.AutomotiveNavigation,
                AllowBackgroundUpdates = true,
                ListenForSignificantChanges = false,
                PauseLocationUpdatesAutomatically = false
            });
        }

        void Geolocator_PositionChanged(object sender, PositionEventArgs e)
        {
            var position = e.Position;

            // do something awesome with your new position!
        }

        #endregion
    }
}
