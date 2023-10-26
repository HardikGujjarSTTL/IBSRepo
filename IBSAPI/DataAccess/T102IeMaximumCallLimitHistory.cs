using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T102IeMaximumCallLimitHistory
{
    public decimal Id { get; set; }

    public string? RegionCode { get; set; }

    public byte? MaximumCall { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }
}
