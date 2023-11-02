using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class AuditT20Ic
{
    public decimal? Seq { get; set; }

    public string? UserAt { get; set; }

    public DateTime? TimeNow { get; set; }

    public string? Term { get; set; }

    public string? Job { get; set; }

    public string? Proc { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public string? BillNo { get; set; }
}
