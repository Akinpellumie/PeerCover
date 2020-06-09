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
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SinglePlan : ContentPage
    {
        public static string policyNo;
        string subscription_id;

        public static string policy_number { get; set; }
        public SinglePlan(string subscriptionId)
        {
            subscription_id = subscriptionId;
            InitializeComponent();
            LoadSinglePlan(subscription_id);
            CheckInternet();
        }

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }

        public async void LoadSinglePlan(string subscription_id)
        {
            await PopupNavigation.Instance.PushAsync(new PopLoader());
            var url = Helper.NewPlanUrl + subscription_id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(url);
            var UsersList = JsonConvert.DeserializeObject<ActiveSubModel>(result);

            SinglePlanDetails.BindingContext = UsersList.subscription[0];
            policyNo = UsersList.subscription[0].policy_number;
            var BtnCheck = UsersList.subscription[0].status;
            var expCheck = UsersList.subscription[0].is_expired;
            await PopupNavigation.Instance.PopAsync(true);

            if (BtnCheck.Contains("Not Paid"))
            {
                PayStc1.IsVisible = false;
                PayStck.IsVisible = true;
                RenewStck.IsVisible = false;
            }
            else if (BtnCheck.Contains("Paid") && expCheck.Contains("0"))
            {
                PayStc1.IsVisible = true;
                PayStck.IsVisible = false;
                RenewStck.IsVisible = false;
            }
            else if (BtnCheck.Contains("Paid") && expCheck.Contains("1"))
            {
                PayStc1.IsVisible = false;
                PayStck.IsVisible = false;
                RenewStck.IsVisible = true;
            }

        }

        async void MakeClaim_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MakeClaim(subscription_id));
        }

        async void PayPremium_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PayPremiums());
        }

        async void Renew_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RenewSub(subscription_id, policyNo));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //LoadSinglePlan(subscription_id);
        }

    }
}