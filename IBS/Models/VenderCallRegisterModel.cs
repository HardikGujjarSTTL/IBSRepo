using IBS.DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
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

        public int? CallSno { get; set; }

        public string? CallStatus { get; set; }

        [Display(Name = "Call Letter Number")]
        [Required]
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

        public int MfgCd { get; set; }

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

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? VendApprovalFr { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? VendApprovalTo { get; set; }

        public string? StaggeredDp { get; set; }

        public string? LotDp1 { get; set; }

        public string? LotDp2 { get; set; }

        public string? Hologram { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? ExpInspDt { get; set; }

        public string? RejCanCall { get; set; }

        public string? DepartmentCode { get; set; }

        public string? AutomaticCall { get; set; }

        public string? FinalOrStage { get; set; }

        public string? Bpo { get; set; }

        public string? ItemCd { get; set; }

        [Display(Name = "GSTIN No")]
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Invalid GSTIN No.")]
        [Required]
        public string? RecipientGstinNo { get; set; }

        public string? NewVendor { get; set; }

        public bool? CountDt { get; set; }

        public string? IrfcFunded { get; set; }

        public string? FifoVoilateReason { get; set; }

        public decimal? RejCharges { get; set; }

        public string? LocalOrOuts { get; set; }

        public int? ClusterCode { get; set; }

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

        //T05_VENDOR
        public string? VendAdd1 { get; set; }

        public string? VendContactPer1 { get; set; }

        public string? VendContactTel1 { get; set; }

        public string? VendStatus { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? VendStatusDtFr { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? VendStatusDtTo { get; set; }

        public string? VendEmail { get; set; }

        //VENDER_CALL_REGISTER_ITEM_VIEW
        public string? Status { get; set; }

        public int ItemSrnoPo { get; set; }

        public string? ItemDescPo { get; set; }

        public decimal? QtyOrdered { get; set; }

        public decimal? CumQtyPrevOffered { get; set; }

        public decimal? CumQtyPrevPassed { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Qty Off Now must be a number")]
        public decimal? QtyToInsp { get; set; }

        public decimal? QtyPassed { get; set; }

        public decimal? QtyRejected { get; set; }

        public decimal? QtyDue { get; set; }

        public string? Consignee { get; set; }

        public int ConsigneeCd { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? DelvDt { get; set; }

        public string? DelvDate { get; set; }

        public string ActionType { get; set; }

        public string FOS { get; set; }

        public bool IsNewVender { get; set; }
        public string hdnIsNewVender { get; set; }

        //public string IsFinalizedStatus { get; set; }
        public bool IsFinalizedStatus { get; set; }

        public string? Createdby { get; set; }

        public int callval { get; set; }

        public decimal wMat_value { get; set; }

        public int desire_dt { get; set; }

        public string CaseNoNoFound { get; set; }

        public int e_status { get; set; }

        public string IE_name { get; set; }

        public string VendInspStopped { get; set; }

        public string InspectingAgency { get; set; }

        public string VendRemarks { get; set; }

        public string CallStage { get; set; }

        public string PoOrLetter { get; set; }

        public string PoSource { get; set; }

        public string ImmsRlyCd { get; set; }

        public int PendingCharges { get; set; }

        public int MaxCount { get; set; }

        public string dp { get; set; }

        public string OnlineCallStatus { get; set; }

        public string OfflineCallStatus { get; set; }

        public string Region { get; set; }

        public string MsgStatus { get; set; }

        public string CHKRejCan { get; set; }

        public List<UnInspectedQtylstModel> lstUnInspectedQty { get;set; }

        public string FrIeName { get; set; }
        
        public string ToIeName { get; set; }
        
        public string UserName { get; set; }
        
        public DateTime? RemInitDatetime { get; set; }

    }

    public class VendorCallRegPrintReport
    {
        public string? PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        public string? MfgName { get; set; }

        public string? MfgAdd { get; set; }

        public string? Purchaser { get; set; }

        public string? Consignee { get; set; }

        public string CaseNo { get; set; } = null!;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? CallRecvDt { get; set; }

        public int CallSno { get; set; }

        public string? CallLetterNo { get; set; }

        public string? CallLetterDt { get; set; }

        public byte? CallInstallNo { get; set; }

        public string? OnlineCall { get; set; }

        public string? FinalOrStage { get; set; }

        public string? Remarks { get; set; }

        public string? ItemRdso { get; set; }

        public string? VendRdso { get; set; }

        public string? VendAppFr { get; set; }

        public string? VendAppTo { get; set; }

        public string? StagDp { get; set; }

        public string? LotDp1 { get; set; }

        public string? LotDp2 { get; set; }

        public string? IeName { get; set; }

        public string? ItemDescPo { get; set; }

        public decimal? QtyOrdered { get; set; }

        public decimal? QtyToInsp { get; set; }

        public decimal? CumQtyPrevOffered { get; set; }

        public decimal? CumQtyPrevPassed { get; set; }

        public string? VendContactPer1 { get; set; }

        public string? VendContactTel1 { get; set; }

        public string? VendEmail { get; set; }

        public string? Bpo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? DelvDt { get; set; }

        public string? ItemCd { get; set; }

        public string? IrfcFunded { get; set; }

        public int? VendCd { get; set; }

        public string? VendName { get; set; }

        public string? VendAdd { get; set; }

        public string? VendEmailPO { get; set; }

        public string? VendContactPerPO1 { get; set; }

        public string? VendContactTelPO1 { get; set; }

        public string? Source { get; set; }

        public string? VendPOEmail { get; set; }

        public string RegionCode { get; set; }

        public List<PrintCallLetterModel> lstPrintCallLetter { get; set; }
    }


    public class PrintCallLetterModel
    {
        public string CaseNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallRecvDt { get; set; }

        public int? CallSno { get; set; }

        public string? Consignee { get; set; }

        public string? ItemDescPo { get; set; }

        public decimal? QtyOrdered { get; set; }

        public decimal? CumQtyPrevOffered { get; set; }

        public decimal? CumQtyPrevPassed { get; set; }

        public decimal? QtyToInsp { get; set; }

        public decimal? QtyPassed { get; set; }

        public decimal? QtyRejected { get; set; }

        public decimal? QtyDue { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? DelvDt { get; set; }

        public string? Bpo { get; set; }

        public string? ItemCd { get; set; }
    }

    public class VenderCallCancellationModel
    {
        public string CaseNo { get; set; } = null!;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallRecvDt { get; set; }

        public short? CallSno { get; set; }

        public string? PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        public string? IeSname { get; set; }

        public string? Vendor { get; set; }

        public string CallCancelStatus { get; set; }

        public int chk1 { get; set; }

        public int chk2 { get; set; }

        public int chk3 { get; set; }

        public int chk4 { get; set; }

        public int chk5 { get; set; }

        public int chk6 { get; set; }

        public int chk7 { get; set; }

        public int chk8 { get; set; }

        public int chk9 { get; set; }

        public int chk10 { get; set; }

        public int chk11 { get; set; }

        public int chk12 { get; set; }

        public bool chk_1 { get; set; }

        public bool chk_2 { get; set; }

        public bool chk_3 { get; set; }

        public bool chk_4 { get; set; }

        public bool chk_5 { get; set; }

        public bool chk_6 { get; set; }

        public bool chk_7 { get; set; }

        public bool chk_8 { get; set; }

        public bool chk_9 { get; set; }

        public bool chk_10 { get; set; }

        public bool chk_11 { get; set; }

        public bool chk_12 { get; set; }

        public string DocRec { get; set; }

        public string Cdesc { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CancelDt { get; set; }

        public string Createdby { get; set; }

        public string CreatedDate { get; set; }

        public string Updatedby { get; set; }

        public string UpdatedDate { get; set; }

        public string ActionType { get; set; }

        public bool[] chkItems { get; set; }

    }

    public class VenderCallStatusModel
    {
        public string CaseNo { get; set; } = null!;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallRecvDt { get; set; }

        public int? CallSno { get; set; }

        public string? VendName { get; set; }

        public bool[] chkItems { get; set; }

        public string? Consignee { get; set; }

        public string ConsigneeFirm { get; set; }

        public string RejectionCharge { get; set; }

        public string LocalOutstation { get; set; }

        public List<SelectListItem> ConsigneeFirmList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> CallRlyFirmList { get; set; } = new List<SelectListItem>();

        public string? ItemDescPo { get; set; }

        public string? CancellationDescription { get; set; }

        public string? ReasonFIFO { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallMarkDt { get; set; }

        public string? IeName { get; set; }
        [Phone]
        public string? IePhoneNo { get; set; }

        public string? PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        [Required(ErrorMessage = "Please Enter Book No.")]
        public string? BkNo { get; set; }
        [Required(ErrorMessage = "Please Enter Set No.")]
        public string? SetNo { get; set; }

        public string? CallStatus1 { get; set; }

        public string? CallStatus { get; set; }

        public string? CallCancelStatus { get; set; }

        public string? CallCancelCharges { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallStatusDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? DesireDt { get; set; }

        public string? UpdateAllowed { get; set; }

        public string? MfgPers { get; set; }

        public string? MfgPhone { get; set; }

        public decimal? Count { get; set; }

        public byte ItemSrnoPo { get; set; }

        public string Createdby { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string Updatedby { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string ActionType { get; set; }

        public string? IeCd { get; set; }
        public int? IssueToIecd { get; set; }

        public string UserId { get; set; }

        public string? MaterialValue { get; set; }

        public string? CallListByRly { get; set; }
        public string? ChkFIFO { get; set; }
        public string? AlertMsg { get; set; }
        public string? Chk1 { get; set; }
        public string? Chk2 { get; set; }
        public string? Chk3 { get; set; }
        public string? Chk4 { get; set; }
        public string? Chk5 { get; set; }
        public string? Chk6 { get; set; }
        public string? Chk7 { get; set; }
        public string? Chk8 { get; set; }
        public string? Chk9 { get; set; }
        public string? Chk10 { get; set; }
        public string? Chk11 { get; set; }
        public string? Chk12 { get; set; }
        public string Hologram { get; set; }
        public string Remarks { get; set; }
        public string Remarkslbl { get; set; }
        public DateTime CallLetterDt { get; set; }
        [Required(ErrorMessage = "Please Enter Book No.")]
        public string? DocBkNo { get; set; }
        [Required(ErrorMessage = "Please Enter Set No.")]
        public string? DocSetNo { get; set; }
    }

    public class VendrorCallDetailsModel
    {
        public string CaseNo { get; set; } = null!;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallRecvDt { get; set; }

        public short? CallSno { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallMarkDt { get; set; }

        public string? IeName { get; set; }
        [Phone]
        public string? IePhoneNo { get; set; }

        public string? PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        public int ItemSrNoPo { get; set; }

        [Required(ErrorMessage = "Please enter a Item Desc")]
        public string? ItemDescPo { get; set; }

        [Required(ErrorMessage = "Please enter a Qty Ordered")]
        public decimal? QtyOrdered { get; set; }

        [Required(ErrorMessage = "Please enter a Cum Qty Prev Offered")]
        public decimal? CumQtyPrevOffered { get; set; }

        [Required(ErrorMessage = "Please enter a Cum Qty Prev Passed")]
        public decimal? CumQtyPrevPassed { get; set; }

        [Required(ErrorMessage = "Please enter a Qty To Insp")]
        public decimal? QtyToInsp { get; set; }

        public decimal? QtyPassed { get; set; }

        public decimal? QtyRejected { get; set; }

        public decimal? QtyDue { get; set; }

        public string? Consignee { get; set; }

        public string? Status { get; set; }

        public int? CallInstallNo { get; set; }

        public string VendCd { get; set; }

        public string CallLetterNo { get; set; }

        public string? CallStatus { get; set; }

        public string? Remarks { get; set; }

        public string? IESName { get; set; }

        public string Createdby { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string Updatedby { get; set; }

        public DateTime? UpdatedDate { get; set; }


    }

    public class UnInspectedQtylstModel
    {
        public string CASE_NO { get; set; }

        public string ITEM_SRNO { get; set; }

        public string ITEM_DESC { get; set; }

        public decimal QTY { get; set; }

        public DateTime DELV_DATE { get; set; }

        public decimal? PASSED { get; set; }

        public decimal? REJECTED { get; set; }


    }

    public class ImageFiles
    {
        public string File_1 { get; set; }
        public string File_2 { get; set; }
        public string File_3 { get; set; }
        public string File_4 { get; set; }
        public string File_5 { get; set; }
        public string File_6 { get; set; }
        public string File_7 { get; set; }
        public string File_8 { get; set; }
        public string File_9 { get; set; }
        public string File_10 { get; set; }
    }
}