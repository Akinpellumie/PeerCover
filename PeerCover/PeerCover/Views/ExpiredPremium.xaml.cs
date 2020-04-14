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

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpiredPremium : ContentPage
    {
        public ExpiredPremium()
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
            //Users = new ObservableCollection<AddedUsers>(UsersList);
            //var hut = UsersCnt.subscriptions;
            expPreList.ItemsSource = UsersCnt.subscriptions;

        }


        public async void ViewSubTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedUser = e.Item as Models.SubscriptionModel;
            await Shell.Current.Navigation.PushAsync(new SinglePlan(selectedUser.subscription_id));

        }
    }
}