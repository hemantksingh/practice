using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swinton.QuotesEngine
{
    public enum InsuranceType
    {
        SingleTrip, Annual
    }

    public class InsuranceBasis
    {
        public InsuranceType Type { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Destination { get; set; }
        public int TravelPeriod { get; set; }
    }
}
