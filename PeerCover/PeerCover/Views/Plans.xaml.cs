using Newtonsoft.Json;
using PeerCover.Models;
using System;
using System.Net.Http;

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
        }
        public async void GetSubDetails()
        {
            HttpClient client = new HttpClient();
            var UserCountEndpoint = Helper.getActiveSubUrl + HelperAppSettings.username;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var result = await client.GetStringAsync(UserCountEndpoint);
            var UsersCnt = JsonConvert.DeserializeObject<ActSubModel>(result);

            ActivePlanList.ItemsSource = UsersCnt.subscriptions;

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