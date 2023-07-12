using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class TempDeptWiseCall
{
    public string? CaseNo { get; set; }

    public string? Department { get; set; }

    public decimal? PoValue { get; set; }

    public DateTime? PoRecvDt { get; set; }
}
