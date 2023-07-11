using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T36Bill
{
    public string BillNo { get; set; } = null!;

    public DateTime? BillDt { get; set; }

    public string? Region { get; set; }

    public string? RlyNonrly { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? Purchaser { get; set; }

    public string? Bpo { get; set; }

    public string? Consignee { get; set; }

    public string? RailDesc { get; set; }

    public string? IcNo { get; set; }

    public DateTime? IcDt { get; set; }

    public decimal? BasePrice { get; set; }

    public decimal? Qty { get; set; }

    public decimal? TotBaseValue { get; set; }

    public decimal? Laying { get; set; }

    public decimal? ExcisePer { get; set; }

    public decimal? Excise { get; set; }

    public decimal? SalesTaxPer { get; set; }

    public decimal? SalesTax { get; set; }

    public decimal? MaterialValue { get; set; }

    public decimal? FeeRate { get; set; }

    public decimal? InspFee { get; set; }

    public decimal? ServTaxRate { get; set; }

    public decimal? ServiceTax { get; set; }

    public decimal? EduCess { get; set; }

    public decimal? SheCess { get; set; }

    public decimal? BillAmount { get; set; }

    public string? Remarks { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
