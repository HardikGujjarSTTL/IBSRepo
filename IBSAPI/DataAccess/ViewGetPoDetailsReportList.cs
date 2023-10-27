using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewGetPoDetailsReportList
{
    public string CaseNo { get; set; } = null!;

    public byte ItemSrno { get; set; }

    public string? ItemDesc { get; set; }

    public decimal? Qty { get; set; }

    public DateTime? ExtDelvDt { get; set; }

    public decimal? Passed { get; set; }

    public decimal? Rejected { get; set; }
}
