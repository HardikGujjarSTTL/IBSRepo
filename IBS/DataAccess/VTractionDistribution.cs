using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class VTractionDistribution
{
    public string? CaseNo { get; set; }

    public string? ItemDesc { get; set; }

    public decimal? QtyPassed { get; set; }

    public string? Uom { get; set; }

    public string? Item { get; set; }
}
