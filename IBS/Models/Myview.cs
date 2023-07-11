using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class Myview
{
    public string? RegionCode { get; set; }

    public string CaseNo { get; set; } = null!;

    public decimal? PoValue { get; set; }

    public string? IeDepartment { get; set; }
}
