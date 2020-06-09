using PeerCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        public NotificationPage()
        {
            InitializeComponent();
            //GetNotifications();
            GetUnreadNotifications();
            CheckInternet();
            NewMsgList.RefreshCommand = new Command(() => {
                //Do your stuff.    
                GetUnreadNotifications();
                NewMsgList.IsRefreshing = false;
            });
            MsgList.RefreshCommand = new Command(() => {
                //Do your stuff.    
                GetNotifications();
                MsgList.IsRefreshing = false;
            });
        }

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }

        public async void GetNotifications()

        {
            indicator.IsRunning = true;
            indicator.IsVisible = true;


            HttpClient client = new HttpClient();
            var dashboardEndpoint = Helper.GetNotificationsUrl + HelperAppSettings.username + Helper.msgUnreadFilter;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(dashboardEndpoint);
            var NotsList = JsonConvert.DeserializeObject<NotificationsModel>(result);
            MsgList.ItemsSource = NotsList.notifications;

            if (result.Contains("id"))
            {
                Frm2.IsVisible = true;
                Frm2B.IsVisible = false;
            }
            else
            {
                Frm2.IsVisible = false;
                Frm2B.IsVisible = true;
            }

            //List<NotificationsModel> messages = new List<NotificationsModel>();

            //foreach (NotificationsModel message in messages)
            //{

            //}


            //if (NotsList.notifications.)
            //{

            //}

            indicator.IsRunning = false;
            indicator.IsVisible = false;
        }

        public async void GetUnreadNotifications()

        {
            indicator.IsRunning = true;
            indicator.IsVisible = true;


            HttpClient client = new HttpClient();
            var dashboardEndpoint = Helper.GetNotificationsUrl + HelperAppSettings.username + Helper.msgReadFilter;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
            var result = await client.GetStringAsync(dashboardEndpoint);
            var NotsList = JsonConvert.DeserializeObject<NotificationsModel>(result);

            NewMsgList.ItemsSource = NotsList.notifications;
            if (result.Contains("id"))
            {
                Frm1.IsVisible = true;
                Frm1B.IsVisible = false;
            }
            else
            {
                Frm1.IsVisible = false;
                Frm1B.IsVisible = true;
            }
            indicator.IsRunning = false;
            indicator.IsVisible = false;
        }

        public void NewBtn_Clicked(object sender, EventArgs e)
        {
            GetUnreadNotifications();
            StartAnimation();
            newNotStack.IsVisible = true;
            oldNotStack.IsVisible = false;
        }

        private async void StartAnimation()
        {
            await Task.Delay(200);
            await NewBtn.FadeTo(0, 250);
            await Task.Delay(200);
            await NewBtn.FadeTo(1, 250);
        }

        private async void StartOldAnimation()
        {
            await Task.Delay(200);
            await OldBtn.FadeTo(0, 250);
            await Task.Delay(200);
            await OldBtn.FadeTo(1, 250);
        }

        public void onNewBtnPressed(object sender, EventArgs e)
        {
            NewBtn.BackgroundColor = Color.Accent;
            OldBtn.BackgroundColor = Color.Gray;
        }
        public void onOldBtnPressed(object sender, EventArgs e)
        {
            OldBtn.BackgroundColor = Color.Accent;
            NewBtn.BackgroundColor = Color.Gray;
        }
        public void OldBtn_Clicked(object sender, EventArgs e)
        {
            GetNotifications();
            StartOldAnimation();
            newNotStack.IsVisible = false;
            oldNotStack.IsVisible = true;
        }


        async void ViewNotsTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedMsg = e.Item as Models.NotifyModel;
            await Shell.Current.Navigation.PushAsync(new ViewNotifications(selectedMsg.id));

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //GetNotifications();
            //GetUnreadNotifications();
        }
    }
}