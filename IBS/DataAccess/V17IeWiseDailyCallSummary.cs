using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class V17IeWiseDailyCallSummary
{
    public string? Region { get; set; }

    public string? IeName { get; set; }

    public DateTime? CallDate { get; set; }

    public string? CallDay { get; set; }

    public decimal? Calls { get; set; }
}
