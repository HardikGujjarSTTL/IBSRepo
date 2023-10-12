using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T14aPoNonrlyHistory
{
    public string? CaseNo { get; set; }

    public string? ContractNo { get; set; }

    public DateTime? ContractDt { get; set; }

    public string? ProjectRef { get; set; }

    public long? MinFee { get; set; }

    public long? MaxFee { get; set; }

    public string? WithServTax { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
