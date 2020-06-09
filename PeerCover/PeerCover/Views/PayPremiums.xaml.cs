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
        string txnId;
        string stats;
        string subId;
        public PayPremiums()
        {
            InitializeComponent();
            GetSubDetails();
            CheckInternet();
            //LoadSinPlan();
        }

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
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
            stats = UsersList.subscription[0].status;
            subId = UsersList.subscription[0].subscription_id;

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
            frmAstra.IsVisible = false;
            frmAstra2.IsVisible = true;
            frmDebCard.IsVisible = false;
            frmDebCard2.IsVisible = true;


            spinB.IsVisible = true;
                try
                {
                        if (SinSubPay.IsVisible == false)
                        {
                            await DisplayAlert("Oops", "Kindly Select a Subscription above", "Ok");
                            spinB.IsVisible = false;
                           
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
                                Task task = PopupNavigation.Instance.PushAsync(new PopUpload(NumId));
                                Task result = await Task.FromResult(task);
                                return;
                                //await CallAstraPay();

                            }
                            else if (resultee.Contains("Image not expired"))
                            {
                                await CallAstraPay();
                            }
                        }
                frmAstra.IsVisible = true;
                frmAstra2.IsVisible = false;
                frmDebCard.IsVisible = true;
                frmDebCard2.IsVisible = false;
                }
                catch (Exception)
                {
                    return;
                }

        }

        async 
        Task
CallAstraPay()
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
                        spinB.IsVisible = false;
                    }
                    else
                    {
                        await DisplayAlert("Oops!", "Please try again Later!", "Ok");
                        spinB.IsVisible = false;
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
            frmAstra.IsVisible = false;
            frmAstra2.IsVisible = true;
            frmDebCard.IsVisible = false;
            frmDebCard2.IsVisible = true;

            //if (stats.Contains("Paid"))
            //{
            //    await DisplayAlert("Oops!", "This subscription has already been paid for. ", "Ok");
            //    return;

            //}

            //else
            //{
            spin1B.IsVisible = true;
                try
                {
                    if (SinSubPay.IsVisible == false)
                    {
                        await DisplayAlert("Oops", "Kindly Select a Subscription above", "Ok");
                        spin1B.IsVisible = false;
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
                            spin1B.IsVisible = false;
                        }
                    }
                }
                catch (Exception)
                {
                    spin1B.IsVisible = false;
                    return;
                }
        }


        public async void OnBankTrans_Clicked(object sender, EventArgs e)
        {
            await ((Frame)sender).ScaleTo(0.8, length: 50, Easing.Linear);
            await Task.Delay(100);
            await ((Frame)sender).ScaleTo(1, length: 50, Easing.Linear);

            //if (stats.Contains("Paid"))
            //{
            //    await DisplayAlert("Oops!", "This subscription has already been paid for. ", "Ok");
            //    return;

            //}

            //else
            //{
                if (BnkOpts.IsVisible == false)
                {
                    BnkOpts.IsVisible = true;

                    //spin2.IsVisible = true;
                
                }
            else if (BnkOpts.IsVisible == true)
            {
                //chvBUp.Source = "chevronDown.png";
                BnkOpts.IsVisible = false;
            }
        }

        public async void PaymentClicked(object sender, EventArgs e)
        {
            Indic.IsVisible = true;
            try
            {
                if (SinSubPay.IsVisible == false)
                {
                    await DisplayAlert("Oops", "Kindly Select a Subscription above", "Ok");
                    //spin2.IsVisible = false;
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
                        Task task = PopupNavigation.Instance.PushAsync(new PopUpload(NumId));
                        Task result = await Task.FromResult(task);
                        return;
                        //await CallOverCounterPay();

                    }
                    else if (resultee.Contains("Image not expired"))
                    {
                        await CallOverCounterPay();
                    }
                }

                else
                {
                    
                }
            }
            catch (Exception)
            {
                spin2.IsVisible = false;
                return;
            }
           
        }

        async 
        Task
CallOverCounterPay()
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
                await Indic.ProgressTo(0.9, 950, Easing.SpringIn);
                var pro = JsonConvert.DeserializeObject<TransResModel>(responsee);
                Helper.transResponse = pro;
                TransHelper.referenceNumber = pro.transactionReferenceNumber;
                TransHelper.transactionId = pro.transactionId;
                TransHelper.inHubRefNum = pro.inHubRefNum;


                //await Navigation.PushAsync(new PayWithCard());
                //TxnId.Text = TransHelper.transactionId;
                txnId = TransHelper.transactionId;

                Indic.IsVisible = false;
                await PopupNavigation.Instance.PushAsync(new PopUpPay(txnId));
                spin2.IsVisible = false;
            }
        }
    }
}