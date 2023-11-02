using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T71RcfBill
{
    public string BillNo { get; set; } = null!;

    public DateTime? BillDt { get; set; }

    public string? PoNo { get; set; }

    public string? PoSeries { get; set; }

    public short? InvoiceNo { get; set; }

    public string? Region { get; set; }

    public virtual T22Bill BillNoNavigation { get; set; } = null!;
}
