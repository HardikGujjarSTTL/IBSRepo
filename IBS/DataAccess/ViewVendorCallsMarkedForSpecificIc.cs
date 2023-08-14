using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class ViewVendorCallsMarkedForSpecificIc
{
    public string? Purchaser { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? IcNo { get; set; }

    public DateTime? IcDt { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public string? BillNo { get; set; }

    public string? IeName { get; set; }

    public string? Vendor { get; set; }

    public string? ItemDescPo { get; set; }

    public decimal? QtyToInsp { get; set; }

    public decimal? QtyPassed { get; set; }

    public decimal? QtyRejected { get; set; }

    public string? Hologram { get; set; }

    public string? IcPhoto { get; set; }

    public string? IcPhotoA1 { get; set; }

    public string? IcPhotoA2 { get; set; }

    public string? L5noPo { get; set; }

    public string? RlyNonrly { get; set; }

    public string? RlyCd { get; set; }

    public int? VendCd { get; set; }
}
