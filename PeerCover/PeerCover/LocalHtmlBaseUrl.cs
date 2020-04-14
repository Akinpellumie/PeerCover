using PeerCover.Views;
using Xamarin.Forms;

namespace PeerCover
{
    //public interface IBaseUrl { string Get(); }

    public class LocalHtmlBaseUrl : ContentPage
    {
        public LocalHtmlBaseUrl()
        {

            var browser = new WebView();
            var htmlSource = new HtmlWebViewSource();

            htmlSource.Html = @"<html>
                                <head>
                                <link rel=""stylesheet"" href=""default.css"">
                                </head>
                                <body>
                                <h1><a href=""local.html"">Click here to Pay</a></h3>
                                
                                </body>
                                </html>";

            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
            browser.Source = htmlSource;
            Content = browser;
        }

        protected override bool OnBackButtonPressed()
        {
            ContentPage jpg = new PayPremiums();

            Application.Current.MainPage = jpg;
            return true;
        }
    }
}