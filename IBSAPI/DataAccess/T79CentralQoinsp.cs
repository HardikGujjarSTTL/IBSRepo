using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T79CentralQoinsp
{
    public string Client { get; set; } = null!;

    public string Weight { get; set; } = null!;

    public string QoiLength { get; set; } = null!;

    public long? Accepted { get; set; }

    public long? Rejected { get; set; }

    public string QoiDate { get; set; } = null!;

    public string? Region { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public string? Grade { get; set; }
}
