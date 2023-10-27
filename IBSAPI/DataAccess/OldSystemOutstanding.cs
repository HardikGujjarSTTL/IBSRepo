using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class OldSystemOutstanding
{
    public string BpoCd { get; set; } = null!;

    public decimal? OutstandingAmt { get; set; }

    public string? OldSystemBpo { get; set; }
}
