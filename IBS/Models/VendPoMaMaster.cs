using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class VendPoMaMaster
{
    public string CaseNo { get; set; } = null!;

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? RlyNonrly { get; set; }

    public string? RlyCd { get; set; }

    public string MaNo { get; set; } = null!;

    public DateTime MaDt { get; set; }

    public string? PoOrLetter { get; set; }

    public string? PoSrc { get; set; }

    public virtual ICollection<VendPoMaDetail> VendPoMaDetails { get; set; } = new List<VendPoMaDetail>();
}
