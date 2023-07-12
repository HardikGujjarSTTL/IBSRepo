using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class ImmsRitesPocaHdr
{
    public string Rly { get; set; } = null!;

    public int Cakey { get; set; }

    public DateTime? CakeyDate { get; set; }

    public int? Pokey { get; set; }

    public string? PoNo { get; set; }

    public string? CaNo { get; set; }

    public DateTime? CaDate { get; set; }

    public string? CaType { get; set; }

    public string? Vcode { get; set; }

    public string? RefNo { get; set; }

    public DateTime? RefDate { get; set; }

    public string? Remarks { get; set; }

    public DateTime? Datetime { get; set; }
}
