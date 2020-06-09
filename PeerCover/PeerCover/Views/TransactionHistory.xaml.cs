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
    public partial class TransactionHistory : ContentPage
    {
        public TransactionHistory()
        {
            InitializeComponent();
            GetTransHis();
            CheckInternet();
            TransList.RefreshCommand = new Command(() => {
                //Do your stuff.    
                GetTransHis();
                TransList.IsRefreshing = false;
            });
        }

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }

        public async void GetTransHis()
        {
            try
            {
            await PopupNavigation.Instance.PushAsync(new PopLoader());
            HttpClient client = new HttpClient();
            var dashboardEndpoint = Helper.TransactionUrl + HelperAppSettings.username;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(dashboardEndpoint);
            var TnxList = JsonConvert.DeserializeObject<TransModel>(result);
            TransList.ItemsSource = TnxList.transactions;

            await PopupNavigation.Instance.PopAsync(true);
                if (TnxList.transactions.Count == 0)
                {
                    FrmTB.IsVisible = true;
                    TranStack.IsVisible = false;
                }
                else
                {
                    TranStack.IsVisible = true;
                    FrmTB.IsVisible = false;
                }

            }
            catch (Exception)
            {
                await DisplayAlert("Oops!", "check yoour internet connection", "Ok");
                return;
            }
        }

    }
}