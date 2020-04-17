using PeerCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnverifiedMemberPage : ContentPage
    {
        public UnverifiedMemberPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            InitializeComponent();
            GetSubs();
            LblName.Text = HelperAppSettings.Name;
        }
        public async void GetSubs()

        {
            indicator.IsRunning = true;
            indicator.IsVisible = true;


            HttpClient client = new HttpClient();
            var dashboardEndpoint = Helper.GetPlansUrl;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(dashboardEndpoint);
            var PlanList = JsonConvert.DeserializeObject<PlansListModel>(result);

            UnvPlanName.BindingContext = PlanList.plans[0];
            UnvPlanDesc.BindingContext = PlanList.plans[0];



            indicator.IsRunning = false;
            indicator.IsVisible = false;
        }

        async void ReadMoreClicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new AboutPage());
        }

        public void SignOutClicked(object sender, EventArgs e)
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

        public async void changePassClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new changePassword());
        }

        public async void SharingClicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = "Community Name:" + " " + HelperAppSettings.community_name + "," + " " + "Community Code:" + " " + HelperAppSettings.community_code,
                Title = "Share Community Code!!!"
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await DisplayAlert("Hello", "Your account has not been verified yet! You'll be blocked from any activities until the Admin verifies your account", "Ok");
        }
    }
}