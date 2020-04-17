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

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcceptedRequest : ContentPage
    {
        public AcceptedRequest()
        {
            InitializeComponent();
            GetAccRequests();
        }

        public async void GetAccRequests()

        {
            indicator.IsRunning = true;
            indicator.IsVisible = true;

            try
            {
            HttpClient client = new HttpClient();
            var dashboardEndpoint = Helper.GetRequestsUrl + HelperAppSettings.community_code + Helper.getRequestFilter + "accepted";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(dashboardEndpoint);
            var requestsList = JsonConvert.DeserializeObject<AccReqModel>(result);
            AcceptedRequestList.ItemsSource = requestsList.requests;

            if (requestsList.requests.Count == 0)
            {
                AcceptStack.IsVisible = false;
                AccStack.IsVisible = true;
            }
            else
            {
            AcceptStack.IsVisible = true;
            AccStack.IsVisible = false;
            }
            indicator.IsRunning = false;
            indicator.IsVisible = false;
            }
            catch (Exception)
            {
                await DisplayAlert("Oops!", "check your internet connection", "Ok");
                return;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetAccRequests();
        }

        public void AcceptedTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedUser = e.Item as Models.RequestsModel;
            DisplayAlert("Yoo!", selectedUser.firstname + " has been accepted to the community", "Ok");
            //if(selectedUser.status.Contains("A"))
            //PopupNavigation.Instance.PushAsync(new PopUpRequests(selectedUser.request_id));
            //MessagingCenter.Subscribe<Models.RequestsModel>(this, "PopUpData", (value) => { });
            ////await Shell.Current.Navigation.PushAsync(new PopUp(selectedUser.id));

        }
    }
}