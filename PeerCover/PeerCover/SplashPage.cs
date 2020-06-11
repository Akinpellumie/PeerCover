using PeerCover.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PeerCover
{
    public class SplashPage : ContentPage
    {
        Image splashImage;

        public SplashPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var sub = new AbsoluteLayout();
            splashImage = new Image
            {
                Source = "AppLogo.png",
                WidthRequest = 350,
                HeightRequest = 350,
            };
            AbsoluteLayout.SetLayoutFlags(splashImage,
                AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(splashImage,
                new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            sub.Children.Add(splashImage);
            this.BackgroundColor = Color.Accent;
            this.Content = sub;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            splashImage.Opacity = 0;
            await splashImage.FadeTo(1, 4000);
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
