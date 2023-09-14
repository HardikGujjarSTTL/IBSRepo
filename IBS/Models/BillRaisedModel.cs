using Microsoft.Build.Framework;

namespace IBS.Models
{
    public class BillRaisedModel
    {
        public string Title { get; set; }

        public string Region { get; set; } = null!;

        public string? BillSummary { get; set; }

        [Required]
        public int FromMn { get; set; }
        [Required]
        public int FromYr { get; set; }

        public int ToMn { get; set; }

        public int ToYr { get; set; }

        public string ActionType { get; set; }


        public string BPO_TYPE { get; set; }
        public string BPO_RLY { get; set; }
        public string BPO_ORGN { get; set; }
        public string INSP_FEE { get; set; }
        public string TAX { get; set; }
        public string BILL_AMOUNT { get; set; }
        public string NO_OF_BILLS { get; set; }

    }
}
