using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class TdsHistory
{
    public int Id { get; set; }

    public string? BillNo { get; set; }

    public string? CaseNo { get; set; }

    public decimal? Tds { get; set; }

    public decimal? TdsCgst { get; set; }

    public decimal? TdsSgst { get; set; }

    public decimal? TdsIgst { get; set; }

    public decimal? RetentionMoney { get; set; }

    public decimal? WriteOffAmt { get; set; }

    public DateTime? TdsDt { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public decimal? BillAmtCleared { get; set; }
}
