using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
            InitializeComponent();
        }

        public async void getStartedClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "Your account has not been verified yet. Kindly contact the admin for verification!!!", "Ok");
        }
    }
}