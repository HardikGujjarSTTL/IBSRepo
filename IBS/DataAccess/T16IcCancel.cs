using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T16IcCancel
{
    public string BkNo { get; set; } = null!;

    public string SetNo { get; set; } = null!;

    public int? IssueToIecd { get; set; }

    public string? IcStatus { get; set; }

    public DateTime? StatusDt { get; set; }

    public string Region { get; set; } = null!;

    public string? Remarks { get; set; }

    public virtual T09Ie? IssueToIecdNavigation { get; set; }
}
