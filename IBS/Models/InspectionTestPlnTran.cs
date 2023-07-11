using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class InspectionTestPlnTran
{
    public string? InspectCd { get; set; }

    public string? ItemCd { get; set; }

    public string? Parmeter { get; set; }

    public string? ValueSpecified { get; set; }

    public string? Observation { get; set; }

    public string? ObservStatus { get; set; }

    public string? Remarks { get; set; }

    public string? InsptHead { get; set; }

    public string? InsptSno { get; set; }
}
