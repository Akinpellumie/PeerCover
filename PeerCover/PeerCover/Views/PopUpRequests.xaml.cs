using PeerCover.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpRequests
    {
        string ReqId;
        public PopUpRequests(string request_id)
        {
            ReqId = request_id;
            InitializeComponent();
        }

        public async void AcceptedBtn_Clicked(object sender, EventArgs e)
        {
            indicator.IsRunning = true;
            indicator.IsVisible = true;
            RequestsModel update = new RequestsModel()
            {
                status = "Accepted",
                reviewedBy = HelperAppSettings.username,
                requestId = ReqId,
            };
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var json = JsonConvert.SerializeObject(update);
            HttpContent result = new StringContent(json);
            result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PutAsync(Helper.UpdateRequestUrl, result);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Alert", "Request Accepted Successfully!!!", "Ok");
                await PopupNavigation.Instance.PopAsync(true);
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("Alert", "Whoopss! Try Again Later", "Ok");
                    await PopupNavigation.Instance.PopAsync(true);
                }
                else
                {
                    await DisplayAlert("Alert", "Please try again later", "Ok");
                    await PopupNavigation.Instance.PopAsync(true);
                }
            }
        }

        public async void DeclineBtn_Clicked(object sender, EventArgs e)
        {
            indicator.IsRunning = true;
            indicator.IsVisible = true;
            RequestsModel update = new RequestsModel()
            {
                status = "Declined",
                reviewedBy = HelperAppSettings.username,
                requestId = ReqId,
            };
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var json = JsonConvert.SerializeObject(update);
            HttpContent result = new StringContent(json);
            result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PutAsync(Helper.UpdateRequestUrl, result);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Alert", "Request Declined!!!", "Ok");
                await PopupNavigation.Instance.PopAsync(true);
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("Alert", "Whoopss! Try Again Later", "Ok");
                    await PopupNavigation.Instance.PopAsync(true);
                    indicator.IsVisible = false;
                    indicator.IsRunning = false;
                }
                else
                {
                    await DisplayAlert("Alert", "Please try again later", "Ok");
                    await PopupNavigation.Instance.PopAsync(true);
                    indicator.IsVisible = false;
                    indicator.IsRunning = false;
                }
            }
        }
    }
}