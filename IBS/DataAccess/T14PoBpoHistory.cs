using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T14PoBpoHistory
{
    public string? CaseNo { get; set; }

    public int? ConsigneeCd { get; set; }

    public string? BpoCd { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
