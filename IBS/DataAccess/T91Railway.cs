using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T91Railway
{
    public string RlyCd { get; set; } = null!;

    public string? Railway { get; set; }

    public string? HeadQuarter { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? ImmsRlyCd { get; set; }
}
