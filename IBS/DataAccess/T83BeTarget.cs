using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T83BeTarget
{
    public string BePer { get; set; } = null!;

    public decimal? BTarget { get; set; }

    public decimal? ETarget { get; set; }

    public string RegionCode { get; set; } = null!;

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public decimal? ExTarget { get; set; }

    public byte? Isdeleted { get; set; }

    public string? Createdby { get; set; }

    public string? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }
}
