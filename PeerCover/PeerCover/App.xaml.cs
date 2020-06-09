using Xamarin.Forms;
using PeerCover.Services;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using PeerCover.Views;

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