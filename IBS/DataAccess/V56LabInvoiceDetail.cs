using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class V56LabInvoiceDetail
{
    public string? InvoiceNo { get; set; }

    public byte? ItemSrno { get; set; }

    public string? ItemDesc { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Rate { get; set; }

    public decimal? TestingCharges { get; set; }

    public decimal? Cgst { get; set; }

    public decimal? Sgst { get; set; }

    public decimal? Igst { get; set; }
}
