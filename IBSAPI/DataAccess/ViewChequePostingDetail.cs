using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewChequePostingDetail
{
    public int? BankCd { get; set; }

    public string? ChqNo { get; set; }

    public DateTime? ChqDt { get; set; }

    public string? BillNo { get; set; }

    public decimal? BillAmount { get; set; }

    public decimal? AmountCleared { get; set; }

    public DateTime? PostingDt { get; set; }

    public decimal? BillAmtCleared { get; set; }

    public string? BpoName { get; set; }

    public DateTime? Datetime { get; set; }
}
