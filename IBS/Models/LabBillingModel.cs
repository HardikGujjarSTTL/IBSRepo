using System.Composition;

namespace IBS.Models
{
    public class LabBillingModel
    {
        public string? RegionCode { get; set; }

        public string? LabBillPer { get; set; }

        public string? LabBillPerMon { get; set; }

        public string? LabBillPerYear { get; set; }

        public decimal? LabExp { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public int Id { get; set; }

        public byte? Isdeleted { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public int? Updatedby { get; set; }

    }
}
