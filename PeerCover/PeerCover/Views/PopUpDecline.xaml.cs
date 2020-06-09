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

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpDecline
    {
        string declineId;
        public PopUpDecline(string id)
        {
            declineId = id;
            InitializeComponent();
        }

        private async void DeclineClmClicked(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;

            DeclineAmtModel update = new DeclineAmtModel()
            {
                claimOwnerUsername = HelperAppSettings.username,
                status = "Recommendation Rejected",
                claimId = declineId,
                rejectionMessage = RejInput.Text,
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
                await DisplayAlert("Success!!!", "Your claim's review has been forwarded and will be attended to in a shortwhile.", "Ok");
                await PopupNavigation.Instance.PopAsync(true);
                await Shell.Current.Navigation.PushAsync(new ClaimsPage());
                indicator.IsVisible = false;
                indicator.IsRunning = false;

            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("PeerCover", "Server error. Please try again later" , "Ok");
                    indicator.IsVisible = false;
                    indicator.IsRunning = false;
                }
                else if(response.StatusCode== System.Net.HttpStatusCode.Unauthorized)
                {
                    await DisplayAlert("Oops!", "Session timeout. Please login again.", "Ok");
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                }
                else
                {
                    indicator.IsRunning = false;
                    indicator.IsVisible = false;
                    await DisplayAlert("Whoops!", "Network error. Please try again later", "Ok");

                }
            }
        }
    }
}