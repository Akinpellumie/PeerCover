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
    public partial class SingleMemberPage : ContentPage
    {
        string myUser;
        public SingleMemberPage(string id, string username)
        {
            myUser = username;
            InitializeComponent();
            GetSubDetails(username);
            LoadSingleMember(id);
            CheckInternet();
        }

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }

        public async void LoadSingleMember(string id)

        {
            var url = Helper.getMembersUrl + id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(url);
            var UsersList = JsonConvert.DeserializeObject<SinMemberModel>(result);
            SingleMemberDetails.BindingContext = UsersList.member[0];
            var ProImage = UsersList.member[0].profile_img_url;
            //DashMemList.ItemsSource = UsersList.member[0].policyDetails;

            //LblPriv.Text = UsersList.member[0].priviledges.ForEach() 

            if (string.IsNullOrEmpty(ProImage))
            {
                MemberImage.Source = "placeholder.png";
            }
            else
            {
                MemberImage.Source = Helper.ImageUrl + ProImage;
            }

        }

        public async void GetSubDetails(string username)
        {
            HttpClient client = new HttpClient();
            var UserCountEndpoint = Helper.getActiveSubUrl + username;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var result = await client.GetStringAsync(UserCountEndpoint);
            var UsersCnt = JsonConvert.DeserializeObject<ActSubModel>(result);
            DashMemList.ItemsSource = UsersCnt.subscriptions;

            TxtEmp.Text = username + " has no active subscription(s).";

            if (UsersCnt.subscriptions.Count == 0)
            {
                FrmSB.IsVisible = true;
                SubList.IsVisible = false;
            }
            else
            {
                SubList.IsVisible = true;
                FrmSB.IsVisible = false;
            }
        }

        private async void backClicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}