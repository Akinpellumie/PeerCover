using System.Collections.Generic;
using System.Linq;

namespace PeerCover.ViewModels
{
    public class CausesViewModel
    {
        public List<Causes> CausesList { get; set; }

        public CausesViewModel()
        {
            CausesList = GetStatus().OrderBy(t => t.Value).ToList();
        }
        public List<Causes> GetStatus()
        {
            var causes = new List<Causes>()
        {
            new Causes(){key = 1, Value = "Accident"},
            new Causes(){key = 2, Value = "Fire Damage"},
            new Causes(){key = 3, Value = "Theft"}

        };
            return causes;
        }

    }
    public class Causes
    {
        public int key { get; set; }
        public string Value { get; set; }
    }
}
