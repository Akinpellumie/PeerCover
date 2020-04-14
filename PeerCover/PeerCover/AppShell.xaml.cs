using PeerCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;

namespace PeerCover
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            //AppShell appShell = new AppShell();
            //AppShell.
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
    }
}
