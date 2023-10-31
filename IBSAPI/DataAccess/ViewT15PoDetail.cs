using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewT15PoDetail
{
    public string CaseNo { get; set; } = null!;

    public byte ItemSrno { get; set; }

    public string? ItemDesc { get; set; }

    public string? ConsigneeName { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Rate { get; set; }

    public decimal? Value { get; set; }

    public int ConsigneeCd { get; set; }
}
