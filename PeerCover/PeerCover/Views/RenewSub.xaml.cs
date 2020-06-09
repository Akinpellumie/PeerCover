using Plugin.FilePicker;
using System;
using PeerCover.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Plugin.FileUploader.Abstractions;
using System.ComponentModel;
using Rg.Plugins.Popup.Services;
using Plugin.Media;
using System.IO;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RenewSub : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<PlansListModel> plan_id { get; set; }

        string ImageName;
        string utilityBillName;
        string licenseName;
        private string fileName;
        string renewSubId;
        string polNum;

        public RenewSub(string subscription_id, string policyNo)
        {
            renewSubId = subscription_id;
            polNum = policyNo;
            InitializeComponent();
            CheckInternet();
            LoadSingleSub(subscription_id);
        }

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }

        public async void LoadSingleSub(string subscription_id)

        {
            var url = Helper.NewPlanUrl + subscription_id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(url);
            var UsersList = JsonConvert.DeserializeObject<ActiveSubModel>(result);
            renewSubStack.BindingContext = UsersList.subscription[0];
        }


        public void Input_unfocused(object sender, FocusEventArgs e)
        {
            var pee = double.Parse(ReVLMInput.Text.Replace
                    ("₦", CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol),
                    NumberStyles.Currency).ToString();
            if (pee != null)
            {

                int TargetDefect;

                if (int.TryParse(pee, out TargetDefect))
                {
                    double Per = ((double)TargetDefect) * 0.03;
                    RePRMInput.Text = Per.ToString();
                }

            }
            else if (ReVLMInput.Text == null)
            {
                RePRMInput.Text = "";
            }
            if (string.IsNullOrEmpty(ReVLMInput.Text))
            {
                RePRMInput.Text = "";
            }
        }
        public void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((Entry)sender).Text))
            {


                var t = double.Parse(((Entry)sender).Text.Replace("₦", CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol), NumberStyles.Currency).ToString("C", CultureInfo.GetCultureInfo("en-us")).Replace("$", "₦");
                ((Entry)sender).Text = t;
                ((Entry)sender).CursorPosition = t.Length - 3;
            }


        }

        public void InputPrm_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((Entry)sender).Text))
            {


                var ti = double.Parse(((Entry)sender).Text.Replace("₦", CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol), NumberStyles.Currency).ToString("C", CultureInfo.GetCultureInfo("en-us")).Replace("$", "₦");
                ((Entry)sender).Text = ti;
                ((Entry)sender).CursorPosition = ti.Length - 3;
            }
        }


        public async void UploadImaTapped(object sender, EventArgs e)
        {
            Permission();
            await CrossMedia.Current.Initialize();
            try
            {


                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Warning", "Allow system.permission for this App", "Ok");

                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    Name = "vehicleUpload.jpg",
                    CompressionQuality = 50,
                    SaveToAlbum = true
                });
                var miFile = file.Path;
                if (string.IsNullOrEmpty(miFile) == false)
                {
                    var upfilebytes = File.ReadAllBytes(miFile);

                    ByteArrayContent baContent = new ByteArrayContent(upfilebytes);
                    var name = Path.GetFileName(miFile);
                    FilePathItem filePath = new FilePathItem("fileName", file.Path);

                    if (file != null)
                    {
                        Relbl1.Text = file.AlbumPath;

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

                        ImageName = responseeee;

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

        public async void UploadBillTapped(object sender, EventArgs e)
        {
            Permission();
            await CrossMedia.Current.Initialize();
            try
            {


                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Warning", "Allow system.permission for this App", "Ok");

                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    Name = "BillUpload.jpg",
                    CompressionQuality = 50,
                    SaveToAlbum = true
                });
                var biFile = file.Path;
                if (string.IsNullOrEmpty(biFile) == false)
                {
                    var upfilebytes = File.ReadAllBytes(biFile);

                    ByteArrayContent baContent = new ByteArrayContent(upfilebytes);
                    var name = Path.GetFileName(biFile);
                    FilePathItem filePath = new FilePathItem("fileName", file.Path);


                    if (file != null)
                    {
                        Relbl2.Text = file.AlbumPath;

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

                        utilityBillName = responseeee;

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

        public async void UploadLicenseTapped(object sender, EventArgs e)
        {
            Permission();
            await CrossMedia.Current.Initialize();
            try
            {


                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Warning", "Allow system.permission for this App", "Ok");

                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    Name = "DLUpload.jpg",
                    CompressionQuality = 50,
                    SaveToAlbum = true
                });
                var dlFile = file.Path;
                if (string.IsNullOrEmpty(dlFile) == false)
                {
                    var upfilebytes = File.ReadAllBytes(dlFile);

                    ByteArrayContent baContent = new ByteArrayContent(upfilebytes);
                    var name = Path.GetFileName(dlFile);
                    FilePathItem filePath = new FilePathItem("fileName", file.Path);


                    if (file != null)
                    {
                        Relbl.Text = file.AlbumPath;

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

                        licenseName = responseeee;

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

        public void amount_textChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= 1)
            {
                Math.Round(Convert.ToDouble(e), 2).ToString("C", CultureInfo.GetCultureInfo("en-us")).Replace("$", "₦");
            }

        }


        private async void NewSubClicked(object sender, EventArgs e)
        {


            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }

            await PopupNavigation.Instance.PushAsync(new PopLoader());

            if ((ReVLMInput.Text).Length <= 0)
            {
                await DisplayAlert("Alert", "Amount can not be less than 1", "ok");
                return;
            }
            try
            {
                NewPlanModel update = new NewPlanModel()
                {
                    vehicleMake = ReVMInput.Text,
                    vehicleRegNum = ReRNInput.Text,
                    vehicleColor = ReVCInput.Text,
                    yearOfMake = ReYMInput.Text,
                    vin = ReVINInput.Text,
                    coveragePeriod = "1YR",
                    planId = "101",

                    username = HelperAppSettings.username,
                    engineNumber = ReENInput.Text,

                };
                var p = double.Parse(ReVLMInput.Text.Replace
                    ("₦", CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol),
                    NumberStyles.Currency).ToString();

                var pt = double.Parse(RePRMInput.Text.Replace
                   ("₦", CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol),
                   NumberStyles.Currency).ToString();
                update.premium = pt;
                update.valuationAmount = p;
                update.vehicleImage = ImageName;
                update.utilityBillUrl = utilityBillName;
                update.driverLicenseUrl = licenseName;
                update.subscriptionId = renewSubId;
                update.policyNumber = polNum;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                var json = JsonConvert.SerializeObject(update);
                HttpContent result = new StringContent(json);
                result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(Helper.RenewSubUrl, result);

                if (response.IsSuccessStatusCode)
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    await DisplayAlert("Success!", "Subscription Renewed!", "Ok");
                    if (HelperAppSettings.priviledges.Contains("ADMIN"))
                    {
                        AppShell fpm = new AppShell();
                        Application.Current.MainPage = fpm;

                    }
                    else
                    {
                        Application.Current.MainPage = new AppShellUser();
                    }
                    ReVMInput.Text = "";
                    ReRNInput.Text = "";
                    ReVCInput.Text = "";
                    ReYMInput.Text = "";
                    ReVLMInput.Text = "";
                    ReVINInput.Text = "";
                    RePRMInput.Text = "";
                    ReENInput.Text = "";
                    indicator.IsVisible = false;
                    indicator.IsRunning = false;

                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        await PopupNavigation.Instance.PopAsync(true);
                        await DisplayAlert("Whoops!", response.ReasonPhrase, "Ok");
                        indicator.IsVisible = false;
                        indicator.IsRunning = false;
                    }
                    else
                    {
                        await PopupNavigation.Instance.PopAsync(true);
                        indicator.IsRunning = false;
                        indicator.IsVisible = false;
                        await DisplayAlert("Whoops!", "Please try again later", "Ok");

                    }
                }

            }
            catch (Exception)
            {
                return;
            }

        }

        public void firstScrn_clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ImageName) && string.IsNullOrEmpty(utilityBillName) && string.IsNullOrEmpty(licenseName))
            {
                DisplayAlert("Oops!", "Kindly select a more clearer image", "Ok");
                return;
            }
            ReFirstScreen.IsVisible = false;
            ReSecondScreen.IsVisible = true;
        }

        public void ScndScrn_clicked(object sender, EventArgs e)
        {
            ReFirstScreen.IsVisible = true;
            ReSecondScreen.IsVisible = false;
        }

        async void Permission()
        {
            await Permissions.RequestAsync<Permissions.Camera>();
            await Permissions.RequestAsync<Permissions.StorageRead>();
            await Permissions.RequestAsync<Permissions.StorageWrite>();
        }
    }
}