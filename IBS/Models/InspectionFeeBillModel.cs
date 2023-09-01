using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class InspectionFeeBillModel
    {
        public string BillNo { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? BillDt { get; set; }

        public string Display_BillDt { get { return this.BillDt != null ? Common.ConvertDateFormat(this.BillDt.Value) : ""; } }

        public string? CaseNo { get; set; }

        public decimal? MaterialValue { get; set; }

        public string? FeeType { get; set; }

        public decimal? FeeRate { get; set; }

        public string? TaxType { get; set; }

        public string? TaxType_GST { get; set; }

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

        public DateTime? DigBillGenDt { get; set; }

        public string? BillResentStatus { get; set; }

        public bool? BillResentCount { get; set; }

        public string? IrnNo { get; set; }

        public string? AckNo { get; set; }

        public DateTime? AckDt { get; set; }

        public string? QrCode { get; set; }

        public string? SentToSap { get; set; }

        public string? BillFinalised { get; set; }

        public string? InvoiceSuppDocs { get; set; }

        public string? CreditDocId { get; set; }

        public string? LoRemarks { get; set; }

        public string? SapStatus { get; set; }

        public decimal? AmountCleared { get; set; }

        public decimal? AmountRecievedThruChequeDD { get; set; }

        public decimal? TotalAmountReceived { get; set; }

        public decimal? AmountRecover { get; set; }
    }

    public class BillItemsListModel
    {
        public string? BillNo { get; set; }

        public byte ItemSrno { get; set; }

        public string? ItemDesc { get; set; }

        public decimal? Qty { get; set; }

        public decimal? Rate { get; set; }

        public byte? UomCd { get; set; }

        public string? UomSDesc { get; set; }

        public decimal? BasicValue { get; set; }

        public decimal? SalesTaxPer { get; set; }

        public decimal? SalesTax { get; set; }

        public string? ExciseType { get; set; }

        public decimal? ExcisePer { get; set; }

        public decimal? Excise { get; set; }

        public string? DiscountType { get; set; }

        public decimal? DiscountPer { get; set; }

        public decimal? Discount { get; set; }

        public decimal? OtherCharges { get; set; }

        public decimal? Value { get; set; }

        public string? OtChargeType { get; set; }

        public decimal? OtChargePer { get; set; }
    }

    public class ChequeDetailsListModel
    {
        public string? BankName { get; set; }

        public string ChqNo { get; set; } = null!;

        public DateTime ChqDt { get; set; }

        public decimal? Amount { get; set; }

        public decimal? AmountCleared { get; set; }
    }
}
