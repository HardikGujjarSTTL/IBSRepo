using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T14aPoNonrly
{
    public string CaseNo { get; set; } = null!;

    public string? ContractNo { get; set; }

    public DateTime? ContractDt { get; set; }

    public string? ProjectRef { get; set; }

    public long? MinFee { get; set; }

    public long? MaxFee { get; set; }

    public string? WithServTax { get; set; }

    public virtual T13PoMaster CaseNoNavigation { get; set; } = null!;
}
