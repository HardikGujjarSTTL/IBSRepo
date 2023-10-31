using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewVendorCallsMarkedForSpecificIcSub
{
    public string? Vendor { get; set; }

    public string? Manufacturer { get; set; }

    public int VendCd { get; set; }

    public int? MfgCd { get; set; }

    public string? Consignee { get; set; }

    public string? ItemDescPo { get; set; }

    public decimal? QtyToInsp { get; set; }

    public DateTime? CallMarkDt { get; set; }

    public string? IeName { get; set; }

    public string? IePhoneNo { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string CaseNo { get; set; } = null!;

    public string? Remark { get; set; }

    public string? CallStatus { get; set; }

    public string? Colour { get; set; }

    public string? MfgPers { get; set; }

    public string? MfgPhone { get; set; }

    public short CallSno { get; set; }

    public string? Hologram { get; set; }

    public string? IcPhoto { get; set; }

    public string? IcPhotoA1 { get; set; }

    public string? IcPhotoA2 { get; set; }

    public decimal? Count { get; set; }

    public string? CoName { get; set; }

    public string? L5noPo { get; set; }

    public string? RlyNonrly { get; set; }

    public string? RlyCd { get; set; }
}
