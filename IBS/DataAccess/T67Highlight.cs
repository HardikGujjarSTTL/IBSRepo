using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T67Highlight
{
    public string RegionCode { get; set; } = null!;

    public string HighDt { get; set; } = null!;

    public string? HightText { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
