using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T78CentralQoi
{
    public long? TotalQtyDispatched { get; set; }

    public long? NoOfIcIssued { get; set; }

    public string Client { get; set; } = null!;

    public string QtyDate { get; set; } = null!;

    public string? RegionCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }
}
