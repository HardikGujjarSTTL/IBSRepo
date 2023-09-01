using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class CallRegisterModel
    {
        public string CaseNo { get; set; }

        public string PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        public string VendName { get; set; }

        public string? Createdby { get; set; }

        public string? Updatedby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public decimal? Isdeleted { get; set; }

        public DateTime? CallRecvDt { get; set; }

        public int? CallInstallNo { get; set; }

        public int? CallSNo { get; set; }

        public string VendCd { get; set; }

        public string CallLetterNo { get; set; }

        public string? CallStatus { get; set; }

        public string? Remarks { get; set; }

        public string? IE_SName { get; set; }

        public string? VendInspStopped { get; set; }

        public string? VendRemarks { get; set; }

        public int? MfgCd { get; set; }

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

    }
}
