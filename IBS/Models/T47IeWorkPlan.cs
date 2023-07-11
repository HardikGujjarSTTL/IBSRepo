using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T47IeWorkPlan
{
    public int? IeCd { get; set; }

    public byte? CoCd { get; set; }

    public DateTime VisitDt { get; set; }

    public string CaseNo { get; set; } = null!;

    public DateTime CallRecvDt { get; set; }

    public short CallSno { get; set; }

    public int? MfgCd { get; set; }

    public string? MfgPlace { get; set; }

    public string? RegionCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public decimal? Id { get; set; }

    public virtual T17CallRegister Ca { get; set; } = null!;

    public virtual T09Ie? IeCdNavigation { get; set; }

    public virtual T05Vendor? MfgCdNavigation { get; set; }
}
