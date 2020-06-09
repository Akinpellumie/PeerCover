using PeerCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        string appToken;
        string ProfileImage;

        public ObservableCollection<PlansListModel> plan_id { get; set; }
        public Dashboard()
        {
            InitializeComponent();
            CheckInternet();
            Permission();
            //GetSubs();
            GetSubDetails();
            LblName.Text = HelperAppSettings.Name;

        }
        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }
        async void Permission()
        {
            await Permissions.RequestAsync<Permissions.Camera>();
            await Permissions.RequestAsync<Permissions.StorageRead>();
            await Permissions.RequestAsync<Permissions.StorageWrite>();
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            await Permissions.RequestAsync<Permissions.LocationAlways>();
            await Permissions.RequestAsync<Permissions.NetworkState>();
        }
        //public async void GetSubs()

        //{
        //    indicator.IsRunning = true;
        //    indicator.IsVisible = true;


        //    HttpClient client = new HttpClient();
        //    var dashboardEndpoint = Helper.GetPlansUrl;
        //    client.DefaultRequestHeaders.Clear();
        //    client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
        //    var result = await client.GetStringAsync(dashboardEndpoint);
        //    var PlanList = JsonConvert.DeserializeObject<PlansListModel>(result);

        //    LblPlanName.BindingContext = PlanList.plans[0];
        //    LblPlanDesc.BindingContext = PlanList.plans[0];



        //    indicator.IsRunning = false;
        //    indicator.IsVisible = false;
        //}

        public async void GetSubDetails()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }

            HttpClient client = new HttpClient();
            var UserCountEndpoint = Helper.getActiveSubUrl + HelperAppSettings.username;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var result = await client.GetStringAsync(UserCountEndpoint);
            var UsersCnt = JsonConvert.DeserializeObject<ActSubModel>(result);
            //Users = new ObservableCollection<AddedUsers>(UsersList);
            //var hut = UsersCnt.subscriptions;
            DashList.ItemsSource = UsersCnt.subscriptions;

        }

        //async void ViewPlanTapped(object sender, ItemTappedEventArgs e)
        //{
        //    if (e.Item == null) return;
        //    var selectedItem = e.Item as Asset;
        //    await Shell.Current.Navigation.PushModalAsync(new EditAsset(selectedItem.item_id));

        //}

        public async void ViewSubTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedUser = e.Item as SubscriptionModel;
            await Shell.Current.Navigation.PushAsync(new SinglePlan(selectedUser.subscription_id));

        }

        public async void ReadMoreClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPlan());
        }

        public async void Sub_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPlan());
        }

        public async void ProDash_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile());
        }

        public async void MemDash_Clicked(object sender, EventArgs e)
        {
            if (HelperAppSettings.priviledges.Contains("ADMIN"))
            {
                await Navigation.PushAsync(new ManageMembers());
            }
            else
            {
                await Navigation.PushAsync(new UserMemberPage());
            }
        }

        public async void Handle_FabClicked(object sender, EventArgs e)
        {
            //await ((Frame)sender).ScaleTo(0.8, length: 50, Easing.Linear);
            //await Task.Delay(100);
            //await ((Frame)sender).ScaleTo(1, length: 50, Easing.Linear);
            await Navigation.PushAsync(new NewPlan());
        }

        public async void changePassClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new changePassword());
        }

        public async void SharingClicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = "Community Name:" + " " + HelperAppSettings.community_name + "," + " " + "Community Code:" + " " + HelperAppSettings.community_code,
                Title = "Share Community Code!!!"
            });
        }
        public async void NotifyIconClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationPage());
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            GetUserById();
            GetSubDetails();
            try
            {
                appToken = await SecureStorage.GetAsync("refreshedToken");
            }
            catch (Exception)
            {
                return;
            }


            if (!string.IsNullOrEmpty(appToken))
            {
                User update = new User()
                {
                    username = HelperAppSettings.username,
                    registrationToken = appToken
                };
                var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                var json = JsonConvert.SerializeObject(update);
                HttpContent result = new StringContent(json);
                result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PutAsync(Helper.MemRegToken, result);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("User App Registration Token sent successfully!");
                }
            }
        }

        public async void SignOutClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Hey!" , "Are you sure you want to sign out?", "Yes", "No");
            if (result == true)
            {
                ContentPage fpm = new LoginPage();
                HelperAppSettings.Token = "";
                HelperAppSettings.firstname = "";
                HelperAppSettings.lastname = "";
                HelperAppSettings.username = "";
                HelperAppSettings.email = "";
                HelperAppSettings.phonenumber = "";
                HelperAppSettings.community_code = "";
                HelperAppSettings.community_name = "";
                HelperAppSettings.id = "";
                HelperAppSettings.profile_img_url = "";
                HelperAppSettings.priviledges = "";
                HelperAppSettings.capName = "";
                HelperAppSettings.Name = "";
                Application.Current.MainPage = fpm;
            }
            else
            {
                return;
            }
            
        }

        public void menuHider_clicked(object sender, EventArgs e)
        {
            if (menuHide.IsVisible == true)
            {
                menuHide.IsVisible = false;
                Frm1.IsVisible = false;
                Frm1b.IsVisible = true;
                chevUp.Source = "chevronUp.png";
            }
            else if (menuHide.IsVisible == false)
            {
                Frm1.IsVisible = true;
                Frm1b.IsVisible = false;
                chevUp.Source = "chevronDown.png";
                menuHide.IsVisible = true;
            }
        }

        public void SubHider_clicked(object sender, EventArgs e)
        {
            if (actSubHide.IsVisible == true)
            {
                actSubHide.IsVisible = false;
                Frm2.IsVisible = false;
                Frm2b.IsVisible = true;
                chvUp.Source = "chevronUp.png";
            }
            else if (actSubHide.IsVisible == false)
            {
                actSubHide.IsVisible = true;
                Frm2.IsVisible = true;
                Frm2b.IsVisible = false;
                chvUp.Source = "chevronDown.png";
            }

        }

        public async void GetUserById()
        {
            HttpClient client = new HttpClient();
            var UserdetailEndpoint = Helper.getMembersUrl + HelperAppSettings.id;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var result = await client.GetStringAsync(UserdetailEndpoint);
            var MemberDetails = JsonConvert.DeserializeObject<MemberDetailsModel>(result);
            
            ProfileImage = MemberDetails.member[0].profile_img_url;
            
            if (string.IsNullOrEmpty(ProfileImage))
            {
                FlyOutImage.Source = "placeholder.png";
            }
            else
            {
                var imgUrl = Helper.ImageUrl + ProfileImage;
                FlyOutImage.Source = imgUrl;
            }
        }
     }
}