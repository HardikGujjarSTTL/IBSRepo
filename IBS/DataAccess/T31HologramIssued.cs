using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T31HologramIssued
{
    public string? HgRegion { get; set; }

    public string? HgNoFr { get; set; }

    public string? HgNoTo { get; set; }

    public DateTime? HgIssueDt { get; set; }

    public int? HgIecd { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual T09Ie? HgIecdNavigation { get; set; }
}
