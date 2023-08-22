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
        public string IC_NO { get; set; }
        public DateTime? IC_DATE { get; set; }
        public string FormattedIC_DATE { get; set; }
        public DateTime? ComplaintDate { get; set; }
        public string? RejMemoNo { get; set; }
        public string? Railway { get; set; }
        public DateTime? RejMemoDt { get; set; }
        public string? InspRegion { get; set; }
        public string? ItemDesc { get; set; }
        public decimal? QtyOffered { get; set; }
        public decimal? QtyRejected { get; set; }
        public decimal? Rate { get; set; }
        public string? RejectionReason { get; set; }
        public string? InspectionBy { get; set; }
        public string CoName { get; set; }
        [NotMapped]
        public SelectList ControllingManagers { get; set; }
        public decimal? Updatedby { get; set; }
        public decimal? Createdby { get; set; }
    }
}
