using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class V22Bill
{
    public string BillNo { get; set; } = null!;

    public DateTime? BillDt { get; set; }

    public DateTime? DigBillGenDt { get; set; }

    public string? BpoCd { get; set; }

    public string? BpoType { get; set; }

    public string? BpoRly { get; set; }

    public string? BpoName { get; set; }

    public string? BpoOrgn { get; set; }

    public string? BpoAdd { get; set; }

    public string? BpoCity { get; set; }

    public string? SapCustCdBpo { get; set; }

    public string? Au { get; set; }

    public string? CaseNo { get; set; }

    public string? RegionCode { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? PoSource { get; set; }

    public string? PoOrLetter { get; set; }

    public int? VendCd { get; set; }

    public string? VendName { get; set; }

    public string? VendAdd1 { get; set; }

    public string? VendAdd2 { get; set; }

    public string? VendorCity { get; set; }

    public int ConsigneeCd { get; set; }

    public string? Consignee { get; set; }

    public string? Relativepath { get; set; }

    public string? ConsigneeAdd1 { get; set; }

    public string? ConsigneeAdd2 { get; set; }

    public string? ConsigneeCity { get; set; }

    public string? SapCustCdCon { get; set; }

    public int? IeCd { get; set; }

    public int? IeCoCd { get; set; }

    public string? IcNo { get; set; }

    public DateTime? IcDt { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public string? CallInstalmentNo { get; set; }

    public decimal? MaterialValue { get; set; }

    public string? FeeType { get; set; }

    public int? Visits { get; set; }

    public decimal? FeeRate { get; set; }

    public string? TaxType { get; set; }

    public decimal? InspFee { get; set; }

    public decimal? ServTaxRate { get; set; }

    public decimal? ServiceTax { get; set; }

    public decimal? EduCess { get; set; }

    public decimal? SheCess { get; set; }

    public decimal? SwachhBharatCess { get; set; }

    public decimal? KrishiKalyanCess { get; set; }

    public decimal? Cgst { get; set; }

    public decimal? Sgst { get; set; }

    public decimal? Igst { get; set; }

    public long? MinFee { get; set; }

    public long? MaxFee { get; set; }

    public decimal? BillAmount { get; set; }

    public decimal? AmountReceived { get; set; }

    public decimal? Tds { get; set; }

    public decimal? TdsSgst { get; set; }

    public decimal? TdsCgst { get; set; }

    public decimal? TdsIgst { get; set; }

    public DateTime? TdsDt { get; set; }

    public decimal? RetentionMoney { get; set; }

    public decimal? CnoteAmount { get; set; }

    public decimal? WriteOffAmt { get; set; }

    public decimal? BillAmtCleared { get; set; }

    public string? BillStatus { get; set; }

    public string? AdvBill { get; set; }

    public string? Remarks { get; set; }

    public string? InvoiceNo { get; set; }

    public string? RecipientGstinNo { get; set; }

    public string? UserId { get; set; }

    public string? BillResentStatus { get; set; }

    public bool? BillResentCount { get; set; }

    public string? IrfcFunded { get; set; }

    public string? IrfcBpoCd { get; set; }

    public string? IrfcBpoName { get; set; }

    public string? IrfcBpoRly { get; set; }

    public string? IrfcBpoAdd { get; set; }

    public string? IrfcBpoCity { get; set; }

    public string? SapCustCdIrfc { get; set; }

    public string? AccGroup { get; set; }

    public string? QrCode { get; set; }

    public string? IrnNo { get; set; }

    public string? AckNo { get; set; }

    public DateTime? AckDt { get; set; }

    public string? SentToSap { get; set; }

    public string? BillFinalised { get; set; }

    public string? InvoiceSuppDocs { get; set; }

    public string? LoRemarks { get; set; }

    public string? CreditDocId { get; set; }

    public string? YrMth { get; set; }
}
