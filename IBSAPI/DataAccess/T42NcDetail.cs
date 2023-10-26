using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T42NcDetail
{
    public string NcNo { get; set; } = null!;

    public string? NcCd { get; set; }

    public int NcCdSno { get; set; }

    public string? NcDescOthers { get; set; }

    public string? IeAction1 { get; set; }

    public DateTime? IeAction1Dt { get; set; }

    public string? CoFinalRemarks1 { get; set; }

    public DateTime? CoFinalRemarks1Dt { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual T69NcCode? NcCdNavigation { get; set; }
}
