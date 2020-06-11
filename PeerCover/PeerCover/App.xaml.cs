using Xamarin.Forms;
using PeerCover.Services;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using PeerCover.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace PeerCover
{
    public partial class App : Application
    {

        public static Page currentpage { get; private set; }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new SplashPage();
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=3c27309c-31da-42d5-9663-a893747476d0;" +
                  "uwp={Your UWP App secret here};" +
                  "ios={Your iOS App secret here}",
                  typeof(Analytics), typeof(Crashes));
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            CheckInternet();

        }

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }

    }
}