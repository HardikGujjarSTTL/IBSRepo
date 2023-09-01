using IBS.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class VendorModel
    {
        public int VendCd { get; set; }

        public string? VendName { get; set; }

        public string? VendAdd1 { get; set; }

        public string? VendAdd2 { get; set; }

        public int? VendCityCd { get; set; }

        public string? VendContactPer1 { get; set; }

        public string? VendContactTel1 { get; set; }

        public string? VendContactPer2 { get; set; }

        public string? VendContactTel2 { get; set; }

        public string? VendApproval { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? VendApprovalFr { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? VendApprovalTo { get; set; }

        public string? VendStatus { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? VendStatusDtFr { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? VendStatusDtTo { get; set; }

        public string? VendStatusDtFrST { get; set; }

        public string? VendStatusDtToST { get; set; }

        public string? VendRemarks { get; set; }

        public string? VendCdAlpha { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string? VendEmail { get; set; }

        public string? VendInspStopped { get; set; }

        public string? VendPwd { get; set; }

        public string? OnlineCallStatus { get; set; }


        public virtual ICollection<T13PoMaster> T13PoMasters { get; set; } = new List<T13PoMaster>();

        public virtual ICollection<T17CallRegister> T17CallRegisters { get; set; } = new List<T17CallRegister>();

        public virtual ICollection<T40ConsigneeComplaint> T40ConsigneeComplaints { get; set; } = new List<T40ConsigneeComplaint>();

        public virtual ICollection<T41NcMaster> T41NcMasters { get; set; } = new List<T41NcMaster>();

        public virtual ICollection<T47IeWorkPlan> T47IeWorkPlans { get; set; } = new List<T47IeWorkPlan>();

        public virtual ICollection<T80PoMaster> T80PoMasters { get; set; } = new List<T80PoMaster>();

        public virtual T03City? VendCityCdNavigation { get; set; }
    }
    public class VendorlistModel
    {
        public int VEND_CD { get; set; }
        public string? VEND_NAME { get; set; }
        public string? VEND_CITY_CD { get; set; }
        public string? VEND_ADD { get; set; }
        public string? VEND_CONT_NO { get; set; }
        public string? VEND_EMAIL { get; set; }
    }

}
