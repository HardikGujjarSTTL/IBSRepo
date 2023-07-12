using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class ImmsRitesPocaDtl
{
    public string Rly { get; set; } = null!;

    public int Cakey { get; set; }

    public byte Slno { get; set; }

    public string? PlNo { get; set; }

    public string? PoSr { get; set; }

    public decimal? PoBalQty { get; set; }

    public decimal? CancQty { get; set; }

    public string? Status { get; set; }

    public string? DemStatus { get; set; }

    public DateTime? Datetime { get; set; }
}
