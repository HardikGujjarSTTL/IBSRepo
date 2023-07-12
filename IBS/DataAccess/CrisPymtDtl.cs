using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class CrisPymtDtl
{
    public string BillNo { get; set; } = null!;

    public string? IcNo { get; set; }

    public string? IcDt { get; set; }

    public string? Invoiceno { get; set; }

    public string? Co6No { get; set; }

    public string? Co6Date { get; set; }

    public string? Co6Status { get; set; }

    public string? Co6StatusDate { get; set; }

    public string? PassedAmt { get; set; }

    public string? DeductedAmt { get; set; }

    public string? NetAmt { get; set; }

    public string? Bookdate { get; set; }

    public string? ReturnReason { get; set; }

    public string? ReturnDate { get; set; }

    public string? Co7No { get; set; }

    public string? Co7Date { get; set; }

    public string? PaymentDt { get; set; }

    public string? Status { get; set; }

    public string? Updatedate { get; set; }
}
