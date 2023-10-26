using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class V22aBillingSummary
{
    public string? RegionCode { get; set; }

    public string? BillingYrMth { get; set; }

    public string? Sector { get; set; }

    public decimal? InspFee { get; set; }

    public decimal? ServiceTax { get; set; }

    public decimal? EduCess { get; set; }

    public decimal? SheCess { get; set; }

    public decimal? SwachhBharatCess { get; set; }

    public decimal? KrishiKalyanCess { get; set; }

    public decimal? Cgst { get; set; }

    public decimal? Sgst { get; set; }

    public decimal? Igst { get; set; }

    public decimal? BillAmount { get; set; }

    public decimal? NoOfBillls { get; set; }
}
