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
    public partial class Plans : TabbedPage
    {
        public Plans()
        {
            InitializeComponent();
            GetSubDetails();
            CheckInternet();
            ActivePlanList.RefreshCommand = new Command(() => {
                //Do your stuff.    
                GetSubDetails();
                ActivePlanList.IsRefreshing = false;
            });
        }

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }

        public async void GetSubDetails()
        {
            indicator.IsRunning = true;
            indicator.IsVisible = true;


            HttpClient client = new HttpClient();
            var UserCountEndpoint = Helper.getActiveSubUrl + HelperAppSettings.username + "&isActive=1";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var result = await client.GetStringAsync(UserCountEndpoint);
            var UsersCnt = JsonConvert.DeserializeObject<ActSubModel>(result);

            ActivePlanList.ItemsSource = UsersCnt.subscriptions;

            if (UsersCnt.subscriptions.Count == 0)
            {
                FrmPLB.IsVisible = true;
                PlanActList.IsVisible = false;
            }
            else
            {
                PlanActList.IsVisible = true;
                FrmPLB.IsVisible = false;
            }
            indicator.IsRunning = false;
            indicator.IsVisible = false;
        }

        public async void ViewSubTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedUser = e.Item as Models.SubscriptionModel;
            await Shell.Current.Navigation.PushAsync(new SinglePlan(selectedUser.subscription_id));

        }
        async void ReadMoreClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPlan());
        }
    }
}