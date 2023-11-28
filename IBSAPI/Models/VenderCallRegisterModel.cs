using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IBSAPI.Models
{
    public class VenderCallRegisterModel
    {
        public string? VendCd { get; set; }

        public string CaseNo { get; set; } = null!;
        public DateTime? CallRecvDt { get; set; }

        public byte? CallInstallNo { get; set; }

        public int? CallSno { get; set; }

        public string? CallStatus { get; set; }
        public string? CallLetterNo { get; set; }

        public string? Remarks { get; set; }

        public string? PoNo { get; set; }
        public DateTime? PoDt { get; set; }

        public string? IeSname { get; set; }

        public string? Vendor { get; set; }

        public string? CDATE { get; set; }
        public string? CDAY { get; set; }
        public string? CTYM { get; set; }

        public DateTime? CallLetterDt { get; set; }

        public DateTime? CallMarkDt { get; set; }

        public int? IeCd { get; set; }
        public DateTime? DtInspDesire { get; set; }
        public DateTime? CallStatusDt { get; set; }

        public string? CallRemarkStatus { get; set; }
        public string? RegionCode { get; set; }
        public string? SetRegionCode { get; set; }
        public int MfgCd { get; set; }

        public string? MfgPlace { get; set; }

        public string? UserId { get; set; }

        //public DateTime? Datetime { get; set; }

        public string? UpdateAllowed { get; set; }

        public string? CallCancelStatus { get; set; }

        public string? BkNo { get; set; }

        public string? SetNo { get; set; }

        public byte? CoCd { get; set; }

        public short? CallCancelCharges { get; set; }

        public string? OnlineCall { get; set; }

        public string? ItemRdso { get; set; }

        public string? VendRdso { get; set; }

        //public DateTime? VendApprovalFr { get; set; }

        //public DateTime? VendApprovalTo { get; set; }

        public string? StaggeredDp { get; set; }

        public string? LotDp1 { get; set; }

        public string? LotDp2 { get; set; }

        public string? Hologram { get; set; }

        //public DateTime? ExpInspDt { get; set; }

        public string? RejCanCall { get; set; }

        public string? DepartmentCode { get; set; }

        public string? AutomaticCall { get; set; }

        public string? FinalOrStage { get; set; }

        public string? Bpo { get; set; }

        public string? ItemCd { get; set; }

        public string? RecipientGstinNo { get; set; }

        public string? NewVendor { get; set; }

        public bool? CountDt { get; set; }

        public string? IrfcFunded { get; set; }

        public string? FifoVoilateReason { get; set; }

        public decimal? RejCharges { get; set; }

        public string? LocalOrOuts { get; set; }

        public int? ClusterCode { get; set; }

        public string? PurchaserCd { get; set; }

        public string? Rly { get; set; }

        public string? L5noPo { get; set; }

        public string? RlyNonrly { get; set; }

        public string? VendAdd1 { get; set; }

        public string? VendContactPer1 { get; set; }

        public string? VendContactTel1 { get; set; }

        public string? VendStatus { get; set; }

        public DateTime? VendStatusDtFr { get; set; }

        public DateTime? VendStatusDtTo { get; set; }

        public string? VendEmail { get; set; }

        public string? Status { get; set; }

        public int ItemSrnoPo { get; set; }

        public string? ItemDescPo { get; set; }

        public decimal? QtyOrdered { get; set; }

        public decimal? CumQtyPrevOffered { get; set; }

        public decimal? CumQtyPrevPassed { get; set; }

        public decimal? QtyToInsp { get; set; }

        public decimal? QtyPassed { get; set; }

        public decimal? QtyRejected { get; set; }

        public decimal? QtyDue { get; set; }

        public string? Consignee { get; set; }

        public int ConsigneeCd { get; set; }

        //public DateTime? DelvDt { get; set; }

        public string? DelvDate { get; set; }

        public string ActionType { get; set; }

        public string FOS { get; set; }

        public bool IsNewVender { get; set; }
        public string hdnIsNewVender { get; set; }

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

    }

    public class RequestVenderCallRegisterModel
    {
        public string CaseNo { get; set; }
        public string CallSno { get; set; } 
        public DateTime CallRecvDt { get; set; }
    }

    public class RequestUpdateItemModel
    {
        public string UserID { get; set; }
        public string CaseNo { get; set; }
        public DateTime CallRecvDt { get; set; }
        public int CallSno { get; set; }
        public int ItemSrnoPo { get; set; }
        public string ItemDescPo { get; set; }
        public int QtyDue { get; set; }
        public int QtyToInsp { get; set; }
        public int QtyOrdered { get; set; }
        public int ConsigneeCd { get; set; }
    }

    public class RequestUpdateManufacturerDetailsModel
    {
        public int MfgCd { get; set; }
        public string? VendContactPer { get; set; }
        public string? VendContactTel { get; set; }
        public string? VendEmail { get; set; }
    }

    public class RequestCaseDetailsforvendorModel
    {
        public string CaseNo { get; set; }
        public int UserID { get; set; }
        public DateTime CallRecvDt { get; set; }
        public string CallStage { get; set; }
    }
}