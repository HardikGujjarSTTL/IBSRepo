using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class VendPoMa
{
    public string CaseNo { get; set; } = null!;

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? RlyNonrly { get; set; }

    public string? RlyCd { get; set; }

    public string MaNo { get; set; } = null!;

    public DateTime MaDt { get; set; }

    public string? MaDesc { get; set; }

    public string? OldPoValue { get; set; }

    public string? NewPoValue { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? MaStatus { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedDatetime { get; set; }
}
