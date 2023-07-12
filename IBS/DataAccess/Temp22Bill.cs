using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class Temp22Bill
{
    public string? RegionCode { get; set; }

    public string? BillNo { get; set; }

    public DateTime? BillDt { get; set; }

    public string? BpoCd { get; set; }

    public string? BpoType { get; set; }

    public string? BpoRly { get; set; }

    public string? BpoOrgn { get; set; }

    public decimal? BillAmount { get; set; }

    public decimal? Tds { get; set; }

    public decimal? RetentionMoney { get; set; }

    public decimal? WriteOffAmt { get; set; }

    public decimal? ChqAmtPosted { get; set; }

    public decimal? BillAmtCleared { get; set; }

    public decimal? Outstanding { get; set; }
}
