
using PeerCover.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserMemberPage : ContentPage
    {
        public UserMemberPage()
        {
            InitializeComponent();
            GetMembers();
        }
        public async void GetMembers()

        {
            indicator.IsRunning = true;
            indicator.IsVisible = true;


            HttpClient client = new HttpClient();
            var dashboardEndpoint = Helper.getCommunityMembers + HelperAppSettings.community_code + "&isActive=True";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(dashboardEndpoint);
            var MemList = JsonConvert.DeserializeObject<MembersListModel>(result);
            MemberList.ItemsSource = MemList.members;
            //LblRole.BindingContext = MemList.members[0];

            indicator.IsRunning = false;
            indicator.IsVisible = false;
        }

        public async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= 2)
            {


                if (Helper.getMembersUrl == null)
                {
                    indicator.IsRunning = true;
                    indicator.IsVisible = true;
                    // Autocomplete.IsEnabled = false;
                }

                var url = Helper.getCommunityMembers + HelperAppSettings.community_code + "&name=" + e.NewTextValue;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
                var result = await client.GetStringAsync(url);
                var UsersList = JsonConvert.DeserializeObject<MembersListModel>(result);


                if (UsersList != null)
                {
                    indicator.IsRunning = false;
                    indicator.IsVisible = false;

                    //Itemsearch.IsVisible = true;
                    //BindingContext = ItemsList;
                    MemberList.ItemsSource = UsersList.members;
                    //Itemsearch.ItemsSource = ItemsList;
                    // Autocomplete.IsEnabled = true;
                }
                else if (UsersList == null)
                {
                    await DisplayAlert("Search", "No Record Found", "Ok");
                }
            }

            else if (string.IsNullOrEmpty(e.NewTextValue))
            {
                //acindicator.IsRunning = true;
                //acindicator.IsVisible = true;
                GetMembers();
                //acindicator.IsRunning = false;
                //acindicator.IsVisible = false;


            }

        }

        public async void ViewMemberTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedUser = e.Item as Models.MembersModel; ;
            await Shell.Current.Navigation.PushAsync(new SingleMemberPage(selectedUser.id, selectedUser.username));

        }

        protected void PageRefreshing(object sender, EventArgs e)
        {
            GetMembers();
            MemberList.EndRefresh();
        }
    }
}