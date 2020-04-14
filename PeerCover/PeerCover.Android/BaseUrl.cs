using System;
using Xamarin.Forms;
using PeerCover;
using PeerCover.Droid;

[assembly: Dependency(typeof(BaseUrl))]
namespace PeerCover.Droid
{
    public class BaseUrl : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/";
        }
    }
}