using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T14PoBpo
{
    public string CaseNo { get; set; } = null!;

    public int ConsigneeCd { get; set; }

    public string? BpoCd { get; set; }

    public virtual T12BillPayingOfficer? BpoCdNavigation { get; set; }

    public virtual T13PoMaster CaseNoNavigation { get; set; } = null!;

    public virtual T06Consignee ConsigneeCdNavigation { get; set; } = null!;

    public virtual ICollection<T15PoDetail> T15PoDetails { get; set; } = new List<T15PoDetail>();
}
