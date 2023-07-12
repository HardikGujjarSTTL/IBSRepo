using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T88SapBilling
{
    public string RegionCode { get; set; } = null!;

    public string SapBillPer { get; set; } = null!;

    public decimal? InspFee { get; set; }

    public decimal? Cgst { get; set; }

    public decimal? Sgst { get; set; }

    public decimal? Igst { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
