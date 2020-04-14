using Xamarin.Forms;

namespace PeerCover
{
    public class ShowHidePassEffect : RoutingEffect
    {
        public string Entry { get; set; }
        public ShowHidePassEffect() : base("Xamarin.ShowHidePassEffect") { }
    }
}