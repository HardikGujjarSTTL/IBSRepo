using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class V2425Realisation
{
    public string? BpoCd { get; set; }

    public string? BpoRly { get; set; }

    public string? BpoType { get; set; }

    public string? BpoOrgn { get; set; }

    public string? VchrNo { get; set; }

    public string? VchrType { get; set; }

    public DateTime? RealisationDt { get; set; }

    public byte? BankCd { get; set; }

    public string? ChqNo { get; set; }

    public DateTime? ChqDt { get; set; }

    public decimal? RealisedAmt { get; set; }

    public byte? AccCd { get; set; }

    public decimal? AmountAdjusted { get; set; }

    public decimal? AmtTransferred { get; set; }

    public decimal? SuspenseAmt { get; set; }
}
