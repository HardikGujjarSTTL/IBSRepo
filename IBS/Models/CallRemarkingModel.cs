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

    public class PendingCallsListModel
    {
        public string? CaseNo { get; set; }

        public DateTime? CallRecvDt { get; set; }

        public short? CallSno { get; set; }

        public string? CallStatus { get; set; }

        public int? MfgCd { get; set; }

        public string? MfgPlace { get; set; }

        public byte? CoCd { get; set; }

        public string? VendName { get; set; }

        public int Mfg_CityCd { get; set; }

        public string? Mfg_City { get; set; }

        public DateTime? DtInspDesire { get; set; }

        public string? CallRemarkStatus { get; set; }

        public string? FrIeName { get; set; }

        public string? ToIeName { get; set; }

        public string? UserName { get; set; }

        public DateTime? RemInitDatetime { get; set; }

    }

}
