using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T102IeMaximumCallLimit
{
    public string RegionCode { get; set; } = null!;

    public byte? MaximumCall { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }
}
