using IBS.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class VenderCallRegisterModel
    {
        public string? VendCd { get; set; }

        public string CaseNo { get; set; } = null!;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallRecvDt { get; set; }

        public byte? CallInstallNo { get; set; }

        public short? CallSno { get; set; }

        public string? CallStatus { get; set; }

        public string? CallLetterNo { get; set; }

        public string? Remarks { get; set; }

        public string? PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        public string? IeSname { get; set; }

        public string? Vendor { get; set; }

        public string? CDATE { get; set; }
        public string? CDAY { get; set; }
        public string? CTYM { get; set; }


        //Details page data fetch related

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallLetterDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallMarkDt { get; set; }

        public int? IeCd { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? DtInspDesire { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallStatusDt { get; set; }

        public string? CallRemarkStatus { get; set; }

        public string? RegionCode { get; set; }
        public string? SetRegionCode { get; set; }

        public int? MfgCd { get; set; }

        public string? MfgPlace { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string? UpdateAllowed { get; set; }

        public string? CallCancelStatus { get; set; }

        public string? BkNo { get; set; }

        public string? SetNo { get; set; }

        public byte? CoCd { get; set; }

        public short? CallCancelCharges { get; set; }

        public string? OnlineCall { get; set; }

        public string? ItemRdso { get; set; }

        public string? VendRdso { get; set; }

        public DateTime? VendApprovalFr { get; set; }

        public DateTime? VendApprovalTo { get; set; }

        public string? StaggeredDp { get; set; }

        public string? LotDp1 { get; set; }

        public string? LotDp2 { get; set; }

        public string? Hologram { get; set; }

        public DateTime? ExpInspDt { get; set; }

        public string? RejCanCall { get; set; }

        public string? DepartmentCode { get; set; }

        public string? AutomaticCall { get; set; }

        public string? FinalOrStage { get; set; }

        public string? Bpo { get; set; }

        public string? RecipientGstinNo { get; set; }

        public string? NewVendor { get; set; }

        public bool? CountDt { get; set; }

        public string? IrfcFunded { get; set; }

        public string? FifoVoilateReason { get; set; }

        public decimal? RejCharges { get; set; }

        public string? LocalOrOuts { get; set; }

        public byte? ClusterCode { get; set; }

        public virtual T09Ie? IeCdNavigation { get; set; }

        public virtual T05Vendor? MfgCdNavigation { get; set; }

        public virtual T01Region? RegionCodeNavigation { get; set; }

        public virtual T19CallCancel? T19CallCancel { get; set; }

        public virtual ICollection<T20Ic> T20Ics { get; set; } = new List<T20Ic>();

        public virtual T44SuperSurprise? T44SuperSurprise { get; set; }

        public virtual ICollection<T47IeWorkPlan> T47IeWorkPlans { get; set; } = new List<T47IeWorkPlan>();

        public virtual ICollection<T50LabRegister> T50LabRegisters { get; set; } = new List<T50LabRegister>();


        //VENDOR_CALL_PO_DETAILS_VIEW Details related
        public string? PurchaserCd { get; set; }

        public string? Rly { get; set; }

        public string? L5noPo { get; set; }

        public string? RlyNonrly { get; set; }
    }
}
