using Newtonsoft.Json;
using PeerCover.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClaimsPage : TabbedPage
    {
        public ClaimsPage()
        {
            InitializeComponent();
            GetClaims();
            ClaimList.RefreshCommand = new Command(() => {
                //Do your stuff.    
                GetClaims();
                ClaimList.IsRefreshing = false;
            });
        }
        public async void GetClaims()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }
            indicator.IsRunning = true;
            indicator.IsVisible = true;


            HttpClient client = new HttpClient();
            var dashboardEndpoint = Helper.GetClaimByIdUrl + HelperAppSettings.username;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(dashboardEndpoint);
            var ClaimsList = JsonConvert.DeserializeObject<ClaimsListModel>(result);

            ClaimList.ItemsSource = ClaimsList.claims;
            if (ClaimsList.claims.Count == 0)
            {
                FrmCB.IsVisible = true;
                stackList.IsVisible = false;
            }
            else
            {
                stackList.IsVisible = true;
                FrmCB.IsVisible = false;
            }

            indicator.IsRunning = false;
            indicator.IsVisible = false;
        }

        async void ViewClaimTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedUser = e.Item as Models.ClaimsModel;
            if (selectedUser.status.Contains("Reported"))
            {
                await Shell.Current.Navigation.PushAsync(new SingleClaim(selectedUser.id));
            }
            else
            {
                await Shell.Current.Navigation.PushAsync(new ClaimResponse(selectedUser.id));
            }


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetClaims();
        }
    }
}