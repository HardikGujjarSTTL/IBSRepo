using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class VendorClusterModel
    {
        public int VendorCode { get; set; }

        public string? VendorName { get; set; }

        public string? DepartmentCode { get; set; }

        [Display(Name = "Department Name")]
        [Required]
        public string? DepartmentName { get; set; }

        [Display(Name = "Cluster Name")]
        [Required]
        public int? ClusterCode { get; set; }

        public string? ClusterName { get; set; }

        public string? GeographicalPartition { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }

        public bool IsNew { get; set; } = true;

        public string? VendFullName { get; set; }

        public string? VendAdd1 { get; set; }
    }

    public class VendorDetailsModel
    {
        public int VendCd { get; set; }

        public string? VendName { get; set; }

        public string? VendAdd1 { get; set; }

        public string? Location { get; set; }

        public string? City { get; set; }

        public string? VendStatus { get; set; }

        public DateTime? VendStatusDtFr { get; set; }

        public string Display_VendStatusDtFr { get { return this.VendStatusDtFr != null ? Common.ConvertDateFormat(this.VendStatusDtFr.Value) : ""; } }

        public DateTime? VendStatusDtTo { get; set; }

        public string Display_VendStatusDtTo { get { return this.VendStatusDtTo != null ? Common.ConvertDateFormat(this.VendStatusDtTo.Value) : ""; } }
    }
}
