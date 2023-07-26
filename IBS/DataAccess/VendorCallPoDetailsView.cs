using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class VendorCallPoDetailsView
{
    public string CaseNo { get; set; } = null!;

    public string? PurchaserCd { get; set; }

    public string? VendCd { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? Rly { get; set; }

    public string? L5noPo { get; set; }

    public string? RlyNonrly { get; set; }
}
