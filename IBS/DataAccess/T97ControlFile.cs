using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T97ControlFile
{
    public string Region { get; set; } = null!;

    public string? AllowOldBillDt { get; set; }

    public int? GraceDays { get; set; }

    public string? Createdby { get; set; }

    public string? Updatedby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public decimal? Isdeleted { get; set; }
}
