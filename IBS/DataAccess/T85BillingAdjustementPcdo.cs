using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T85BillingAdjustementPcdo
{
    public string RegionCode { get; set; } = null!;

    public string AdjusmentYrMth { get; set; } = null!;

    public decimal? AdjustmentAmt { get; set; }

    public string? Remarks { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public bool? Isdeleted { get; set; }

    public string? Createdby { get; set; }

    public string? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }
}
