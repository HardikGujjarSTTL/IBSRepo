using System.Composition;

namespace IBS.Models
{
    public class BillingAdjustmentModel
    {
        public string Region_Code { get; set; } = null!;

        public string Adjusment_Yr_Mth { get; set; } = null!;

        public string AdjusmentPerMon { get; set; }

        public string AdjusmentPerYear { get; set; }

        public decimal? Adjustment_Amt { get; set; }

        public string? Remarks { get; set; }

        public string? User_Id { get; set; }

        public DateTime? Datetime { get; set; }

        public byte? Isdeleted { get; set; }

        public DateTime? Createddate { get; set; }

        public string? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public string? Updatedby { get; set; }

    }
}
