using System.Collections.Generic;
using System.Linq;

namespace PeerCover.ViewModels
{
    public class GenderViewModel
    {
        public List<Genders> GenderList { get; set; }

        public GenderViewModel()
        {
            GenderList = GetBanks().OrderBy(t => t.Value).ToList();
        }
        public List<Genders> GetBanks()
        {
            var banks = new List<Genders>()
        {
            new Genders(){key = 1, Value = "Male"},
            new Genders(){key = 2, Value = "Female"},
             new Genders(){key = 3, Value = "Others"},

        };
            return banks;
        }

    }
    public class Genders
    {
        public int key { get; set; }
        public string Value { get; set; }
    }
}
