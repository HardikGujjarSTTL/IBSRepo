using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class VendPoMaDetail
{
    public string CaseNo { get; set; } = null!;

    public string MaNo { get; set; } = null!;

    public DateTime MaDt { get; set; }

    public byte MaSno { get; set; }

    public string? MaField { get; set; }

    public string? MaDesc { get; set; }

    public string? OldPoValue { get; set; }

    public string? NewPoValue { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? MaStatus { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedDatetime { get; set; }

    public string? MaRemarks { get; set; }

    public virtual VendPoMaMaster VendPoMaMaster { get; set; } = null!;
}
