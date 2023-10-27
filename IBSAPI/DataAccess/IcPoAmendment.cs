using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class IcPoAmendment
{
    public string? CaseNo { get; set; }

    public string? PoNo { get; set; }

    public string? AmendmentDetail { get; set; }

    public int Id { get; set; }
}
