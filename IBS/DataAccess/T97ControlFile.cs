using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T97ControlFile
{
    public string Region { get; set; } = null!;

    public string? AllowOldBillDt { get; set; }

    public bool? GraceDays { get; set; }
}
