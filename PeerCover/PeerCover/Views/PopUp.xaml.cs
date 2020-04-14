using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PeerCover.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUp
    {
        string UserName;
        string MemId;
        public PopUp(string username, string id)
        {
            UserName = username;
            MemId = id;
            InitializeComponent();
        }

        public async void AdminBtn_Clicked(object sender, EventArgs e)
        {
            User update = new User()
            {
                username = UserName,
            };
            var t = new List<string>
            {
                "ADMIN",
            };
            update.priviledges = t;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var json = JsonConvert.SerializeObject(update);
            HttpContent result = new StringContent(json);
            result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(Helper.UpdateRolesUrl, result);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Alert", "Successful!!!", "Ok");
                await PopupNavigation.Instance.PopAsync(true);
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("InHub", response.ReasonPhrase, "Ok");
                    await PopupNavigation.Instance.PopAsync(true);
                }
                else
                {
                    await DisplayAlert("Kayar", "Please try again later", "Ok");
                    await PopupNavigation.Instance.PopAsync(true);
                }
            }
        }

        public async void LossBtn_Clicked(object sender, EventArgs e)
        {
            User update = new User()
            {
                username = UserName,
            };
            var t = new List<string>
            {
                "LOSS ASSESSOR",
            };
            update.priviledges = t;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var json = JsonConvert.SerializeObject(update);
            HttpContent result = new StringContent(json);
            result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(Helper.UpdateRolesUrl, result);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Alert", "Successful!!!", "Ok");
                await PopupNavigation.Instance.PopAsync(true);

            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("Alert", response.ReasonPhrase, "Ok");
                    await PopupNavigation.Instance.PopAsync(true);
                }
                else
                {
                    await DisplayAlert("Alert", "Please try again later", "Ok");
                    await PopupNavigation.Instance.PopAsync(true);
                }
            }
        }

        async void ViewMemClicked(object sender, EventArgs e)
        {

            await Shell.Current.Navigation.PushModalAsync(new SingleMemberPage(MemId, UserName));

        }
    }
}