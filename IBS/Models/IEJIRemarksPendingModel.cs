using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class IEJIRemarksPendingModel
    {
        public string ComplaintId { get; set; } = null!;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ComplaintDt { get; set; }

        public string? RejMemo { get; set; }

        public string? CaseNo { get; set; }

        public string? BkNo { get; set; }

        public string? SetNo { get; set; }

        public string? IeName { get; set; }

        public string? IeCoName { get; set; }

        public string? CompRecvRegion { get; set; }

        public string? Consignee { get; set; }

        public string? Vendor { get; set; }

        public string? ItemDesc { get; set; }

        public string? QtyOff { get; set; }

        public string? QtyRejected { get; set; }

        public decimal? RejectionValue { get; set; }

        public string? RejectionReason { get; set; }

        public string? Status { get; set; }

        public string? JiRequired { get; set; }

        public string? JiSno { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? JiDt { get; set; }

        public string? DefectDesc { get; set; }

        public string? JiStatusDesc { get; set; }

        public string? Action { get; set; }

        public string? PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? IcDt { get; set; }

        public string? JiIeName { get; set; }

        public string? ActionProposed { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ActionProposedDt { get; set; }

        public string? CoName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? JiFixDt { get; set; }

        public byte? IeCd { get; set; }

        public string? IeJiRemarks { get; set; }
    }
}
