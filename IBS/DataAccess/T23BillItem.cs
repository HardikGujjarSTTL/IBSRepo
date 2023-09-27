using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T23BillItem
{
    public string? BillNo { get; set; }

    public int? ItemSrno { get; set; }

    public string? ItemDesc { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Rate { get; set; }

    public byte? UomCd { get; set; }

    public decimal? BasicValue { get; set; }

    public decimal? SalesTaxPer { get; set; }

    public decimal? SalesTax { get; set; }

    public string? ExciseType { get; set; }

    public decimal? ExcisePer { get; set; }

    public decimal? Excise { get; set; }

    public string? DiscountType { get; set; }

    public decimal? DiscountPer { get; set; }

    public decimal? Discount { get; set; }

    public decimal? OtherCharges { get; set; }

    public decimal? Value { get; set; }

    public string? OtChargeType { get; set; }

    public decimal? OtChargePer { get; set; }

    public virtual T22Bill? BillNoNavigation { get; set; }

    public virtual T04Uom? UomCdNavigation { get; set; }
}
