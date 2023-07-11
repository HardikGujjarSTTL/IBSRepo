using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T27Jv
{
    public string VchrNo { get; set; } = null!;

    public DateTime? VchrDt { get; set; }

    public string? RvVchrNo { get; set; }

    public byte? RvSno { get; set; }

    public byte? BankCd { get; set; }

    public string? ChqNo { get; set; }

    public DateTime? ChqDt { get; set; }
}
