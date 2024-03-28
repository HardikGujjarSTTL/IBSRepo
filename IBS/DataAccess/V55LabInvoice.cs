using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class V55LabInvoice
{
    public string InvoiceNo { get; set; } = null!;

    public DateTime? InvoiceDt { get; set; }

    public string? SampleRegNo { get; set; }

    public string? BpoCd { get; set; }

    public string? BpoType { get; set; }

    public string? BpoRly { get; set; }

    public string? BpoName { get; set; }

    public string? BpoOrgn { get; set; }

    public string? BpoAdd { get; set; }

    public string? BpoCity { get; set; }

    public string? SapCustCdBpo { get; set; }

    public string? CaseNo { get; set; }

    public string? RegionCode { get; set; }

    public string? TransactionNo { get; set; }

    public string? CustomerRefNo { get; set; }

    public string? IeName { get; set; }

    public byte? IeCd { get; set; }

    public decimal? TotalCgst { get; set; }

    public decimal? TotalSgst { get; set; }

    public decimal? TotalIgst { get; set; }

    public decimal? BillAmount { get; set; }

    public decimal? TotalBillAmount { get; set; }

    public string? RecipientGstinNo { get; set; }

    public string? QrCode { get; set; }

    public string? IrnNo { get; set; }

    public string? AckNo { get; set; }

    public DateTime? AckDt { get; set; }

    public string? SentToSap { get; set; }

    public string? BillFinalised { get; set; }

    public string? InvSCity { get; set; }

    public DateTimeOffset? DigBillGenDt { get; set; }
}
