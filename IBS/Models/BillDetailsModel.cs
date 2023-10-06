using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class BillDetailsModel
    {
        public string BillNo { get; set; } = null!;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? BillDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? FromDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? ToDt { get; set; }

        public string? CaseNo { get; set; }

        public decimal? MaterialValue { get; set; }

        public string? FeeType { get; set; }

        public decimal? FeeRate { get; set; }

        public string? TaxType { get; set; }

        public decimal? InspFee { get; set; }

        public decimal? ServiceTax { get; set; }

        public decimal? EduCess { get; set; }

        public decimal? SheCess { get; set; }

        public long? MinFee { get; set; }

        public long? MaxFee { get; set; }

        public decimal? BillAmount { get; set; }

        public decimal? AmountReceived { get; set; }

        public decimal? Tds { get; set; }

        public decimal? BillAmtCleared { get; set; }

        public string? BillStatus { get; set; }

        public string? Remarks { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public decimal? RetentionMoney { get; set; }

        public decimal? WriteOffAmt { get; set; }

        public decimal? ServTaxRate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? TdsDt { get; set; }

        public string? AdvBill { get; set; }

        public string? ScannedStatus { get; set; }

        public decimal? SwachhBharatCess { get; set; }

        public decimal? KrishiKalyanCess { get; set; }

        public decimal? Sgst { get; set; }

        public decimal? Cgst { get; set; }

        public decimal? Igst { get; set; }

        public string? InvoiceNo { get; set; }

        public decimal? TdsSgst { get; set; }

        public decimal? TdsCgst { get; set; }

        public decimal? TdsIgst { get; set; }

        public string? CnoteBillNo { get; set; }

        public decimal? CnoteAmount { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? DigBillGenDt { get; set; }

        public string? BillResentStatus { get; set; }

        public bool? BillResentCount { get; set; }

        public string? IrnNo { get; set; }

        public string? AckNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? AckDt { get; set; }

        public string? QrCode { get; set; }

        public string? SentToSap { get; set; }

        public string? BillFinalised { get; set; }

        public string? InvoiceSuppDocs { get; set; }

        public string? CreditDocId { get; set; }

        public string? LoRemarks { get; set; }

        public string? SapStatus { get; set; }

        public string? Createdby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public string? Updatedby { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public decimal? Isdeleted { get; set; }

        public string? Billadtype { get; set; }

        public int? ReferenceAid { get; set; }
    }
}
