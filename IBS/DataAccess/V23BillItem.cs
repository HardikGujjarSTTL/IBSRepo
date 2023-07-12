using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class V23BillItem
{
    public string? BillNo { get; set; }

    public byte? ItemSrno { get; set; }

    public string? ItemDesc { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Rate { get; set; }

    public string? UomSDesc { get; set; }

    public string? UomFactor { get; set; }

    public decimal? BasicValue { get; set; }

    public decimal? SalesTaxPer { get; set; }

    public decimal? SalesTax { get; set; }

    public string? ExciseType { get; set; }

    public decimal? ExcisePer { get; set; }

    public decimal? Excise { get; set; }

    public string? DiscountType { get; set; }

    public decimal? DiscountPer { get; set; }

    public decimal? Discount { get; set; }

    public string? OtChargeType { get; set; }

    public decimal? OtChargePer { get; set; }

    public decimal? OtherCharges { get; set; }

    public decimal? Value { get; set; }
}
