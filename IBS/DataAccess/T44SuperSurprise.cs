using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T44SuperSurprise
{
    public string? SuperSurpriseNo { get; set; }

    public DateTime? SuperSurpriseDt { get; set; }

    public string CaseNo { get; set; } = null!;

    public DateTime CallRecvDt { get; set; }

    public short CallSno { get; set; }

    public byte? IeCd { get; set; }

    public byte? CoCd { get; set; }

    public string? ItemDesc { get; set; }

    public string? Discrepancy { get; set; }

    public string? Outcome { get; set; }

    public string? PreIntRej { get; set; }

    public string? NameScopeItem { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? SbuHeadRemarks { get; set; }

    public virtual T17CallRegister Ca { get; set; } = null!;
}
