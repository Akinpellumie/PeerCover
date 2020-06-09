using PeerCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using System.Linq;
using PeerCover.GroupHelper;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageMembers : ContentPage
    {
        //public static ObservableCollection<Grouping<string, MembersListModel>> MembersGrouped { get; set; }

        public static ObservableCollection<MembersListModel> Members { get; set; }
        public static ObservableCollection<MembersListModel> AllMembers { get; set; }
        private ObservableCollection<GroupedMembersModel> grouped { get; set; }

        public ManageMembers()
        {
            InitializeComponent();
            GetMembers();
            CheckInternet();
            MemberList.RefreshCommand = new Command(() => {
                //Do your stuff.    
                GetMembers();
                MemberList.IsRefreshing = false;
            });
        }

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
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
            var sorted = from member in MemList.members
                         orderby member.firstname
                         group member by member.NameSort into memberGroup
                         select new Grouping<string, MembersModel>(memberGroup.Key, memberGroup);
            var groupedMembers = new ObservableCollection<Grouping<string, MembersModel>>(sorted);


            stack2.IsVisible = false;
            stack1.IsVisible = true;
            MemberList.ItemsSource = groupedMembers;
            MemberList.IsGroupingEnabled = true;
            MemberList.GroupDisplayBinding = new Binding("Key");
            MemberList.GroupShortNameBinding = new Binding("Key");

            //grouped = new ObservableCollection<GroupedMembersModel>();
            //var veggieGroup = new GroupedMembersModel() { LongName = , ShortName = "v" };

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
                    stack2.IsVisible = true;
                    stack1.IsVisible = false;
                    SearchMemList.ItemsSource = UsersList.members;
                }
                else if (string.IsNullOrEmpty(UsersList.members[0].firstname))
                {
                    stack2.IsVisible = false;
                    stack1.IsVisible = false;
                    emptysearch.IsVisible = true;
                    await DisplayAlert("Search", "No Record Found", "Ok");
                }
            }

            else if (string.IsNullOrEmpty(e.NewTextValue))
            {
                stack2.IsVisible = false;
                stack1.IsVisible = true;
                emptysearch.IsVisible = false;
                GetMembers();
            
            }

        }

        public async void ViewMemberTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedUser = e.Item as MembersModel;
            await Shell.Current.Navigation.PushAsync(new SingleMemberPage(selectedUser.id, selectedUser.username));
            
        }
        
        protected void PageRefreshing(object sender, EventArgs e)
        {
            GetMembers();
            MemberList.EndRefresh();
        }
    }
}