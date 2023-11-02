using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class GetBankdetail
{
    public string VchrNo { get; set; } = null!;

    public DateTime? VchrDt { get; set; }

    public string ChqNo { get; set; } = null!;

    public DateTime ChqDt { get; set; }

    public int BankCd { get; set; }

    public string? Bpo { get; set; }

    public decimal? Amount { get; set; }

    public decimal? AmountAdjusted { get; set; }

    public decimal? AmtTransferred { get; set; }

    public decimal? SuspenseAmt { get; set; }

    public int? AccCd { get; set; }
}
