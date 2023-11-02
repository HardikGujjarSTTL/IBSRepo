using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class InspectionTstPlndim
{
    public string? InspectCd { get; set; }

    public string? Dimension { get; set; }

    public string? DimPerDrg { get; set; }

    public string? Tolerance { get; set; }

    public string? NoOfSample { get; set; }
}
