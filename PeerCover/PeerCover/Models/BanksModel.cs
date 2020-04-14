using System;
using System.Collections.Generic;
using System.Text;

namespace PeerCover.Models
{
    public class BanksModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string accountName { get; set; }
        public string accountNumber {get; set;}
    }

    public class BankNameModels
    {
        public List<BanksModel> banks { get; set; }
    }
}
