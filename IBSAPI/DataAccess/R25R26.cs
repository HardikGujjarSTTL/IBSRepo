using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class R25R26
{
    public string? CaseNo { get; set; }

    public byte BankCd { get; set; }

    public string ChqNo { get; set; } = null!;

    public DateTime ChqDt { get; set; }

    public string? BpoCd { get; set; }

    public string? BpoType { get; set; }

    public string? BillNo { get; set; }
}
