using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class InspectionTestParamvalue
{
    public string? ItemCd { get; set; }

    public string? SrNo { get; set; }

    public string? Parmeter { get; set; }

    public string? ValueSpecified { get; set; }

    public string? Remarks { get; set; }

    public string? InsptHead { get; set; }

    public string? InsptSno { get; set; }

    public string? HeaderDisplay { get; set; }

    public decimal? GrpOrder { get; set; }

    public string? GrpOrderId { get; set; }
}
