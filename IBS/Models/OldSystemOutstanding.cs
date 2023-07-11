using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class OldSystemOutstanding
{
    public string BpoCd { get; set; } = null!;

    public decimal? OutstandingAmt { get; set; }

    public string? OldSystemBpo { get; set; }
}
