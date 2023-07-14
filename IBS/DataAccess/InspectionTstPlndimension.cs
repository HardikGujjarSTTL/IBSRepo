using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class InspectionTstPlnDimension
{
    public int? ExtendedHgt { get; set; }

    public int? CompressedHgt { get; set; }

    public int? DustCvrDiameter { get; set; }

    public int? TubeDiameter { get; set; }

    public int? BarPnDia17mm { get; set; }

    public int? BarPnCd { get; set; }

    public int? BarPnTotalLngth { get; set; }

    public int? EyeRing { get; set; }

    public string? PlNo { get; set; }

    public string? InspectCd { get; set; }
}
