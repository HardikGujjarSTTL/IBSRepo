using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T26ChequePosting
{
    public int? BankCd { get; set; }

    public string? ChqNo { get; set; }

    public DateTime? ChqDt { get; set; }

    public string? BillNo { get; set; }

    public decimal? BillAmount { get; set; }

    public decimal? AmountCleared { get; set; }

    public DateTime? PostingDt { get; set; }

    public string? BpoCd { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual T25RvDetail? T25RvDetail { get; set; }
}
