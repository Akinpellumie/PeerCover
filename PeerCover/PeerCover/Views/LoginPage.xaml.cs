﻿using Newtonsoft.Json;
using PeerCover.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PeerCover.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = this;
            Init();
        }

        void Init()
        {
            App.StartCheckIfInternet(lbl_NoInternet, this);
        }
        public async void ForgotPassword(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ForgotPassword());
        }
        public async void SignUpClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SignUp());
        }

        public async void LoginClicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new PopLoader());
            try
            {
                if (string.IsNullOrEmpty(UsernameInput.Text) || string.IsNullOrEmpty(PasswordInput.Text))
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    await DisplayAlert("Alert", "Username or Password cannot be empty", "ok");
                    return;
                }
                indicator.IsRunning = false;
                indicator.IsVisible = false;
                User members = new User(UsernameInput.Text, PasswordInput.Text)
                {
                    username = UsernameInput.Text,
                    password = PasswordInput.Text,
                    mac = HelperAppSettings.AndroidId,
                    logOutOfDevice = false
                };

                if (members.CheckInformation())
                {


                    var json = JsonConvert.SerializeObject(members);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Token", HelperAppSettings.Token);
                    var loginEndpoint = Helper.LoginUrl;

                    var result = await client.PostAsync(loginEndpoint, content);

                    if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        await PopupNavigation.Instance.PopAsync(true);
                        await DisplayAlert("InHub", "Invalid Username or Password", "Ok");

                    }

                    else if (result.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        var pels = await DisplayAlert("Alert", UsernameInput.Text + " is logged in on another device. LogOut of the old device to continue!", "Confirm", "Decline");
                        if (pels == true)
                        {
                            User member = new User(UsernameInput.Text, PasswordInput.Text)
                            {
                                username = UsernameInput.Text,
                                password = PasswordInput.Text,
                                mac = HelperAppSettings.AndroidId,
                                logOutOfDevice = true
                            };
                            var json2 = JsonConvert.SerializeObject(member);
                            var contents = new StringContent(json2, Encoding.UTF8, "application/json");

                            HttpClient _client = new HttpClient();
                            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                            "Token", HelperAppSettings.Token);
                            var LoginEndpoint = Helper.LoginUrl;

                            var resultee = await client.PostAsync(LoginEndpoint, contents);

                            string responseeee = await resultee.Content.ReadAsStringAsync();
                            if (resultee.IsSuccessStatusCode)
                            {
                                var profile = JsonConvert.DeserializeObject<LoginProfileModel>(responseeee);

                                Helper.userprofile = profile;

                                HelperAppSettings.Token = profile.token;
                                HelperAppSettings.firstname = profile.firstname;
                                HelperAppSettings.lastname = profile.lastname;
                                HelperAppSettings.username = profile.username;
                                HelperAppSettings.email = profile.email;
                                HelperAppSettings.phonenumber = profile.phonenumber;
                                HelperAppSettings.community_code = profile.community_code;
                                HelperAppSettings.community_name = profile.community_name;
                                HelperAppSettings.id = profile.Id;
                                HelperAppSettings.Status = profile.Status;
                                HelperAppSettings.profile_img_url = profile.profile_img_url;
                                HelperAppSettings.priviledges = string.Join(",", profile.priviledges);
                                HelperAppSettings.capName = profile.capitalizedname;
                                HelperAppSettings.Name = profile.name;
                                HelperAppSettings.fcm_token = profile.fcm_token;
                                HelperAppSettings.account_number = profile.account_number;

                                if (HelperAppSettings.Status.Contains("Not Verified"))
                                {
                                    AppShellNewUser fpm1 = new AppShellNewUser();
                                    Application.Current.MainPage = fpm1;
                                    //await PopupNavigation.Instance.PopAsync(true);
                                }

                                else if (HelperAppSettings.Status.Contains("Verified"))
                                {
                                    AppShell fpm = new AppShell();
                                    //await PopupNavigation.Instance.PopAsync(true);

                                    if (profile.priviledges.Contains("ADMIN"))
                                    {
                                        Application.Current.MainPage = fpm;
                                        //await PopupNavigation.Instance.PopAsync(true);

                                    }
                                    else
                                    {
                                        Application.Current.MainPage = new AppShellUser();
                                    }
                                }
                                await PopupNavigation.Instance.PopAsync(true);

                            }
                        }
                    }

                    else
                    {
                        string responsee = await result.Content.ReadAsStringAsync();
                        if (responsee == null)
                        {
                            await PopupNavigation.Instance.PopAsync(true);
                            await DisplayAlert("InHub", "Check your connection and try Again", "Ok");

                        }
                        else if (result.IsSuccessStatusCode == false)
                        {
                            await PopupNavigation.Instance.PopAsync(true);
                            await DisplayAlert("Alert", "Incorrect Username or Password", "Retry");

                            return;
                        }

                        else if (result.IsSuccessStatusCode)
                        {
                            var profile = JsonConvert.DeserializeObject<LoginProfileModel>(responsee);

                            Helper.userprofile = profile;

                            HelperAppSettings.Token = profile.token;
                            HelperAppSettings.firstname = profile.firstname;
                            HelperAppSettings.lastname = profile.lastname;
                            HelperAppSettings.username = profile.username;
                            HelperAppSettings.email = profile.email;
                            HelperAppSettings.phonenumber = profile.phonenumber;
                            HelperAppSettings.community_code = profile.community_code;
                            HelperAppSettings.community_name = profile.community_name;
                            HelperAppSettings.id = profile.Id;
                            HelperAppSettings.Status = profile.Status;
                            HelperAppSettings.profile_img_url = profile.profile_img_url;
                            HelperAppSettings.priviledges = string.Join(",", profile.priviledges);
                            HelperAppSettings.capName = profile.capitalizedname;
                            HelperAppSettings.Name = profile.name;
                            HelperAppSettings.fcm_token = profile.fcm_token;
                            HelperAppSettings.account_number = profile.account_number;

                            if (HelperAppSettings.Status.Contains("Not Verified"))
                            {
                                AppShellNewUser fpm1 = new AppShellNewUser();
                                Application.Current.MainPage = fpm1;
                                //await PopupNavigation.Instance.PopAsync(true);
                            }

                            else if (HelperAppSettings.Status.Contains("Verified"))
                            {
                                AppShell fpm = new AppShell();
                                //await PopupNavigation.Instance.PopAsync(true);

                                if (profile.priviledges.Contains("ADMIN"))
                                {
                                    Application.Current.MainPage = fpm;
                                    //await PopupNavigation.Instance.PopAsync(true);

                                }
                                else
                                {
                                    Application.Current.MainPage = new AppShellUser();
                                }
                            }
                            await PopupNavigation.Instance.PopAsync(true);

                        }
                        else if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            indicator.IsRunning = false;
                            indicator.IsVisible = false;
                            await PopupNavigation.Instance.PopAsync(true);
                            await DisplayAlert("Login Failed", "Username or Password is Incorrect, Please Try Again!", "Ok");

                        }

                    }

                }
                else
                {
                    indicator.IsRunning = false;
                    indicator.IsVisible = false;
                    await PopupNavigation.Instance.PopAsync(true);
                    await DisplayAlert("Login", "Invalid Login details", "ok");

                }

            }
            catch (Exception)
            {
                return;
            }
        }

        public bool RememberMe
        {
            get => Preferences.Get(nameof(RememberMe), false);
            set
            {
                Preferences.Set(nameof(RememberMe), value);
                if (!value)
                {
                    Preferences.Set(nameof(Username), string.Empty);
                }
                OnPropertyChanged(nameof(RememberMe));
            }
        }

        string username = Preferences.Get(nameof(Username), string.Empty);
        public string Username
        {
            get => username;
            set
            {
                username = value;
                if (RememberMe)
                    Preferences.Set(nameof(Username), value);
                OnPropertyChanged(nameof(RememberMe));
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var password = await SecureStorage.GetAsync("token");
                PasswordInput.Text = password;
            }
            catch (Exception)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }
    }
}