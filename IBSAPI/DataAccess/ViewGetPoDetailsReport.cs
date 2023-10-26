using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewGetPoDetailsReport
{
    public string CaseNo { get; set; } = null!;

    public int VendCd { get; set; }

    public string? Vendor { get; set; }

    public string? VendRemarks { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? PoSource { get; set; }

    public string? ImmsRlyCd { get; set; }

    public string? RlyCd { get; set; }

    public string? Remarks { get; set; }
}
