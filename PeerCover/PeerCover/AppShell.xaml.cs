using PeerCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace PeerCover
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            AppShellModel appShell = new AppShellModel();
            appShell.fullname = "Akinlade Akinpelumi";
            appShell.commName1 = HelperAppSettings.community_name;
            appShell.userName = HelperAppSettings.username;
            BindingContext = appShell;
            if (string.IsNullOrEmpty(HelperAppSettings.profile_img_url))
            {
                appShell.ImageUrl = "placeholder.png";
            }
            else
            {
                var imgUrl = Helper.ImageUrl + HelperAppSettings.profile_img_url;
                appShell.ImageUrl = imgUrl;
            }
            InitializeComponent();

            this.BindingContext = Helper.userprofile;
            GetUserById();
        }

        public async void GetUserById()
        {
            HttpClient client = new HttpClient();
            var UserdetailEndpoint = Helper.getMembersUrl + HelperAppSettings.id;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var result = await client.GetStringAsync(UserdetailEndpoint);
            var MemberDetails = JsonConvert.DeserializeObject<MemberDetailsModel>(result);
            //Users = new ObservableCollection<AddedUsers>(UsersList);
            this.BindingContext = MemberDetails.member;
            
            //ProfileName.BindingContext = MemberDetails.member[0];
            //FlyImage.BindingContext = MemberDetails.member[0];

            if (MemberDetails.member[0].profile_img_url == null)
            {
                this.BindingContext = ImageSource.FromFile("ProfilePic.png");
            }

        }

        protected void onAppearing (object sender, EventArgs e)
        {
            base.OnAppearing();
            GetUserById();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

        //    protected override bool OnBackButtonPressed()
        //    {
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        var result = await this.DisplayAlert("Alert!", "Are you sure you want to exit this App?", "Yes", "No");
        //        if (result==true)
        //        {
        //            base.OnBackButtonPressed();
        //            Application.Current.Quit();
        //        }
        //    });
        //    return true;
        //    }

        //public interface IAndroidMethods
        //{
        //    void Quit();
        //}
    }
 }
