using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class BPOWiseOutstandingBillsModel
    {
        public string FromDt { get; set; }

        public string ToDt { get; set; }

        public string BpoCd { get; set; }
        public string BpoType { get; set; }
        public string BpoRly { get; set; }
        public string BpoRegion { get; set; }
        public string Railway { get; set; }
        public string PSU { get; set; }
        public string StateGovt { get; set; }
        public string ForeignRailways { get; set; }
        public string PrivateSector { get; set; }
        public string TypeofOutStandingBills { get; set; }
        public string Region { get; set; }

    }
}
