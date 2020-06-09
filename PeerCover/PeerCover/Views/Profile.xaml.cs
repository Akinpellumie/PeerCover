using PeerCover.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class Profile : ContentPage
    {
        public string ComName
        {
            get
            {
                return HelperAppSettings.community_code;
            }
            set
            {
                CommName.Text = HelperAppSettings.community_code;
            }
        }
        public Profile()
        {
            InitializeComponent();
            GetUserById();
            CheckInternet();
            //PageName.BindingContext = HelperAppSettings.capName;
            //UserImagePro.BindingContext = HelperAppSettings.profile_img_url;
        }

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
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

            PageName.BindingContext = MemberDetails.member[0];
            UserName.BindingContext = MemberDetails.member[0];
            emailTxt.Text = MemberDetails.member[0].email;
            dateJoined.Text = "Joined on" + " " + MemberDetails.member[0].Pr_date;
            CommName.Text = MemberDetails.member[0].community_name;
            var ProfileImage = MemberDetails.member[0].profile_img_url;

            if (string.IsNullOrEmpty(ProfileImage))
            {
                UserImagePro.Source = "placeholder.png";
                HeaderImg.Source = "placeholder.png";
            }
            else
            {
                UserImagePro.Source = Helper.ImageUrl + ProfileImage;
                HeaderImg.Source = Helper.ImageUrl + ProfileImage;
            }

        }

        async void EditProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditProfile());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetUserById();
        }
    }
}