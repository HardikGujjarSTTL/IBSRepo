using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class ViewVendorCallsMarkedForSpecificPo
{
    public string? L5noPo { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? RlyNonrly { get; set; }

    public string? RlyCd { get; set; }

    public int? VendCd { get; set; }
}
