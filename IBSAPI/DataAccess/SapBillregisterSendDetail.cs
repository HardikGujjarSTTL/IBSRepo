using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class SapBillregisterSendDetail
{
    public decimal Id { get; set; }

    public string? Batchid { get; set; }

    public DateTime? BatchDt { get; set; }

    public string? SapAcknowledgementId { get; set; }

    public DateTime? SapAcknowledgementDt { get; set; }

    public string? BillNo { get; set; }

    public DateTime? BillDt { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public DateTime? IcDt { get; set; }

    public decimal? BillAmount { get; set; }

    public decimal? ServiceTax { get; set; }

    public decimal? EduCess { get; set; }

    public decimal? SheCess { get; set; }

    public decimal? SwachhBharatCess { get; set; }

    public decimal? KrishiKalyanCess { get; set; }

    public decimal? InspFee { get; set; }

    public decimal? Igst { get; set; }

    public decimal? Sgst { get; set; }

    public decimal? Cgst { get; set; }

    public string? InvoiceNo { get; set; }

    public string? RecipientGstinNo { get; set; }

    public string? BpoType { get; set; }

    public string? IrnNo { get; set; }

    public string? Senttosap { get; set; }

    public string? Finalised { get; set; }

    public DateTime? AckDt { get; set; }

    public DateTime? DigBillGenDt { get; set; }

    public string? Bpo { get; set; }

    public decimal? MaterialValue { get; set; }

    public string? State { get; set; }

    public string? CaseNo { get; set; }

    public string? Checkvalue { get; set; }

    public string? QrCode { get; set; }
}
