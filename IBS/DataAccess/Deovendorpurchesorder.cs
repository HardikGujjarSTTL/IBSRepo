using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class Deovendorpurchesorder
{
    public string CaseNo { get; set; } = null!;

    public string? RealCaseNo { get; set; }

    public string? PoNo { get; set; }

    public string? PoDt { get; set; }

    public string? RecvDt { get; set; }

    public string? RlyNonrly { get; set; }

    public string? RlyCd { get; set; }

    public string? VendName { get; set; }

    public string? Remarks { get; set; }

    public string? PoDoc { get; set; }

    public DateTime? Datetime { get; set; }

    public decimal? Isdeleted { get; set; }

    public string? RegionCode { get; set; }

    public string? UserId { get; set; }
}
