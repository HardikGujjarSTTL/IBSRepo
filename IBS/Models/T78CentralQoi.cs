using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T78CentralQoi
{
    public long? TotalQtyDispatched { get; set; }

    public long? NoOfIcIssued { get; set; }

    public string Client { get; set; } = null!;

    public string QtyDate { get; set; } = null!;

    public string? RegionCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
