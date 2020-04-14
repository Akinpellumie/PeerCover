using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace PeerCover.ViewModels
{
    public class PaymentViewModel : BaseViewModel
    {
        [Obsolete]
        public PaymentViewModel()
        {
            Title = "Pay Premium";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://google.com.ng")));
            AstraWebCommand = new Command(() => Device.OpenUri(new Uri("https://astrapolaris.com")));
        }

        public ICommand OpenWebCommand { get; }
        public ICommand AstraWebCommand { get; }
    }

}