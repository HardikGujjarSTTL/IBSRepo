using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewGetPoDetailsReportRVendorPlace
{
    public string? BillNo { get; set; }

    public DateTime? IcDt { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public string? ReasonReject { get; set; }

    public string? IeName { get; set; }

    public string? Vendor { get; set; }

    public string? ItemDescPo { get; set; }

    public string? IcTypeId { get; set; }

    public int? VendCd { get; set; }

    public string CaseNo { get; set; } = null!;
}
