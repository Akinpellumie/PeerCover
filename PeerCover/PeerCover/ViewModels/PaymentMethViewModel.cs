using System.Collections.Generic;
using System.Linq;

namespace PeerCover.ViewModels
{
    public class PaymentMethViewModel
    {
        public List<PaymentMethods> PaymentList { get; set; }

        public PaymentMethViewModel()
        {
            PaymentList = GetMethods().OrderBy(t => t.Value).ToList();
        }
        public List<PaymentMethods> GetMethods()
        {
            var payments = new List<PaymentMethods>()
        {
            new PaymentMethods(){key = 1, Value = "USSD"},
            new PaymentMethods(){key = 2, Value = "BANK TELLER"},
             new PaymentMethods(){key = 3, Value = "BANK TRANSFER"},
             new PaymentMethods(){key = 3, Value = "OTHERS"},

        };
            return payments;
        }

    }
    public class PaymentMethods
    {
        public int key { get; set; }
        public string Value { get; set; }
    }
}
