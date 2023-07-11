using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class V12BillPayingOfficer
{
    public string BpoCd { get; set; } = null!;

    public string? Bpo { get; set; }

    public string? SapCustCdBpo { get; set; }
}
