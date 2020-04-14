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
    public partial class SingleClaim : ContentPage
    {
        string nyId;
        public SingleClaim(string id)
        {
            nyId = id;
            InitializeComponent();
            PopupNavigation.Instance.PushAsync(new PopLoader());
            LoadSingleClaim(id);
            PopupNavigation.Instance.PopAsync(true);
        }
        public async void LoadSingleClaim(string id)

        {
            var url = Helper.GetClaimsUrl + id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(url);
            var UsersList = JsonConvert.DeserializeObject<SingleClaimListModel>(result);
            SingleClaimDetails.BindingContext = UsersList.claim[0];
            //SnName.Text = UsersList.claim[0].firstname + UsersList.claim[0].lastname;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await PopupNavigation.Instance.PushAsync(new PopLoader());
            LoadSingleClaim(nyId);
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}