using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T28SapRealisation
{
    public string RegionCode { get; set; } = null!;

    public string RealisationPer { get; set; } = null!;

    public string ClientType { get; set; } = null!;

    public decimal? Amount { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
