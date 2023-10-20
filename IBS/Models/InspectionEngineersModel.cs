using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class InspectionEngineersModel
    {
        public int IeCd { get; set; }

        [Display(Name = "IE Name")]
        [Required]
        public string? IeName { get; set; }

        [Display(Name = "IE Short Name")]
        [Required]
        public string? IeSname { get; set; }

        [Display(Name = "Employee No")]
        [Required]
        public string? IeEmpNo { get; set; }

        public int? IeDesig { get; set; }

        public int? IeSealNo { get; set; }

        [Display(Name = "Department")]
        [Required]
        public string? IeDepartment { get; set; }

        public string IeCityCd { get; set; }

        [Display(Name = "IE City")]
        [Required]
        public int IeCityId { get; set; }

        [Display(Name = "IE Phone No")]
        [Required]
        public string? IePhoneNo { get; set; }

        public byte? IeCoCd { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? IeJoinDt { get; set; }

        public string? IeStatus { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? IeStatusDt { get; set; }

        [Display(Name = "IE Posting")]
        [Required]
        public string? IeType { get; set; }

        [Display(Name = "IE Region")]
        [Required]
        public string? IeRegion { get; set; }

        public string? IePwd { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        [EmailAddress]
        public string? IeEmail { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? IeDob { get; set; }

        public int? AltIe { get; set; }

        public string? IeCallMarking { get; set; }

        public int? AltIeTwo { get; set; }

        public int? AltIeThree { get; set; }

        public int? ContAltIe { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallMarkingStoppingDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? DscExpiryDt { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }

        [Display(Name = "Cluster Name")]
        [Required]
        public int Cluster { get; set; }

        public string lstCluster { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallMarkingStartDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? InspectionStartDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? RepatriationDt { get; set; }

        public string? IeJobType { get; set; }

        public int ID { get; set; }

        public int ClusterID { get; set; }

        public List<InspectionEngineersListModel> lstInspectionEClusterModel { get; set; }
    }

    public partial class InspectionEngineersListModel
    {
        public int In_ID { get; set; }

        public int IeCd { get; set; }

        public string? IeName { get; set; }

        public string? IeDepartment { get; set; }

        public string lstCluster { get; set; }

        public int Cluster { get; set; }

        public string ClusterID { get; set; }
    }
}
