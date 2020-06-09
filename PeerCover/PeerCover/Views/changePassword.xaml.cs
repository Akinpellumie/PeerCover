using PeerCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class changePassword : ContentPage
    {
        public changePassword()
        {
            InitializeComponent();
        }

        private async void ChangePassClicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }

            if (string.IsNullOrEmpty(OldPasswordInput.Text) || string.IsNullOrEmpty(NewPasswordInput.Text))
            {
                await DisplayAlert("Alert", "Entry cannot be empty", "ok");
                return;
            }
            await PopupNavigation.Instance.PushAsync(new PopLoader());
            User update = new User()
            {
                username = HelperAppSettings.username,
                oldpassword = OldPasswordInput.Text,
                newpassword = NewPasswordInput.Text,
            };
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var json = JsonConvert.SerializeObject(update);
            HttpContent result = new StringContent(json);
            result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(Helper.changePswdUrl, result);

            if (response.IsSuccessStatusCode)
            {
                await PopupNavigation.Instance.PopAsync(true);
                await DisplayAlert("Successful", "Kindly Login Again", "Ok");
                ContentPage fpm = new LoginPage();
                OldPasswordInput.Text = "";
                NewPasswordInput.Text = "";
                Application.Current.MainPage = fpm;
                indicator.IsVisible = false;
                indicator.IsRunning = false;

            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    await DisplayAlert("Whoops!", "Server error. Please try again later", "Ok");
                    indicator.IsVisible = false;
                    indicator.IsRunning = false;
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    await DisplayAlert("Oops!" , "Session timeout. Please login again!" , "Ok");
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                }
                else
                {
                    indicator.IsRunning = false;
                    indicator.IsVisible = false;
                    await DisplayAlert("Whoops!", "Please try again later", "Ok");

                }
            }
        }
    }
}