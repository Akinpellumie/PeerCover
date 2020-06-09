using PeerCover.Models;
using PeerCover.ViewModels;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.FilePicker;
using System.Threading.Tasks;
using Plugin.FileUploader.Abstractions;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfile : ContentPage
    {
        string ProfileImage;
        string UserImage;
        string myGend;
        string bnkCd2;
        string bnkNm2;
        string bnkCde;
        string bnkNm;
        string gndPck;
        string firstname;
        string lastname;
        string email;
        string phone;
        string address;
        string acctNumEdit;
        string acctNameEdit;
        string filename;
        FileBytesItem bfitem;
        private string fileName;

        public EditProfile()
        {
            InitializeComponent();
            GetUserById();
            GetBanks();
            GenderPicker.BindingContext = new GenderViewModel();
        }
        public async void GetUserById()
        {
            HttpClient client = new HttpClient();
            var UserdetailEndpoint = Helper.getMembersUrl + HelperAppSettings.id;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var result = await client.GetStringAsync(UserdetailEndpoint);
            var MemberDetails = JsonConvert.DeserializeObject<MemberDetailsModel>(result);
            //JObject jsonDes = JObject.Parse(result);

            //PageName.BindingContext = jsonDes["member"]["capName"].ToString();
            //UserImagePro.BindingContext = MemberDetails.member[0];
            FNInput.BindingContext = MemberDetails.member[0];
            LNInput.BindingContext = MemberDetails.member[0];
            EMInput.BindingContext = MemberDetails.member[0];
            PNInput.BindingContext = MemberDetails.member[0];
            ACNInput.BindingContext = MemberDetails.member[0];
            ANMInput.BindingContext = MemberDetails.member[0];
            ADRInput.BindingContext = MemberDetails.member[0];
            BankPicker.BindingContext = MemberDetails.member[0];
            ProfileImage = MemberDetails.member[0].profile_img_url;
            BankPicker.Title = MemberDetails.member[0].bankname;
            GenderPicker.Title = MemberDetails.member[0].gender;
            myGend = MemberDetails.member[0].gender;
            bnkCde = MemberDetails.member[0].bank_code;
            bnkNm = MemberDetails.member[0].bankname;
            if (string.IsNullOrEmpty(ProfileImage))
            {
                EditUserImage.Source = "placeholder.png";
            }
            else
            {
                EditUserImage.Source = Helper.ImageUrl + ProfileImage;
            }

            if (string.IsNullOrEmpty(bnkNm))
            {
                BankPicker.Title = "--Select Bank--";
            }
            else
            {
                BankPicker.Title = bnkNm;
            }

            if (string.IsNullOrEmpty(myGend))
            {
                GenderPicker.Title = "--Select Gender--";
            }
            else
            {
                GenderPicker.Title = myGend;
            }

        }

        //protected override bool OnBackButtonPressed()
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        if (!string.IsNullOrEmpty(firstname) || !string.IsNullOrEmpty(lastname) || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(phone) || !string.IsNullOrEmpty(address) || !string.IsNullOrEmpty(gndPck) || !string.IsNullOrEmpty(bnkNm2) || !string.IsNullOrEmpty(acctNumEdit) || !string.IsNullOrEmpty(acctNameEdit))
        //        {
        //            var result = await DisplayAlert("Discard unsaved changes?", "You have unsaved changes, are you sure you want to discard them?", "Okay", "Cancel");
        //            if (result) await this.Navigation.PopAsync();

        //        }
        //    });
        //        return true;
        //}
        public async void CallIconUpload(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }

            await PopupNavigation.Instance.PushAsync(new PopLoader());
            if (string.IsNullOrEmpty(ProfileImage) && string.IsNullOrEmpty(filename))
            {
                await UpdateMemberClicked();
            }
            else if (string.IsNullOrEmpty(ProfileImage) && !string.IsNullOrEmpty(filename))
            {
                await IconImg_Clicked();
                await UpdateMemberClicked();
            }
            else if (!string.IsNullOrEmpty(ProfileImage) && !string.IsNullOrEmpty(filename))
            {
                await IconImg_Clicked();
                //await Task.CompletedTask;
                //{ { await IconImg_Clicked(); } }
                await UpdateMemberClicked();
            }
            else if (!string.IsNullOrEmpty(ProfileImage) && string.IsNullOrEmpty(filename))
            {
                await UpdateMemberClicked();
            }
            await PopupNavigation.Instance.PopAsync(true);
        }

        public async void CallPrfUploadAsync(object sender, EventArgs e)
        {
            var file2 = await CrossFilePicker.Current.PickFile();

            bfitem = new FileBytesItem("fileName", file2.DataArray, file2.FileName);

            FilePathItem fpitem = new FilePathItem("fileName", file2.FilePath);

            EditUserImage.Source = ImageSource.FromStream(() => file2.GetStream());

            if (file2 != null)
            {
                filename = file2.FilePath;
            }
            
        }

            async Task
