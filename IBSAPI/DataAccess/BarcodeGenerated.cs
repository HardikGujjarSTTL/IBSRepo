using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class BarcodeGenerated
{
    public string Id { get; set; } = null!;

    public string? BarcodeNo { get; set; }

    public int? Qty { get; set; }

    public string? CaseNo { get; set; }

    public DateTime? Createddate { get; set; }

    public string? Createdby { get; set; }

    public string? Userid { get; set; }

    public string? Ipaddress { get; set; }

    public int? CallSno { get; set; }

    public DateTime? CallDate { get; set; }
}
