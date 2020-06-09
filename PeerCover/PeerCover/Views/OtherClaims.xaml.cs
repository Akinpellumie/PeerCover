using PeerCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OtherClaims : ContentPage
    {
        public OtherClaims()
        {
            InitializeComponent();
            GetOtherClaims();
            CheckInternet();
            OtherClaimsList.RefreshCommand = new Command(() => {
                //Do your stuff.    
                GetOtherClaims();
                OtherClaimsList.IsRefreshing = false;
            });
        }

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }


        public async void GetOtherClaims()

        {
            indicator.IsRunning = true;
            indicator.IsVisible = true;


            HttpClient client = new HttpClient();
            var dashboardEndpoint = Helper.getCommClaimsUrl + HelperAppSettings.community_code;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(dashboardEndpoint);
            var ClaimsList = JsonConvert.DeserializeObject<ClaimsListModel>(result);

            OtherClaimsList.ItemsSource = ClaimsList.claims;

             if (ClaimsList.claims.Count == 0)
            {
                FrmSB.IsVisible = true;
                stcList.IsVisible = false;
            }
            else
            {
                stcList.IsVisible = true;
                FrmSB.IsVisible = false;
            }

            indicator.IsRunning = false;
            indicator.IsVisible = false;
        }

        async void ViewClaimTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedUser = e.Item as ClaimsModel;
            await Shell.Current.Navigation.PushAsync(new SingleClaim(selectedUser.id));

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetOtherClaims();
        }
    }
}