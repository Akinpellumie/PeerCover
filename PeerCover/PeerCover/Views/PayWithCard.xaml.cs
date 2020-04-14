using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayWithCard : ContentPage
    {
        public PayWithCard()
        {
            InitializeComponent();
            PayCard.Source = Helper.CardPayUrl + TransHelper.transactionId;
        }

        private void Payview_Navigating(object sender, WebNavigatingEventArgs e)
        {
            Indic.IsVisible = true;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Indic.ProgressTo(0.9, 900, Easing.SpringIn);
        }

        private void Payview_Navigated(object sender, WebNavigatedEventArgs e)
        {
            Indic.IsVisible = false;
        }

        protected override bool OnBackButtonPressed()
        {
            try
            {
                if (HelperAppSettings.priviledges.Contains("ADMIN"))
                {
                    AppShell fpm = new AppShell();
                    Application.Current.MainPage = fpm;
                }
                else
                {
                    Application.Current.MainPage = new AppShellUser();
                }
                //Shell.Current.Navigation.PopModalAsync();
                return true;
            }
            catch (Exception)
            {
                return true;
            }

        }

    }
}