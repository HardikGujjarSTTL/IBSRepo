using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T85BillingAdjustementPcdo
{
    public string RegionCode { get; set; } = null!;

    public string AdjusmentYrMth { get; set; } = null!;

    public decimal? AdjustmentAmt { get; set; }

    public string? Remarks { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