IconImg_Clicked()
        {
                try
                {
                    
                             FileUploadResponse k = null;
                    try
                    {


                        k = await Plugin.FileUploader.CrossFileUploader.Current.UploadFileAsync(Helper.UploadUrl, bfitem, new Dictionary<string, string>() { { "Authorization", Helper.userprofile.token } }, new Dictionary<string, string>() { { "fileName", this.fileName } });
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    string responset = k.Message;
                    if (k.StatusCode == 201)
                    {

                        UserImage = responset;

                    }
                    else if (k.StatusCode == 401)
                    {
                        await DisplayAlert("InHub", k.Message, "ok");
                    }
                    else
                    {
                        await DisplayAlert("InHub", k.Message, "ok");
                    }

                }
                catch (Exception)
                {
                    return;
                }
        }



        async


        Task
UpdateMemberClicked()
        {
            //await PopupNavigation.Instance.PushAsync(new PopLoader());
            try
            {
                MembersModel update = new MembersModel()
                {
                    firstname = FNInput.Text,
                    lastname = LNInput.Text,
                    username = HelperAppSettings.username,
                    phonenumber = PNInput.Text,
                    email = EMInput.Text,
                    accountNumber = ACNInput.Text.Trim(),
                    address = ADRInput.Text,
                    accountName = ANMInput.Text,
                };
                if (string.IsNullOrEmpty(UserImage))
                {
                    update.profileImgUrl = ProfileImage;
                }
                else
                {
                    update.profileImgUrl = UserImage;
                }

                if (!string.IsNullOrEmpty(bnkNm2) && !string.IsNullOrEmpty(bnkCd2))
                {
                    update.bankName = bnkNm2;
                    update.bankCode = bnkCd2;
                }
                else if (string.IsNullOrEmpty(bnkNm2) && string.IsNullOrEmpty(bnkCd2))
                {
                    update.bankName = bnkNm;
                    update.bankCode = bnkCde;
                }

                if (!string.IsNullOrEmpty(myGend))
                {
                    update.gender = myGend;
                }
                else if (string.IsNullOrEmpty(myGend))
                {
                    update.gender = gndPck;
                }


                var clientee = new HttpClient();
                clientee.DefaultRequestHeaders.Clear();
                clientee.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                var jsonUpd = JsonConvert.SerializeObject(update);
                HttpContent result = new StringContent(jsonUpd);
                result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await clientee.PutAsync(Helper.getMembersUrl, result);

                if (response.IsSuccessStatusCode)
                {
                    await Indic.ProgressTo(0.9, 950, Easing.SpringIn);
                    //await PopupNavigation.Instance.PopAsync(true);
                    await DisplayAlert("Alert", "Profile Updated", "Ok");
                    await Shell.Current.Navigation.PushAsync(new Profile());
                    Indic.IsVisible = false;
                    //indicator.IsVisible = false;
                    //indicator.IsRunning = false;

                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        //await PopupNavigation.Instance.PopAsync(true);
                        await DisplayAlert("Oops!", "Server Unavailable! please try again later." , "Ok");

                    }
                    else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        //await PopupNavigation.Instance.PopAsync(true);
                        await DisplayAlert("Oops!", "Session expired. Kindly Login again.", "Ok");
                        Application.Current.MainPage = new NavigationPage(new LoginPage());

                    }
                    else
                    {
                        await DisplayAlert("Oops!" , "Please try again later." , "Ok");
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public async void GetBanks()
        {
            HttpClient client = new HttpClient();
            var UserCountEndpoint = Helper.BankNamesUrl;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var result = await client.GetStringAsync(UserCountEndpoint);
            var UsersCnt = JsonConvert.DeserializeObject<List<BanksModel>>(result);

            BankPicker.ItemsSource = UsersCnt;
        }

        public void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= 1)
            {
                ACNInput.TextColor = Color.Default;
                acctNumEdit = ACNInput.Text;
            }
            if (BankPicker.SelectedItem != null && string.IsNullOrEmpty(ACNInput.Text) == false)
            {
                if (e.NewTextValue.Length == 10)
                {
                    Fetchdetails();
                }

            }
        }
        public void Input2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= 1)
            {
                ANMInput.TextColor = Color.Default;
                acctNameEdit = ANMInput.Text;
            }
        }


        async void Fetchdetails()
        {

            await PopupNavigation.Instance.PushAsync(new PopAcctLoader());
            try
            {
                HttpClient client = new HttpClient();
                var BnkDetailsEndpoint = Helper.fetchBankDetails + (BankPicker.SelectedItem as BanksModel).code + "/" + ACNInput.Text;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                var result = await client.GetAsync(BnkDetailsEndpoint);
                string responsee = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var profile = JsonConvert.DeserializeObject<BanksModel>(responsee);
                    if (profile != null)
                    {
                        ANMInput.Text = profile.accountName;
                    }
                    else
                    {
                        await DisplayAlert("Alert", "An error occured, please try again later", "ok");
                        await PopupNavigation.Instance.PopAsync(true);
                    }

                    await PopupNavigation.Instance.PopAsync(true);
                }

                else if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Application.Current.MainPage = new LoginPage();
                }

                else
                {
                    await DisplayAlert("Alert", "An error occured, please re-enter account details", "ok");
                }
                //await PopupNavigation.Instance.PopAsync(true);
            }
            catch (Exception)
            {
                await PopupNavigation.Instance.PopAsync(true);
                return;
            }
        }


        private void BnkPck_SldIdxChanged(object sender, EventArgs e)
        {
            if (BankPicker.SelectedItem != null && string.IsNullOrEmpty(ACNInput.Text) == false)
            {
                Fetchdetails();
            }

            bnkNm2 = (BankPicker.SelectedItem as BanksModel).name;
            bnkCd2 = (BankPicker.SelectedItem as BanksModel).code;
        }


        private void GndPck_SldIdxChanged(object sender, EventArgs e)
        {
            gndPck = (GenderPicker.SelectedItem as Genders).Value;
        }

        public void InputFt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= 1)
            {
                firstname = FNInput.Text;
            }
        }

        public void InputLt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= 1)
            {
                lastname = LNInput.Text;
            }
        }
        public void InputEm_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= 1)
            {
                email = EMInput.Text;
            }
        }

        public void InputPn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= 1)
            {
                phone = PNInput.Text;
            }
        }

        public void InputAdr_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= 1)
            {
                address = ADRInput.Text;
            }
        }
    }
}