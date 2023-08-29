using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace IBS.Models
{
    public class ConsigneeComplaints
    {
        [Key]
        public string ComplaintId { get; set; }
        public string? CASE_NO { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string PO_NO { get; set; }
        public DateTime? PO_DT { get; set; }
        public string FormattedPO_DT { get; set; }
        public string FormattedComplaintDate { get; set; }
        public string VEND_NAME { get; set; }
        public int VendCd { get; set; }
        public string Consignee { get; set; }
        public string ie_name { get; set; }
        public int ConsigneeCd { get; set; }
        public int ie_cd { get; set; }
        public int ie_co_cd { get; set; }
        public string IC_NO { get; set; }
        public DateTime? IC_DATE { get; set; }
        public string FormattedIC_DATE { get; set; }
        public DateTime? ComplaintDate { get; set; }
        public string? RejMemoNo { get; set; }
        public string? Railway { get; set; }
        public DateTime? RejMemoDt { get; set; }
        public DateTime? JIDate { get; set; }
        public DateTime? JIDateConclusion { get; set; }
        public DateTime? JiFixDt { get; set; }
        public string? InspRegion { get; set; }
        public string? JIDecided { get; set; }
        public string? JIInspRegion { get; set; }
        public string? unitofM { get; set; }
        public string? uom_cd { get; set; }
        public string? COMP_RECV_REGION { get; set; }
        public string? InspER { get; set; }
        public string? UserId { get; set; }
        public string? ItemDesc { get; set; }
        public decimal? QtyOffered { get; set; }
        public decimal? QtyRejected { get; set; }
        public decimal? Rate { get; set; }
        public decimal? rejectionValue { get; set; }
        public string? RejectionReason { get; set; }
        public string? InspectionBy { get; set; }
        public string CoName { get; set; }
        public string? ITEM_SRNO_PO { get; set; }
        public string JiRequired { get; set; }
        public string NoJiOther { get; set; }
        public string JiSno { get; set; }
        public string NoJIReason { get; set; }
        public string Remarks { get; set; }
        public string JiStatusDesc { get; set; }
        public string AcceptRejornot { get; set; }
        public string DefectDesc { get; set; }
        public string? JiStatusCd { get; set; }
        public string Status { get; set; }
        public string Checksheet { get; set; }
        public string RootCause { get; set; }
        public string TechnicalReference { get; set; }
        public string AnyOther { get; set; }
        public string StatusCAPA { get; set; }
        public string DARStatus { get; set; }
        public string DARPurpose { get; set; }
        public string FinalRemarks { get; set; }
        public string Penaltytype { get; set; }
        public DateTime? DARDate { get; set; }
        public DateTime? PenaltyDate { get; set; }
        public byte? JiIeCd { get; set; }
        [NotMapped]
        public decimal? Updatedby { get; set; }
        public decimal? Createdby { get; set; }
    }
}
