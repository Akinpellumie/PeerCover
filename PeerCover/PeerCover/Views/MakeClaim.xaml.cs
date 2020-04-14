using PeerCover.Models;
using PeerCover.ViewModels;
using Newtonsoft.Json;
using Plugin.FilePicker;
using Plugin.FileUploader.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MakeClaim : ContentPage
    {
        string policyNo;
        //List imageList;
        string ImageName1;
        string ImageName2;
        string ImageName3;
        string docName;
        private string fileName;
        string SubId;
        string bnkCde;
        string bnkNm;
        string bnkCd2;
        string bnkNm2;

        public MakeClaim(string subscription_id)
        {
            SubId = subscription_id;
            InitializeComponent();
            GetUserById();
            GetBanks();
            LoadUserPlan(subscription_id);
            PickCauses.BindingContext = new CausesViewModel();
        }

        public async void LoadUserPlan(string subscription_id)

        {
            var url = Helper.NewPlanUrl + subscription_id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(url);
            var UsersList = JsonConvert.DeserializeObject<ActiveSubModel>(result);
            PlanDetails.BindingContext = UsersList.subscription[0];
            policyNo = UsersList.subscription[0].policy_number;
        }

        private async void Button1_Clicked(object sender, EventArgs e)
        {
            var file1 = await CrossFilePicker.Current.PickFile();

            Plugin.FileUploader.Abstractions.FileBytesItem bfitem = new FileBytesItem("fileName", file1.DataArray, file1.FileName);

            Plugin.FileUploader.Abstractions.FilePathItem fpitem = new Plugin.FileUploader.Abstractions.FilePathItem("fileName", file1.FilePath);

            if (file1 != null)
            {
                LblImg1.Text = file1.FileName;
            }
            Plugin.FileUploader.Abstractions.FileUploadResponse k = null;
            try
            {

                k = await Plugin.FileUploader.CrossFileUploader.Current.UploadFileAsync(Helper.UploadUrl, bfitem, new Dictionary<string, string>() { { "Authorization", Helper.userprofile.token } }, new Dictionary<string, string>() { { "fileName", this.fileName } });
            }
            catch (Exception)
            {
                return;
            }
            string responsee = k.Message;
            if (k.StatusCode == 201)
            {

                ImageName1 = responsee;

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

        private async void Button2_Clicked(object sender, EventArgs e)
        {
            var file2 = await Plugin.FilePicker.CrossFilePicker.Current.PickFile();

            Plugin.FileUploader.Abstractions.FileBytesItem bfitem = new Plugin.FileUploader.Abstractions.FileBytesItem("fileName", file2.DataArray, file2.FileName);

            Plugin.FileUploader.Abstractions.FilePathItem fpitem = new Plugin.FileUploader.Abstractions.FilePathItem("fileName", file2.FilePath);

            if (file2 != null)
            {
                LblImg2.Text = file2.FileName;

            }
            Plugin.FileUploader.Abstractions.FileUploadResponse k = null;
            try
            {


                k = await Plugin.FileUploader.CrossFileUploader.Current.UploadFileAsync(Helper.UploadUrl, bfitem, new Dictionary<string, string>() { { "Authorization", Helper.userprofile.token } }, new Dictionary<string, string>() { { "fileName", this.fileName } });
            }
            catch (Exception)
            {
                return;
            }
            string responseee = k.Message;
            if (k.StatusCode == 201)
            {

                ImageName2 = responseee;

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

        private async void Button3_Clicked(object sender, EventArgs e)
        {
            var file3 = await CrossFilePicker.Current.PickFile();

            Plugin.FileUploader.Abstractions.FileBytesItem bfitem = new Plugin.FileUploader.Abstractions.FileBytesItem("fileName", file3.DataArray, file3.FileName);

            Plugin.FileUploader.Abstractions.FilePathItem fpitem = new Plugin.FileUploader.Abstractions.FilePathItem("fileName", file3.FilePath);

            if (file3 != null)
            {
                LblImg3.Text = file3.FileName;

            }
            Plugin.FileUploader.Abstractions.FileUploadResponse k = null;
            try
            {


                k = await Plugin.FileUploader.CrossFileUploader.Current.UploadFileAsync(Helper.UploadUrl, bfitem, new Dictionary<string, string>() { { "Authorization", Helper.userprofile.token } }, new Dictionary<string, string>() { { "fileName", this.fileName } });
            }
            catch (Exception)
            {
                return;
            }
            string responseeee = k.Message;
            if (k.StatusCode == 201)
            {

                ImageName3 = responseeee;

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

        public async void UploadImage1Tapped(object sender, EventArgs e)
        {
            Permission();
            await CrossMedia.Current.Initialize();
            try
            {


                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Warning", "Allow system.permission for this App", "Ok");

                }

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    Name = "DamageUpload.jpg",
                    CompressionQuality = 50,
                    SaveToAlbum = true
                });
                var mg1File = file.Path;
                if (string.IsNullOrEmpty(mg1File) == false)
                {
                    var upfilebytes = File.ReadAllBytes(mg1File);

                    ByteArrayContent baContent = new ByteArrayContent(upfilebytes);
                    var name = Path.GetFileName(mg1File);
                    FilePathItem filePath = new FilePathItem("fileName", file.Path);

                    if (file != null)
                    {
                        LblImg1.Text = file.AlbumPath;

                    }

                    ImageSource.FromStream(() => file.GetStream());


                    FileUploadResponse k = null;
                    try
                    {
                        k = await Plugin.FileUploader.CrossFileUploader.Current.UploadFileAsync(Helper.UploadUrl, filePath,
                            new Dictionary<string, string>() { { "Authorization", Helper.userprofile.token } }, new Dictionary<string, string>()
                            { { "fileName", this.fileName } });
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    string responseeee = k.Message;
                    if (k.StatusCode == 201)
                    {

                        ImageName1 = responseeee;

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
            }

            catch (Exception)
            {
                return;
            }
        }

        public async void UploadImage2Tapped(object sender, EventArgs e)
        {
            Permission();
            await CrossMedia.Current.Initialize();
            try
            {


                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Warning", "Allow system.permission for this App", "Ok");

                }

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    Name = "DamageUpload.jpg",
                    CompressionQuality = 50,
                    SaveToAlbum = true
                });
                var mg2File = file.Path;
                if (string.IsNullOrEmpty(mg2File) == false)
                {
                    var upfilebytes = File.ReadAllBytes(mg2File);

                    ByteArrayContent baContent = new ByteArrayContent(upfilebytes);
                    var name = Path.GetFileName(mg2File);
                    FilePathItem filePath = new FilePathItem("fileName", file.Path);

                    if (file != null)
                    {
                        LblImg2.Text = file.AlbumPath;

                    }

                    ImageSource.FromStream(() => file.GetStream());


                    FileUploadResponse k = null;
                    try
                    {
                        k = await Plugin.FileUploader.CrossFileUploader.Current.UploadFileAsync(Helper.UploadUrl, filePath,
                            new Dictionary<string, string>() { { "Authorization", Helper.userprofile.token } }, new Dictionary<string, string>()
                            { { "fileName", this.fileName } });
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    string responseeee = k.Message;
                    if (k.StatusCode == 201)
                    {

                        ImageName2 = responseeee;

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
            }

            catch (Exception)
            {
                return;
            }
        }

        public async void UploadImage3Tapped(object sender, EventArgs e)
        {
            Permission();
            await CrossMedia.Current.Initialize();
            try
            {


                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Warning", "Allow system.permission for this App", "Ok");

                }

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    Name = "DamageUpload.jpg",
                    CompressionQuality = 50,
                    SaveToAlbum = true
                });
                var mg3File = file.Path;
                if (string.IsNullOrEmpty(mg3File) == false)
                {
                    var upfilebytes = File.ReadAllBytes(mg3File);

                    ByteArrayContent baContent = new ByteArrayContent(upfilebytes);
                    var name = Path.GetFileName(mg3File);
                    FilePathItem filePath = new FilePathItem("fileName", file.Path);

                    if (file != null)
                    {
                        LblImg3.Text = file.AlbumPath;

                    }

                    ImageSource.FromStream(() => file.GetStream());


                    FileUploadResponse k = null;
                    try
                    {
                        k = await Plugin.FileUploader.CrossFileUploader.Current.UploadFileAsync(Helper.UploadUrl, filePath,
                            new Dictionary<string, string>() { { "Authorization", Helper.userprofile.token } }, new Dictionary<string, string>()
                            { { "fileName", this.fileName } });
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    string responseeee = k.Message;
                    if (k.StatusCode == 201)
                    {

                        ImageName3 = responseeee;

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
            }

            catch (Exception)
            {
                return;
            }
        }

        public async void UploadDocTapped(object sender, EventArgs e)
        {
            Permission();
            await CrossMedia.Current.Initialize();
            try
            {


                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Warning", "Allow system.permission for this App", "Ok");

                }

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    Name = "DamageUpload.jpg",
                    CompressionQuality = 50,
                    SaveToAlbum = true
                });
                var mg3File = file.Path;
                if (string.IsNullOrEmpty(mg3File) == false)
                {
                    var upfilebytes = File.ReadAllBytes(mg3File);

                    ByteArrayContent baContent = new ByteArrayContent(upfilebytes);
                    var name = Path.GetFileName(mg3File);
                    FilePathItem filePath = new FilePathItem("fileName", file.Path);

                    if (file != null)
                    {
                        LblDoc.Text = file.AlbumPath;

                    }

                    ImageSource.FromStream(() => file.GetStream());


                    FileUploadResponse k = null;
                    try
                    {
                        k = await Plugin.FileUploader.CrossFileUploader.Current.UploadFileAsync(Helper.UploadUrl, filePath,
                            new Dictionary<string, string>() { { "Authorization", Helper.userprofile.token } }, new Dictionary<string, string>()
                            { { "fileName", this.fileName } });
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    string responseeee = k.Message;
                    if (k.StatusCode == 201)
                    {

                        docName = responseeee;

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
            }

            catch (Exception)
            {
                return;
            }
        }

        private void BnkPck_SldIdxChanged(object sender, EventArgs e)
        {
            if (MaBankPicker.SelectedItem != null && string.IsNullOrEmpty(MaACNInput.Text) == false)
            {
                Fetchdetails();
            }

            bnkNm2 = (MaBankPicker.SelectedItem as BanksModel).name;
            bnkCd2 = (MaBankPicker.SelectedItem as BanksModel).code;

        }

        async void Fetchdetails()
        {

            await PopupNavigation.Instance.PushAsync(new PopAcctLoader());
            try
            {
                HttpClient client = new HttpClient();
                var BnkDetailsEndpoint = Helper.fetchBankDetails + (MaBankPicker.SelectedItem as BanksModel).code + "/" + MaACNInput.Text;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                var result = await client.GetAsync(BnkDetailsEndpoint);
                string responsee = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var profile = JsonConvert.DeserializeObject<BanksModel>(responsee);
                    if (profile != null)
                    {
                        MaANMInput.Text = profile.accountName;
                    }
                    else
                    {
                        await DisplayAlert("Alert", "An error occured, please try again later", "ok");
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

        public void CostInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((Entry)sender).Text))
            {


                var t = double.Parse(((Entry)sender).Text.Replace("₦", CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol), NumberStyles.Currency).ToString("C", CultureInfo.GetCultureInfo("en-us")).Replace("$", "₦");
                ((Entry)sender).Text = t;
                ((Entry)sender).CursorPosition = t.Length - 3;
            }


        }

        private async void MakeClaimClicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new PopLoader());
            if (string.IsNullOrEmpty(MaACNInput.Text) || string.IsNullOrEmpty(MaANMInput.Text))
            {
                await PopupNavigation.Instance.PopAsync(true);
                await DisplayAlert("Oops!", "Account Number or Account Name cannot be empty", "Ok");
                MaACNInput.TextColor = Color.Red;
                MaANMInput.TextColor = Color.Red;
                return;
            }

            try
            {
                MakeClaimModel update = new MakeClaimModel()
                {
                    policyHolder = HelperAppSettings.username,
                    communityCode = HelperAppSettings.community_code,
                    policyNumber = policyNo,
                    incidentReport = RecInput.Text,
                    subscriptionId = SubId,
                    incidentCause = (PickCauses.SelectedItem as Causes).Value,
                    claimRolePlayed = "Claimant",
                    accountName = MaANMInput.Text,
                    accountNumber = MaACNInput.Text,
                };
                var pt = double.Parse(CostInput.Text.Replace
                   ("₦", CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol),
                   NumberStyles.Currency).ToString();

                update.policyHolderClaimAmount = pt;

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
                update.claimOwnerQuotationDoc = docName;
                var t = new List<string>
                {
                    ImageName1,
                    ImageName2,
                    ImageName3
                };
                update.imgDamageUrl = t;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                var json = JsonConvert.SerializeObject(update);
                HttpContent result = new StringContent(json);
                result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(Helper.GetClaimsUrl, result);

                if (response.IsSuccessStatusCode)
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    await DisplayAlert("Yipee!!!", " Your Claim Request has been made Successfully!!!", "Ok");
                    if (HelperAppSettings.priviledges.Contains("ADMIN"))
                    {
                        AppShell fpm = new AppShell();
                        Application.Current.MainPage = fpm;

                    }
                    else
                    {
                        Application.Current.MainPage = new AppShellUser();
                    }
                    CostInput.Text = "";
                    RecInput.Text = "";

                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        await PopupNavigation.Instance.PopAsync(true);
                        await DisplayAlert("InHub", response.ReasonPhrase, "Ok");

                    }
                    else
                    {
                        await PopupNavigation.Instance.PopAsync(true);
                        await DisplayAlert("InHub", "Please try again later", "Ok");

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
            var UserCountEndpoint = Helper.getBanksUrl;

            var result = await client.GetStringAsync(UserCountEndpoint);
            var UsersCnt = JsonConvert.DeserializeObject<BankNameModels>(result);

            MaBankPicker.ItemsSource = UsersCnt.banks;

        }

        public async void GetUserById()
        {
            HttpClient client = new HttpClient();
            var UserdetailEndpoint = Helper.getMembersUrl + HelperAppSettings.id;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var result = await client.GetStringAsync(UserdetailEndpoint);
            var MemberDetails = JsonConvert.DeserializeObject<MemberDetailsModel>(result);
            AcctDetailsStack.BindingContext = MemberDetails.member[0];
            MaACNInput.BindingContext = MemberDetails.member[0];
            MaANMInput.BindingContext = MemberDetails.member[0];
            MaBankPicker.BindingContext = MemberDetails.member[0];
            MaBankPicker.Title = MemberDetails.member[0].bankname;
            bnkCde = MemberDetails.member[0].bank_code;
            bnkNm = MemberDetails.member[0].bankname;
        }
        public void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= 1)
            {
                MaACNInput.TextColor = Color.Accent;
            }

            if (MaBankPicker.SelectedItem != null && string.IsNullOrEmpty(MaACNInput.Text) == false)
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
                MaACNInput.TextColor = Color.Accent;
            }
        }
        private class List
        {
            public string ImageName1 { get; set; }
            public string ImageName2 { get; set; }
            public string ImageName3 { get; set; }
        }

        public void MainScrn_clicked(object sender, EventArgs e)
        {
            AcctDetailsStack.IsVisible = true;
            MainDetails.IsVisible = false;
        }

        public void AcctScrn_clicked(object sender, EventArgs e)
        {
            AcctDetailsStack.IsVisible = false;
            MainDetails.IsVisible = true;
        }

        async void Permission()
        {
            await Permissions.RequestAsync<Permissions.Camera>();
            await Permissions.RequestAsync<Permissions.StorageRead>();
            await Permissions.RequestAsync<Permissions.StorageWrite>();
        }
    }
}