using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class HistT06Consignee
{
    public int? ConsigneeCd { get; set; }

    public string? ConsigneeType { get; set; }

    public string? ConsigneeDesig { get; set; }

    public string? ConsigneeDept { get; set; }

    public string? ConsigneeFirm { get; set; }

    public string? ConsigneeAdd1 { get; set; }

    public string? ConsigneeAdd2 { get; set; }

    public byte? ConsigneeCity { get; set; }
}
