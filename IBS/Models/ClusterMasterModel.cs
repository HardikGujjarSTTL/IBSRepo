using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class ClusterMasterModel
    {
        public int ClusterCode { get; set; }

        [Display(Name = "Cluster Name")]    
        [Required]
        public string? ClusterName { get; set; }

        [Display(Name = "Geographical Area")]
        [Required]
        public string? GeographicalPartition { get; set; }

        [Display(Name = "Department Name")]
        [Required]
        public string? DepartmentName { get; set; }

        [Display(Name = "Region Name")]
        [Required]
        public string? RegionCode { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        [Display(Name = "HQ Area")]
        [Required]
        public string? HqArea { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }

        public bool IsNew { get; set; } = true;
    }
}
