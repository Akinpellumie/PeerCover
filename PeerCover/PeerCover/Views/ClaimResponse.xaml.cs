using PeerCover.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClaimResponse : ContentPage
    {
        string ClmId;
        //string id;
        public ClaimResponse(string id)
        {
            ClmId = id;
            InitializeComponent();
            LoadSingleClaim(id);
        }
        public async void LoadSingleClaim(string id)

        {
            var url = Helper.GetClaimsUrl + id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(url);
            var UsersList = JsonConvert.DeserializeObject<SingleClaimListModel>(result);
            ClaimResDetails.BindingContext = UsersList.claim[0];

            var ActSection = UsersList.claim[0].status;

            if (ActSection.Contains("Recommendation Accepted") || ActSection.Contains("Declined"))
            {
                ActionSect.IsVisible = false;
            }
        }

        public void DeclinePopClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new PopUpDecline(ClmId));
            MessagingCenter.Subscribe<Models.ClaimsModel>(this, "PopUpData", (value) => { });
        }

        private async void AcceptClaimClicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }
            indicator.IsVisible = true;
            indicator.IsRunning = true;

            AcceptAmtModel update = new AcceptAmtModel()
            {
                claimOwnerUsername = HelperAppSettings.username,
                status = "Recommendation Accepted",
                claimId = ClmId,
            };
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var json = JsonConvert.SerializeObject(update);
            HttpContent result = new StringContent(json);
            result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PutAsync(Helper.GetClaimsUrl, result);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success!!!", "Recommendation Accepted", "Ok");
                await Shell.Current.Navigation.PopModalAsync();
                //Application.Current.MainPage = new Dashboard();
                indicator.IsVisible = false;
                indicator.IsRunning = false;

            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("Whoops!!!", "Server error. Please try again later." , "Ok");
                    indicator.IsVisible = false;
                    indicator.IsRunning = false;
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await DisplayAlert("Oops!", "Session Timeout. Kindly Login and try again." , "Ok");
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                }
                else
                {
                    indicator.IsRunning = false;
                    indicator.IsVisible = false;
                    await DisplayAlert("Oops!", "Please try again later", "Ok");

                }
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            string id = ClmId;
            LoadSingleClaim(id);
        }
    }
}