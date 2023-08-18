using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T25RvDetail
{
    public string? VchrNo { get; set; }

    public int? Sno { get; set; }

    public int BankCd { get; set; }

    public string ChqNo { get; set; } = null!;

    public DateTime ChqDt { get; set; }

    public decimal? Amount { get; set; }

    public int? AccCd { get; set; }

    public decimal? AmountAdjusted { get; set; }

    public decimal? SuspenseAmt { get; set; }

    public string? Narration { get; set; }

    public string? SampleNo { get; set; }

    public DateTime? PostDt { get; set; }

    public string? Status { get; set; }

    public string? BpoCd { get; set; }

    public string? BpoType { get; set; }

    public string? CaseNo { get; set; }

    public decimal? AmtTransferred { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual T13PoMaster? CaseNoNavigation { get; set; }

    public virtual T24Rv? VchrNoNavigation { get; set; }
}
