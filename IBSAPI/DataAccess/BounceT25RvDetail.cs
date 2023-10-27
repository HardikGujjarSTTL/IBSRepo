using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class BounceT25RvDetail
{
    public byte? BankCd { get; set; }

    public string? ChqNo { get; set; }

    public DateTime? ChqDt { get; set; }

    public decimal? Amount { get; set; }

    public byte? AccCd { get; set; }

    public decimal? BounceAmt { get; set; }

    public string? Region { get; set; }
}
