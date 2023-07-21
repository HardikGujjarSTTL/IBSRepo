using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class CallsmarkedtoieView
{
    public string? Vendor { get; set; }

    public string? Manufacturer { get; set; }

    public int VendCd { get; set; }

    public int? MfgCd { get; set; }

    public string? Consignee { get; set; }

    public string? ItemDescPo { get; set; }

    public string? ExtDelvDt { get; set; }

    public string? CallMarkDt { get; set; }

    public string? InspDesireDt { get; set; }

    public string? CallRecvDt { get; set; }

    public string? NewVendor { get; set; }

    public string? IeName { get; set; }

    public string? IePhoneNo { get; set; }

    public string? PoNo { get; set; }

    public string? PoDate { get; set; }

    public string? PoYr { get; set; }

    public string? PoSource { get; set; }

    public string? Source { get; set; }

    public string? RlyCd { get; set; }

    public string CaseNo { get; set; } = null!;

    public string? UserId { get; set; }

    public string? Datetime { get; set; }

    public string? Remarks { get; set; }

    public string? CallStatus { get; set; }

    public string? CallStatusFull { get; set; }

    public string? Colour { get; set; }

    public string? MfgPers { get; set; }

    public string? MfgPhone { get; set; }

    public short CallSno { get; set; }

    public decimal? Count { get; set; }
}
