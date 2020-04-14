using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpConfirmation : ContentPage
    {
        public SignUpConfirmation()
        {
            InitializeComponent();
        }
        public void SignUpValidClicked(object sender, EventArgs e)
        {
            ContentPage fpm = new LoginPage();
            HelperAppSettings.Token = "";
            HelperAppSettings.firstname = "";
            HelperAppSettings.lastname = "";
            HelperAppSettings.username = "";
            HelperAppSettings.email = "";
            HelperAppSettings.phonenumber = "";
            HelperAppSettings.community_code = "";
            HelperAppSettings.community_name = "";
            HelperAppSettings.id = "";
            HelperAppSettings.profile_img_url = "";
            HelperAppSettings.priviledges = "";
            HelperAppSettings.capName = "";
            HelperAppSettings.Name = "";
            Application.Current.MainPage = fpm;
        }
    }
}