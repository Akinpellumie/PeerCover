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
    public partial class TransactionHistory : ContentPage
    {
        public TransactionHistory()
        {
            InitializeComponent();
            GetTransHis();
        }

        public async void GetTransHis()
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
        }

    }
}