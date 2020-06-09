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
        double Latitude;
        double Longitude;
        string PckCause;

        public MakeClaim(string subscription_id)
        {
            SubId = subscription_id;
            InitializeComponent();
            GetUserById();
            GetBanks();
            CheckInternet();
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

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
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
                        await DisplayAlert("Oops!", "Upload server error. Please try again later." , "ok");
                    }
                    else
                    {
                        await DisplayAlert("Whoops!", "Please try again later." , "ok");
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
                        await DisplayAlert("Oops!", "Server error. Please try again later.", "ok");
                    }
                    else if (k.StatusCode == 500)
                    {
                        await DisplayAlert("Oops!", "Session timeout, kindly login again.", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Oops!", "Please try again later.", "ok");
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
                        await DisplayAlert("Oops!", "Server error. Please try again later." , "ok");
                    }
                    else if(k.StatusCode == 500)
                    {
                        await DisplayAlert("Oops!" , "Session timeout, kindly login again." , "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Oops!", "Please try again later." , "ok");
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
                        await DisplayAlert("Oops!", "Server error. Please try again later.", "ok");
                    }
                    else if (k.StatusCode == 500)
                    {
                        await DisplayAlert("Oops!", "Session timeout, kindly login again.", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Oops!", "Please try again later.", "ok");
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

        private void CausesPck_SldIdxChanged(object sender, EventArgs e)
        {
            PckCause = (PickCauses.SelectedItem as Causes).Value;

        }

        async void Fetchdetails()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }

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
                    await DisplayAlert("Oops!", "Service Timeout, Kindly login again", "Ok");
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
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }

            if (string.IsNullOrEmpty(MaACNInput.Text) || string.IsNullOrEmpty(MaANMInput.Text) || string.IsNullOrEmpty(RecInput.Text) || string.IsNullOrEmpty(CostInput.Text))
            {;
                await DisplayAlert("Oops!", "Input fields cannot be empty", "Ok");
                MaACNInput.PlaceholderColor = Color.Red;
                MaANMInput.PlaceholderColor = Color.Red;
                RecInput.PlaceholderColor = Color.Red;
                CostInput.PlaceholderColor = Color.Red;

                return;
            }
            if(string.IsNullOrEmpty(LblDoc.Text) || string.IsNullOrEmpty(LblImg1.Text) || string.IsNullOrEmpty(LblImg2.Text) || string.IsNullOrEmpty(LblImg3.Text))
            {
                await DisplayAlert("Oops!","Please check if you've capture all required Images and documents.","Ok");
                return;
            }
            await PopupNavigation.Instance.PushAsync(new PopLoader());
            try
            {
                MakeClaimModel update = new MakeClaimModel()
                {
                    policyHolder = HelperAppSettings.username,
                    communityCode = HelperAppSettings.community_code,
                    policyNumber = policyNo,
                    incidentReport = RecInput.Text,
                    subscriptionId = SubId,
                    longitude = Longitude,
                    latitude = Latitude,
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
                    else if(response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        await PopupNavigation.Instance.PopAsync(true);
                        await DisplayAlert("InHub", "An error occured. Please try again later", "Ok");

                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await PopupNavigation.Instance.PopAsync(true);
                        await DisplayAlert("InHub", "Session timeout. Please Login again.", "Ok");
                        Application.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                    else
                    {
                        await PopupNavigation.Instance.PopAsync(true);
                        await DisplayAlert("InHub", "Please try again later.", "Ok");
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

                MaBankPicker.ItemsSource = UsersCnt;

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
                MaACNInput.TextColor = Color.Default;
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
                MaACNInput.TextColor = Color.Default;
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
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            await Permissions.RequestAsync<Permissions.LocationAlways>();
        }

        async void GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Oops!","Location is Not Supported","Ok");
                return;
            }
            catch (FeatureNotEnabledException)
            {
                await DisplayAlert("Oops!", "Location Feature is Not Enabled", "Ok");
                return;
            }
            catch (PermissionException)
            {
                Permission();
                return;
            }
            catch (Exception)
            {
                return;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetLocation();
        }
    }
}