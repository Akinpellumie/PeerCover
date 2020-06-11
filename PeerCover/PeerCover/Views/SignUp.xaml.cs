using Newtonsoft.Json;
using PeerCover.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PeerCover.ViewModels;
using Rg.Plugins.Popup.Services;
using PeerCover.Utils;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            InitializeComponent();
             BindingContext = this;
            CheckInternet();
        }
        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }
        public async void CreateUserBtn_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }
            await PopupNavigation.Instance.PushAsync(new PopLoader());
            try
            {

                if (string.IsNullOrEmpty(FTInput.Text) || string.IsNullOrEmpty(USNInput.Text) || string.IsNullOrEmpty(PWDInput.Text) || string.IsNullOrEmpty(EAInput.Text) || string.IsNullOrEmpty(CCInput.Text) || string.IsNullOrEmpty(LTInput.Text) || string.IsNullOrEmpty(PNInput.Text))
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    await DisplayAlert("Alert", "Input fields cannot be empty", "Ok");
                    return;
                }
                AddMemberModel members = new AddMemberModel(FTInput.Text, LTInput.Text, USNInput.Text, EAInput.Text, PNInput.Text, CCInput.Text, PWDInput.Text)
                {
                    firstname = FTInput.Text.Trim(),
                    username = USNInput.Text.Trim(),
                    lastname = LTInput.Text.Trim(),
                    email = EAInput.Text.Trim(),
                    phonenumber = PNInput.Text.Trim(),
                    communityCode = CCInput.Text.Trim(),
                    password = PWDInput.Text.Trim()

                };
                if (members.CheckInformation())

                {
                    var client = new HttpClient();

                    var json = JsonConvert.SerializeObject(members);
                    HttpContent result = new StringContent(json);
                    result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(Helper.SignUpUrl, result);

                    if (response.IsSuccessStatusCode)
                    {
                        //await DisplayAlert("InHub", "Registration Succesful", "Ok");
                        await Navigation.PushModalAsync(new SignUpConfirmation());
                        await PopupNavigation.Instance.PopAsync(true);
                        FTInput.Text = "";
                        LTInput.Text = "";
                        USNInput.Text = "";
                        EAInput.Text = "";
                        PNInput.Text = "";
                        CCInput.Text = "";
                        PWDInput.Text = "";

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
            }
            catch (Exception)
            {
                return;
            }
        }

        public async void Username_Unfocused(object sender, FocusEventArgs e)
        {
            //Loader.IsVisible = true;
            try
            {

                var url = Helper.UsernameAvailability + USNInput.Text;
                HttpClient client = new HttpClient();
                var result = await client.GetStringAsync(url);
                //var UsersList = JsonConvert.DeserializeObject<MembersListModel>(result);
                if (result.Contains("Username is available"))
                {
                    ///Loader.IsVisible = false;
                    await DisplayAlert("Success", "Username is Available", "Ok");
                }
                else if (result.Contains("Username is not available"))
                {
                    //Loader.IsVisible = false;
                    await DisplayAlert("Attention", "Username already Exist", "Ok");
                    LblUser.TextColor = Color.Red;
                    LblUsn.Text = "Username already exist. Try another one*";
                    USNInput.TextColor = Color.Red;
                    //USNInput = Color.Red;
                }
            }
            catch (Exception)
            {
                return;
            }

        }

        public void BackToLoginPressed(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
        public void Input2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                if (e.NewTextValue.Length >= 1)
                {
                    //UsnFrm.BorderColor = Color.Accent;
                    USNInput.TextColor = Color.Default;
                    LblUser.TextColor = Color.Default;
                    LblUsn.Text = "";
                }
            }
            catch (Exception)
            {
                return;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitStates();
        }
            void InitStates()
        {
            var stateGroup = new VisualStateGroup
            {
                Name = "StrengthStates",
                TargetType = typeof(Label)
            };

            stateGroup.States.Add(CreateState("Blank", "", Color.White));
            stateGroup.States.Add(CreateState("VeryWeak", "Very Weak", Color.Red));
            stateGroup.States.Add(CreateState("Weak", "Weak", Color.Orange));
            stateGroup.States.Add(CreateState("Medium", "Good", Color.GreenYellow));
            stateGroup.States.Add(CreateState("String", "Strong", Color.LightGreen));
            stateGroup.States.Add(CreateState("VeryStrong", "Very Strong", Color.LightGreen));

            VisualStateManager.SetVisualStateGroups(this.StrengthIndicator, new VisualStateGroupList { stateGroup });

        }


        void Handle_TextChanged(object sender, TextChangedEventArgs e)
        {
            var strength = PasswordAdvisor.CheckStrength(e.NewTextValue);
            var strengthName = Enum.GetName(typeof(PasswordScore), strength);
            VisualStateManager.GoToState(this.StrengthIndicator, strengthName);
        }


        string strength;

        public string Strength
        {
            get => strength;
            set
            {
                strength = value;
                OnPropertyChanged(nameof(Strength));
            }
        }

        static VisualState CreateState(string strength, string text, Color color)
        {
            var textSetter = new Setter { Value = text, Property = Label.TextProperty };
            var colorSetter = new Setter { Value = color, Property = Label.TextColorProperty };

            return new VisualState
            {
                Name = strength,
                TargetType = typeof(Label),
                Setters = { textSetter, colorSetter }
            };
        }

    }
}