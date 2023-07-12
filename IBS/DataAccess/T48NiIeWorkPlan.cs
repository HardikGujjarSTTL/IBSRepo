using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T48NiIeWorkPlan
{
    public int IeCd { get; set; }

    public byte? CoCd { get; set; }

    public string? NiWorkCd { get; set; }

    public string? NiOtherDesc { get; set; }

    public DateTime NiWorkDt { get; set; }

    public string? RegionCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual T09Ie IeCdNavigation { get; set; } = null!;
}
