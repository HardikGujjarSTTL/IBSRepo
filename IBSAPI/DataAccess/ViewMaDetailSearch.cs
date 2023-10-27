using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewMaDetailSearch
{
    public string CaseNo { get; set; } = null!;

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string MaNo { get; set; } = null!;

    public DateTime MaDt { get; set; }

    public string? RlyNonrly { get; set; }

    public string? RlyCd { get; set; }

    public string? PoOrLetter { get; set; }

    public byte MaSno { get; set; }

    public string? MaField { get; set; }

    public string? MaStatus { get; set; }

    public string? MaDesc { get; set; }

    public string? OldPoValue { get; set; }

    public string? NewPoValue { get; set; }

    public string? MaRemarks { get; set; }

    public int? VendCd { get; set; }
}
