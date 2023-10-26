using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewGetInspectionCertDetail
{
    public string? Icno { get; set; }

    public string Caseno { get; set; } = null!;

    public string? Bkno { get; set; }

    public string? Setno { get; set; }

    public string? Status { get; set; }

    public DateTime Callrecvdt { get; set; }

    public int Callsno { get; set; }

    public string? Iesname { get; set; }

    public string? Consignee { get; set; }

    public string? Callstatusdesc { get; set; }

    public string? Regioncode { get; set; }

    public string? Callstatus { get; set; }
}
