using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPlan : ContentPage
    {
        public AboutPlan()
        {
            InitializeComponent();
        }
        public async void getStartedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPlan());
        }
    }
}