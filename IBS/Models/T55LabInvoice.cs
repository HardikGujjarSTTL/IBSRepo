using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T55LabInvoice
{
    public string InvoiceNo { get; set; } = null!;

    public DateTime? InvoiceDt { get; set; }

    public string? SampleRegNo { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? CaseNo { get; set; }

    public string? BpoCd { get; set; }

    public string? RecipientGstinNo { get; set; }

    public decimal? BillAmount { get; set; }

    public decimal? TotalCgst { get; set; }

    public decimal? TotalSgst { get; set; }

    public decimal? TotalIgst { get; set; }

    public string? RegionCode { get; set; }

    public string? TransactionNo { get; set; }

    public string? CustomerRefNo { get; set; }

    public byte? IeCd { get; set; }

    public string? IrnNo { get; set; }

    public string? AckNo { get; set; }

    public DateTime? AckDt { get; set; }

    public string? QrCode { get; set; }

    public string? SentToSap { get; set; }

    public string? InvSCity { get; set; }

    public string? BillFinalised { get; set; }

    public string? IncType { get; set; }

    public string? CNote { get; set; }

    public string? CreditId { get; set; }

    public virtual T50LabRegister? SampleRegNoNavigation { get; set; }
}
