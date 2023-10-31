using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class NoIeWorkPlan
{
    public int IeCd { get; set; }

    public byte? CoCd { get; set; }

    public string? Reason { get; set; }

    public DateTime NwpDt { get; set; }

    public string? RegionCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual T09Ie IeCdNavigation { get; set; } = null!;
}
