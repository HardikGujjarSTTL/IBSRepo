using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class RkmT26ChequePosting
{
    public byte? BankCd { get; set; }

    public string? ChqNo { get; set; }

    public DateTime? ChqDt { get; set; }

    public decimal? ClearedAmt { get; set; }
}
