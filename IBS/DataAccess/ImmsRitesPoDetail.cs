using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class ImmsRitesPoDetail
{
    public decimal Oid { get; set; }

    public int ImmsPokey { get; set; }

    public string ImmsRlyCd { get; set; } = null!;

    public string? ItemSrno { get; set; }

    public string? ItemDesc { get; set; }

    public int? ConsigneeCd { get; set; }

    public string? ImmsConsigneeCd { get; set; }

    public string? ImmsConsigneeName { get; set; }

    public string? ImmsConsigneeDetail { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Rate { get; set; }

    public byte? UomCd { get; set; }

    public string? ImmsUomCd { get; set; }

    public string? ImmsUomDesc { get; set; }

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

    public decimal? OtCharges { get; set; }

    public decimal? Value { get; set; }

    public DateTime? DelvDt { get; set; }

    public DateTime? ExtDelvDt { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? PlNo { get; set; }

    public string? ItemCd { get; set; }

    public string? Allocation { get; set; }
}
