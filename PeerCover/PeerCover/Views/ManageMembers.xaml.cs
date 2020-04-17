using PeerCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageMembers : ContentPage
    {
        //public static ObservableCollection<Grouping<string, MembersListModel>> MembersGrouped { get; set; }

        public static ObservableCollection<MembersListModel> Members { get; set; }
        public static ObservableCollection<MembersListModel> AllMembers { get; set; }

        public ManageMembers()
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

            //MemberList.BindingContext = MembersGrouped;
            //var sorted = from member in Members
            //             orderby member.members[0].firstname
            //             group member by member.members[0].NameSort into memberGroup
            //             select new Grouping<string, MembersListModel>(memberGroup.Key, memberGroup);

            ////create a new collection of groups
            //MembersGrouped = new ObservableCollection<Grouping<string, MembersListModel>>(sorted);

            //MemberList.ItemsSource = MembersGrouped;
            //MemberList.IsGroupingEnabled = true;
            //MemberList.GroupDisplayBinding = new Binding("Key");

            //var ProfileImage = MemList.members[0].profile_img_url;

            //if (ProfileImage != null)
            //{
            //    MmImage.Source = Helper.ImageUrl + ProfileImage;
            //}
            //else
            //{
            //    MmImage.Source = "placeholder.png";
            //}


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

                    MemberList.ItemsSource = UsersList.members;
                }
                else if (UsersList == null)
                {
                    await DisplayAlert("Search", "No Record Found", "Ok");
                }
            }

            else if (string.IsNullOrEmpty(e.NewTextValue))
            {
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