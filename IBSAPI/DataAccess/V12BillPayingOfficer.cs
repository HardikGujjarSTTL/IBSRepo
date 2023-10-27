using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class V12BillPayingOfficer
{
    public string BpoCd { get; set; } = null!;

    public string? Bpo { get; set; }

    public string? SapCustCdBpo { get; set; }
}
