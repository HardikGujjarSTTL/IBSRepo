using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class V06Consignee
{
    public int ConsigneeCd { get; set; }

    public string? Consignee { get; set; }

    public string? GstinNo { get; set; }

    public string? Status { get; set; }
}
