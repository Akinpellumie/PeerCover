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
    public partial class NewPlan : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<PlansListModel> plan_id { get; set; }

        string ImageName;
        string utilityBillName;
        string licenseName;
        private string fileName;

        public NewPlan()
        {
            InitializeComponent();
        }


        public void Input_unfocused(object sender, FocusEventArgs e)
        {
            var pee = double.Parse(VLMInput.Text.Replace
                    ("₦", CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol),
                    NumberStyles.Currency).ToString();
            if (pee != null)
            {

                int TargetDefect;

                if (int.TryParse(pee, out TargetDefect))
                {
                    double Per = ((double)TargetDefect) * 0.03;
                    PRMInput.Text = Per.ToString();
                }

                //var P = e.NewTextValue;
                //Double.TryParse("4.0", CultureInfo.InvariantCulture);
                //double x = Int32.Parse(P);
                //PRMInput.Text = x * 0.3;
            }
            else if (VLMInput.Text == null)
            {
                PRMInput.Text = "";
            }
            if (string.IsNullOrEmpty(VLMInput.Text))
            {
                PRMInput.Text = "";
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
                        lbl1.Text = file.AlbumPath;

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
                        lbl2.Text = file.AlbumPath;

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
                        lbl.Text = file.AlbumPath;

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
            if(string.IsNullOrEmpty(VMInput.Text) || string.IsNullOrEmpty(RNInput.Text) || string.IsNullOrEmpty(VCInput.Text) || string.IsNullOrEmpty(YMInput.Text) || string.IsNullOrEmpty(ENInput.Text) || string.IsNullOrEmpty(VLMInput.Text) || string.IsNullOrEmpty(PRMInput.Text) || string.IsNullOrEmpty(VINInput.Text))
            {
                await DisplayAlert("Holla!", "Input fields cannot be empty.", "Ok");
                return;
            }

            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }

            await PopupNavigation.Instance.PushAsync(new PopLoader());

            if ((VLMInput.Text).Length <= 0)
            {
                await DisplayAlert("Alert", "Amount can not be less than 1", "ok");
                return;
            }
            try
            {
                NewPlanModel update = new NewPlanModel()
                {
                    vehicleMake = VMInput.Text,
                    vehicleRegNum = RNInput.Text,
                    vehicleColor = VCInput.Text,
                    yearOfMake = YMInput.Text,
                    //valuationAmount = VLMInput.Text,
                    vin = VINInput.Text,
                    //premium = PRMInput.Text,
                    coveragePeriod = "1YR",
                    planId = "101",
                    username = HelperAppSettings.username,
                    engineNumber = ENInput.Text,

                };
                //update.valuationAmount = this.AstraStack.BindingContext as TransferAstrainput;
                var p = double.Parse(VLMInput.Text.Replace
                    ("₦", CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol),
                    NumberStyles.Currency).ToString();
                var pt = double.Parse(PRMInput.Text.Replace
                   ("₦", CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol),
                   NumberStyles.Currency).ToString();
                update.premium = pt;
                update.valuationAmount = p;
                update.vehicleImage = ImageName;
                update.utilityBillUrl = utilityBillName;
                update.driverLicenseUrl = licenseName;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                var json = JsonConvert.SerializeObject(update);
                HttpContent result = new StringContent(json);
                result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(Helper.NewPlanUrl, result);

                if (response.IsSuccessStatusCode)
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    await DisplayAlert("Success!", "New Plan Subscribed", "Ok");
                    await Shell.Current.Navigation.PushAsync(new Dashboard());
                    VMInput.Text = "";
                    RNInput.Text = "";
                    VCInput.Text = "";
                    YMInput.Text = "";
                    VLMInput.Text = "";
                    VINInput.Text = "";
                    PRMInput.Text = "";
                    ENInput.Text = "";
                    indicator.IsVisible = false;
                    indicator.IsRunning = false;

                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        await PopupNavigation.Instance.PopAsync(true);
                        await DisplayAlert("Whoops!", "Server error! Please try again later." , "Ok");
                        indicator.IsVisible = false;
                        indicator.IsRunning = false;
                    }
                    else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await DisplayAlert("Ooops!","Session timeout. Please Login again.","Ok");
                        Application.Current.MainPage = new NavigationPage(new LoginPage());
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
                DisplayAlert("Oops!", "Kindly upload your Vehicle Image", "Ok");
                return;
            }
            FirstScreen.IsVisible = false;
            SecondScreen.IsVisible = true;
        }

        public void ScndScrn_clicked(object sender, EventArgs e)
        {
            FirstScreen.IsVisible = true;
            SecondScreen.IsVisible = false;
        }

        async void Permission()
        {
            await Permissions.RequestAsync<Permissions.Camera>();
            await Permissions.RequestAsync< Permissions.StorageRead>();
            await Permissions.RequestAsync<Permissions.StorageWrite>();
        }
    }

}