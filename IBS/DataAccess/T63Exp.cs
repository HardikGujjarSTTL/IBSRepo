using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T63Exp
{
    public string RegionCode { get; set; } = null!;

    public string ExpPer { get; set; } = null!;

    public decimal? ExpAmt { get; set; }

    public decimal? TaxAmt { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Isdeleted { get; set; }

    public DateTime? Createddate { get; set; }

    public string? Createdby { get; set; }

    public DateTime? Updateddate { get; set; }

    public string? Updatedby { get; set; }
}
