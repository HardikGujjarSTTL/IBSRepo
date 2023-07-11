using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T77IcfBill
{
    public string BillNo { get; set; } = null!;

    public DateTime? BillDt { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public DateTime? IcDt { get; set; }

    public string? CallInstalmentNo { get; set; }

    public decimal? MaterialValue { get; set; }

    public decimal? InspFee { get; set; }

    public decimal? ServiceTax { get; set; }

    public decimal? SwachhBharatCess { get; set; }

    public decimal? KrishiKalyanCess { get; set; }

    public decimal? BillAmount { get; set; }

    public string? PoSeries { get; set; }

    public short? InvoiceNo { get; set; }

    public string? Region { get; set; }

    public string? L4noPo { get; set; }

    public virtual T22Bill BillNoNavigation { get; set; } = null!;
}
