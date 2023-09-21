using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class CallRemarkingModel
    {
        [Display(Name = "Controlling Officer")]
        [Required]
        public int COCd { get; set; }

        [Display(Name = "Inspection Engineer")]
        [Required]
        public int? FrIeCd { get; set; }

        [Display(Name = "Remarking To Inspection Engineer")]
        [Required]
        public int? ToIeCd { get; set; }

        [Display(Name = "Remarking Reason")]
        [Required]
        public string? RemarkingStatus { get; set; }

        public int? FrIePendingCalls { get; set; }

        public int? ToIePendingCalls { get; set; }

        public string? Region { get; set; }

        public string CaseNos { get; set; }

        public string? RemInitBy { get; set; }

    }

    public class CallRemarkingApprovalModel
    {
        public int Id { get; set; }

        public string? CaseNo { get; set; }

        public DateTime? CallRecvDt { get; set; }

        public int? CallSno { get; set; }

        public string? ToIeName { get; set; }

        public string? FrIeName { get; set; }

        public int? FrIePendingCalls { get; set; }

        public int? ToIePendingCalls { get; set; }

        public int IeCd { get; set; }

        public int CoCd { get; set; }

        public string? UserName { get; set; }

        public string? RemarkingStatus { get; set; }

        public string? RemarkReason { get; set; }

        public string? CallRemarkStatus { get; set; }

        public DateTime? CALL_DES_DT { get; set; }

        public DateTime? DtInspDesire { get; set; }

        public string Display_DtInspDesire { get { return this.DtInspDesire != null ? Common.ConvertDateFormat(this.DtInspDesire.Value) : ""; } }

        public string? Mfg { get; set; }

        public string? ItemDescPo { get; set; }

        public int COUNT { get; set; }

        public string? RLY { get; set; }

        public decimal? MAT_VALUE { get; set; }

        public DateTime? ExtDelvDt { get; set; }

        public string? pending_since { get; set; }

        [Required]
        public string? Remark { get; set; }

        public string Action { get; set; }

        public string? UserId { get; set; }

    }

    public class PendingCallsListModel
    {
        public int Id { get; set; }

        public string? CaseNo { get; set; }

        public DateTime? CallRecvDt { get; set; }

        public int? CallSno { get; set; }

        public string? CallStatus { get; set; }

        public int? MfgCd { get; set; }

        public string? MfgPlace { get; set; }

        public int? CoCd { get; set; }

        public string? VendName { get; set; }

        public int Mfg_CityCd { get; set; }

        public string? Mfg_City { get; set; }

        public DateTime? DtInspDesire { get; set; }

        public string? CallRemarkStatus { get; set; }

        public string? FrIeName { get; set; }

        public string? ToIeName { get; set; }

        public string? UserName { get; set; }

        public DateTime? RemInitDatetime { get; set; }

        public string? RemarkReason { get; set; }

        public int? FrIePendingCalls { get; set; }

        public int? ToIePendingCalls { get; set; }

    }

}
