using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class OngoingNonrlyContract
{
    public string? ContractCd { get; set; }

    public string? ClientType { get; set; }

    public string? ClientName { get; set; }

    public string? ContractNo { get; set; }

    public DateTime? ContPeriodFr { get; set; }

    public DateTime? ContPeriodTo { get; set; }

    public string? ContFeeType { get; set; }

    public string? ContTaxType { get; set; }

    public decimal? ContFee { get; set; }

    public long? MinFee { get; set; }

    public long? MaxFee { get; set; }

    public byte? ContCm { get; set; }

    public string? ContSpecConds { get; set; }

    public string? ContPenalty { get; set; }

    public string? ContRegion { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? ClientSname { get; set; }
}
