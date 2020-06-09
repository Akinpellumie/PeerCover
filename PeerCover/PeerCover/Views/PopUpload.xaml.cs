using PeerCover.Models;
using Newtonsoft.Json;
using Plugin.FileUploader.Abstractions;
using Plugin.Media;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpload
    {
        private string fileName;
        string subId;
        public static string newImgName { get; set; }

        public PopUpload(string NumId)
        {
            subId = NumId;
            InitializeComponent();
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
                Indic.IsVisible = true;
                var miFile = file.Path;
                if (string.IsNullOrEmpty(miFile) == false)
                {
                    var upfilebytes = File.ReadAllBytes(miFile);

                    ByteArrayContent baContent = new ByteArrayContent(upfilebytes);
                    var name = Path.GetFileName(miFile);
                    FilePathItem filePath = new FilePathItem("fileName", file.Path);

                    //if (file != null)
                    //{
                    //    lbl1.Text = file.AlbumPath;

                    //}

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
                        newImgName = responseeee;
                        await Indic.ProgressTo(0.9, 500, Easing.SpringIn);

                        TransactionModel update = new TransactionModel()
                        {
                            subscriptionId = subId,
                            vehicleImgUrl = newImgName
                        };

                        var client = new HttpClient();
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                        var json = JsonConvert.SerializeObject(update);
                        HttpContent result = new StringContent(json);
                        result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        var response = await client.PostAsync(Helper.UpdateCarImg, result);

                        if (response.IsSuccessStatusCode)
                        {
                            await Indic.ProgressTo(0.9, 950, Easing.SpringIn);
                            await DisplayAlert("Success", "Vehicle Image Updated", "Ok");
                            await PopupNavigation.Instance.PopAsync(true);
                            Indic.IsVisible = false;
                        }
                        else if (k.StatusCode == 401)
                        {
                            Indic.IsVisible = false;
                            await DisplayAlert("InHub", k.Message, "ok");

                        }
                        else
                        {
                            Indic.IsVisible = false;
                            await DisplayAlert("InHub", k.Message, "ok");
                        }
                    }
                }
            }

            catch (Exception)
            {
                return;
            }
        }

        //private async void NewSubClicked(object sender, EventArgs e)
        //{
        //    Indic.IsVisible = true;
        //    try
        //    {
        //        TransactionModel update = new TransactionModel()
        //        {
        //            subscriptionId = subId,
        //            vehicleImgUrl = newImgName
        //        };

        //        var client = new HttpClient();
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

        //        var json = JsonConvert.SerializeObject(update);
        //        HttpContent result = new StringContent(json);
        //        result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //        var response = await client.PostAsync(Helper.NewPlanUrl, result);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            await PopupNavigation.Instance.PopAsync(true);
        //            await DisplayAlert("InHub", "New Plan Subscribed", "Ok");
        //            await Shell.Current.Navigation.PushAsync(new Dashboard());
        //            VMInput.Text = "";
        //            RNInput.Text = "";
        //            VCInput.Text = "";
        //            YMInput.Text = "";
        //            VLMInput.Text = "";
        //            VINInput.Text = "";
        //            PRMInput.Text = "";
        //            ENInput.Text = "";
        //            indicator.IsVisible = false;
        //            indicator.IsRunning = false;

        //        }
        //        else
        //        {
        //            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        //            {
        //                await PopupNavigation.Instance.PopAsync(true);
        //                await DisplayAlert("Whoops!", response.ReasonPhrase, "Ok");
        //                indicator.IsVisible = false;
        //                indicator.IsRunning = false;
        //            }
        //            else
        //            {
        //                await PopupNavigation.Instance.PopAsync(true);
        //                indicator.IsRunning = false;
        //                indicator.IsVisible = false;
        //                await DisplayAlert("Whoops!", "Please try again later", "Ok");

        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return;
        //    }

        //}

        async void Permission()
        {
            await Permissions.RequestAsync<Permissions.Camera>();
            await Permissions.RequestAsync<Permissions.StorageRead>();
            await Permissions.RequestAsync<Permissions.StorageWrite>();
        }
    }
}