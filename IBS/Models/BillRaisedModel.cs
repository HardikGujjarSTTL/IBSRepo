using IBS.Models.Reports;
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

        public string FromMonthName { get; set; }

        public string ToMonthName { get; set; }

        public string IncRites { get; set; }

        public string ActionType { get; set; }

        public string BPO_TYPE { get; set; }

        public string BPO_RLY { get; set; }

        public string BPO_ORGN { get; set; }

        public decimal? INSP_FEE { get; set; }

        public decimal? TAX { get; set; }

        public decimal? BILL_AMOUNT { get; set; }

        public decimal? NO_OF_BILLS { get; set; }

        public List<BillRaisedListModel> lstBill { get; set; }

        //Sector Wise Billing Report
        public string? SECTOR { get; set; }

        public decimal? SERVICE_TAX { get; set; }

        public decimal? EDU_CESS { get; set; }

        public decimal? SHE_CESS { get; set; }

        public decimal? SWACHH_BHARAT_CESS { get; set; }

        public decimal? KRISHI_KALYAN_CESS { get; set; }

        public decimal? CGST { get; set; }

        public decimal? SGST { get; set; }

        public decimal? IGST { get; set; }

        public List<BillSectorListModel> lstBillSector { get; set; }


    }
    public class BillRaisedListModel
    {
        public string BPO_TYPE { get; set; }

        public string BPO_RLY { get; set; }

        public string BPO_ORGN { get; set; }

        public decimal? INSP_FEE { get; set; }

        public decimal? TAX { get; set; }

        public decimal? BILL_AMOUNT { get; set; }

        public decimal? NO_OF_BILLS { get; set; }
    }

    public class BillSectorListModel
    {
        public string? SECTOR { get; set; }

        public decimal? INSP_FEE { get; set; }

        public decimal? SERVICE_TAX { get; set; }

        public decimal? EDU_CESS { get; set; }

        public decimal? SHE_CESS { get; set; }

        public decimal? SWACHH_BHARAT_CESS { get; set; }

        public decimal? KRISHI_KALYAN_CESS { get; set; }

        public decimal? CGST { get; set; }

        public decimal? SGST { get; set; }

        public decimal? IGST { get; set; }

        public decimal? BILL_AMOUNT { get; set; }

        public decimal? NO_OF_BILLS { get; set; }
    }
}
