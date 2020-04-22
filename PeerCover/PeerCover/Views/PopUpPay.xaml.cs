using Newtonsoft.Json;
using PeerCover.Models;
using PeerCover.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpPay
    {
        string transId;
        public PopUpPay(string txnId)
        {
            transId = txnId;
            InitializeComponent();
            PickMethod.BindingContext = new PaymentMethViewModel();
        }

        public async void VerifyPayment_Clicked(object sender, EventArgs e)
        {
            try
            {
            indicator.IsRunning = true;
            indicator.IsVisible = true;
            
                PayMethModel update = new PayMethModel()
                {
                    username = HelperAppSettings.username,
                    transactionId = transId,
                    transRefNum = transRef.Text,
                    paymentMethod = (PickMethod.SelectedItem as PaymentMethods).Value
            };
                var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                var json = JsonConvert.SerializeObject(update);
                HttpContent result = new StringContent(json);
                result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(Helper.VerifyBankPayUrl, result);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "Your transaction details has been verified successfully!!!", "Ok");
                    await PopupNavigation.Instance.PopAsync(true);
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        await DisplayAlert("Alert", "Whoopss! Try Again Later", "Ok");
                        await PopupNavigation.Instance.PopAsync(true);
                        indicator.IsVisible = false;
                        indicator.IsRunning = false;
                    }
                    else
                    {
                        await DisplayAlert("Alert", "Please try again later", "Ok");
                        await PopupNavigation.Instance.PopAsync(true);
                        indicator.IsVisible = false;
                        indicator.IsRunning = false;
                    }
                }

            }
            catch (Exception)
            {
                return;
            }
        }
    }
}