using PeerCover.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeclinedRequest : ContentPage
    {
        public DeclinedRequest()
        {
            InitializeComponent();
            GetDecRequests();
            DeclinedRequestList.RefreshCommand = new Command(() => {
                //Do your stuff.    
                GetDecRequests();
                DeclinedRequestList.IsRefreshing = false;
            });
        }

        public async void GetDecRequests()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }

            indicator.IsRunning = true;
            indicator.IsVisible = true;

            try
            {
            HttpClient client = new HttpClient();
            var dashboardEndpoint = Helper.GetRequestsUrl + HelperAppSettings.community_code + Helper.getRequestFilter + "declined";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(dashboardEndpoint);
            var requestsList = JsonConvert.DeserializeObject<DecReqModel>(result);
            DeclinedRequestList.ItemsSource = requestsList.requests;

            if (requestsList.requests.Count == 0)
            {
                emptyStack.IsVisible = true;
                ListStack.IsVisible = false;
            }
            else
            {
            emptyStack.IsVisible = false;
            ListStack.IsVisible = true;
            }
            indicator.IsRunning = false;
            indicator.IsVisible = false;
            }
            catch(Exception)
            {
                await DisplayAlert("Oops!","check your internet connection","Ok");
                return;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public void DeclinedTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedUser = e.Item as RequestsModel;
            DisplayAlert("Yoo!", selectedUser.firstname + " request has been declined", "Ok");

        }
    }
}