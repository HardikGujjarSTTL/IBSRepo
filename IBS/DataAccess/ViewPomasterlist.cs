using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class ViewPomasterlist
{
    public int? VendCd { get; set; }

    public string CaseNo { get; set; } = null!;

    public string? PoNo { get; set; }

    public string? PoDt { get; set; }

    public string? RlyCd { get; set; }

    public string? RealCaseNo { get; set; }

    public string? VendName { get; set; }

    public string? ConsigneeSName { get; set; }

    public string? Remarks { get; set; }
}
