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

        public string MonthName { get; set; }

        public string YearName { get; set; }

        public string ActionType { get; set; }

        public string BPO_TYPE { get; set; }

        public string BPO_RLY { get; set; }

        public string BPO_ORGN { get; set; }

        public decimal? INSP_FEE { get; set; }

        public decimal? TAX { get; set; }

        public decimal? BILL_AMOUNT { get; set; }

        public decimal? NO_OF_BILLS { get; set; }

    }
    public class BillRaisedReportModel
    {
        [Required]
        public int FromMn { get; set; }
        [Required]
        public int FromYr { get; set; }

        public string BPO_TYPE { get; set; }

        public string BPO_RLY { get; set; }

        public string BPO_ORGN { get; set; }

        public int? INSP_FEE { get; set; }

        public int? TAX { get; set; }

        public int? BILL_AMOUNT { get; set; }

        public int? NO_OF_BILLS { get; set; }
    }
}
