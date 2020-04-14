using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordConfirmation : ContentPage
    {
        public PasswordConfirmation()
        {
            InitializeComponent();
        }
        public void PassValidClicked(object sender, EventArgs e)
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