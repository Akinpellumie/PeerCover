using PeerCover.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayPremiums : ContentPage
    {
        public static string planId;
        double pre;
        string NumId;
        string vehMk;
        string polNo;
        public PayPremiums()
        {
            InitializeComponent();
            GetSubDetails();
            //LoadSinPlan();
        }

        public async void GetSubDetails()
        {
            await PopupNavigation.Instance.PushAsync(new PopLoader());
            HttpClient client = new HttpClient();
            var UserCountEndpoint = Helper.getActiveSubUrl + HelperAppSettings.username;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var result = await client.GetStringAsync(UserCountEndpoint);
            var UsersCnt = JsonConvert.DeserializeObject<ActSubModel>(result);

            SubPicker.ItemsSource = UsersCnt.subscriptions;
            await PopupNavigation.Instance.PopAsync(true);
            if (SubPicker.ItemsSource == null)
            {
                SubPicker.IsEnabled = false;
                SubPicker.Title = "No available Subscription";
            }
        }
        public async void SelectedItemEvent_Clicked(object sender, EventArgs e)
        {
            var t = SubPicker.SelectedItem as SubscriptionModel;
            var myId = t.subscription_id;
            NumId = myId;
            var url = Helper.NewPlanUrl + myId;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var resultee = await client.GetStringAsync(url);
            var UsersList = JsonConvert.DeserializeObject<ActiveSubModel>(resultee);
            SinSubPay.BindingContext = UsersList.subscription[0];
            pre = UsersList.subscription[0].premium;
            vehMk = UsersList.subscription[0].vehicle_make;
            polNo = UsersList.subscription[0].policy_number;

            if (SinSubPay != null)
            {
                //await ((StackLayout)sender).ScaleTo(0.8, length: 50, Easing.Linear);
                //await Task.Delay(100);
                SinSubPay.IsVisible = true;
            }
        }

        public async void AstraWebPayClicked(object sender, EventArgs e)
        {
            await ((Frame)sender).ScaleTo(0.8, length: 50, Easing.Linear);
            await Task.Delay(100);
            await ((Frame)sender).ScaleTo(1, length: 50, Easing.Linear);

            spin.IsVisible = true;
            try
            {
                if (SinSubPay.IsVisible == false)
                {
                    await DisplayAlert("Oops", "Kindly Select a Subscription above", "Ok");
                    spin.IsVisible = false;
                    return;
                }

                if (!string.IsNullOrEmpty(NumId))
                {
                    var url = Helper.ExpiredImgUrl + NumId;
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
                    var resultee = await client.GetStringAsync(url);

                    //string responseeee = await resultee.Content.ReadAsStringAsync();
                    if (resultee.Contains("Image is expired"))
                    {
                        await PopupNavigation.Instance.PushAsync(new PopUpload(NumId));

                        CallAstraPay();

                    }
                    else if (resultee.Contains("Image not expired"))
                    {
                        CallAstraPay();
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        async void CallAstraPay()
        {
            try
            {
                {
                    TransactionModel txndetails = new TransactionModel()
                    {
                        subscriptionId = NumId,
                        depositorsName = HelperAppSettings.Name,
                        depositorsUsername = HelperAppSettings.username,
                        recipientName = "InsuranceHub",
                        recipientUsername = "InsuranceHub",
                        policyHolder = HelperAppSettings.username,
                        policyNumber = polNo,
                        vehicleMake = vehMk,
                        communityCode = HelperAppSettings.community_code,
                        transactionType = "premium",
                        paymentMethod = "Astra Pay",

                    };
                    var p = pre;
                    var pel = p.ToString();
                    txndetails.paymentAmount = pel;

                    var json = JsonConvert.SerializeObject(txndetails);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
                    var logEndpoint = Helper.TransUrl;

                    var result = await client.PostAsync(logEndpoint, content);
                    string responsee = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode)
                    {
                        var pro = JsonConvert.DeserializeObject<TransResModel>(responsee);
                        Helper.transResponse = pro;
                        TransHelper.referenceNumber = pro.transactionReferenceNumber;
                        TransHelper.transactionId = pro.transactionId;
                        TransHelper.inHubRefNum = pro.inHubRefNum;
                        await Navigation.PushAsync(new AstraWebView());
                        spin.IsVisible = false;
                    }
                    else
                    {
                        await DisplayAlert("Oops!", "Please try again Later!", "Ok");
                        spin.IsVisible = false;
                    }
                }

            }
            catch (Exception)
            {
                return;
            }
        }
        public async void CopyClicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = "0125463876",
                Title = "Copy!"
            });
        }
          
        public async void PayWithCardClicked(object sender, EventArgs e)
        {
            await ((Frame)sender).ScaleTo(0.8, length: 50, Easing.Linear);
            await Task.Delay(100);
            await ((Frame)sender).ScaleTo(1, length: 50, Easing.Linear);
            spin1.IsVisible = true;
            try
            {
                if (SinSubPay.IsVisible == false)
                {
                    await DisplayAlert("Oops", "Kindly Select a Subscription above", "Ok");
                    spin1.IsVisible = false;
                    return;
                }

                else
                {
                    TransactionModel txndetails = new TransactionModel()
                    {
                        subscriptionId = NumId,
                        depositorsName = HelperAppSettings.Name,
                        depositorsUsername = HelperAppSettings.username,
                        recipientName = "InsuranceHub",
                        recipientUsername = "InsuranceHub",
                        policyHolder = HelperAppSettings.username,
                        policyNumber = polNo,
                        vehicleMake = vehMk,
                        communityCode = HelperAppSettings.community_code,
                        transactionType = "premium",
                        paymentMethod = "Paystack",

                    };
                    var p = pre;
                    var pel = p.ToString();
                    txndetails.paymentAmount = pel;

                    var json = JsonConvert.SerializeObject(txndetails);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
                    var logEndpoint = Helper.TransUrl;

                    var result = await client.PostAsync(logEndpoint, content);
                    string responsee = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode)
                    {
                        var pro = JsonConvert.DeserializeObject<TransResModel>(responsee);
                        Helper.transResponse = pro;
                        TransHelper.referenceNumber = pro.transactionReferenceNumber;
                        TransHelper.transactionId = pro.transactionId;
                        TransHelper.inHubRefNum = pro.inHubRefNum;

                        await Navigation.PushAsync(new PayWithCard());
                        spin1.IsVisible = false;
                    }
                }
            }
            catch (Exception)
            {
                spin1.IsVisible = false;
                return;
            }
        }


        public async void OnBankTrans_Clicked(object sender, EventArgs e)
        {
            await ((Frame)sender).ScaleTo(0.8, length: 50, Easing.Linear);
            await Task.Delay(100);
            await ((Frame)sender).ScaleTo(1, length: 50, Easing.Linear);
            if (BnkOpts.IsVisible == false)
            {
                await DisplayAlert("Notice!", "Kindly use the  Transaction ID as your Depositor's Name in the " +
                    "Bank OR in the Description section of your mobile bank Transfer", "Ok");
                spin2.IsVisible = true;
                try
                {
                    if (SinSubPay.IsVisible == false)
                    {
                        await DisplayAlert("Oops", "Kindly Select a Subscription above", "Ok");
                        spin2.IsVisible = false;
                        return;
                    }

                    else
                    {
                        TransactionModel txndetails = new TransactionModel()
                        {
                            subscriptionId = NumId,
                            depositorsName = HelperAppSettings.Name,
                            depositorsUsername = HelperAppSettings.username,
                            recipientName = "InsuranceHub",
                            recipientUsername = "InsuranceHub",
                            policyHolder = HelperAppSettings.username,
                            policyNumber = polNo,
                            vehicleMake = vehMk,
                            communityCode = HelperAppSettings.community_code,
                            transactionType = "premium",
                            paymentMethod = "Over the Counter",

                        };
                        var p = pre;
                        var pel = p.ToString();
                        txndetails.paymentAmount = pel;

                        var json = JsonConvert.SerializeObject(txndetails);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
                        var logEndpoint = Helper.TransUrl;

                        var result = await client.PostAsync(logEndpoint, content);
                        string responsee = await result.Content.ReadAsStringAsync();

                        if (result.IsSuccessStatusCode)
                        {
                            var pro = JsonConvert.DeserializeObject<TransResModel>(responsee);
                            Helper.transResponse = pro;
                            TransHelper.referenceNumber = pro.transactionReferenceNumber;
                            TransHelper.transactionId = pro.transactionId;
                            TransHelper.inHubRefNum = pro.inHubRefNum;

                            //await Navigation.PushAsync(new PayWithCard());
                            TxnId.Text = TransHelper.transactionId;
                            BnkOpts.IsVisible = true;
                            spin2.IsVisible = false;
                        }
                    }
                }
                catch (Exception)
                {
                    spin2.IsVisible = false;
                    return;
                }
            }
            else if (BnkOpts.IsVisible == true)
            {
                //chvBUp.Source = "chevronDown.png";
                BnkOpts.IsVisible = false;
            }


        }

    }
}