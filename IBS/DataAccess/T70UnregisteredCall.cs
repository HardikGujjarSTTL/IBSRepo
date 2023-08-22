using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T70UnregisteredCall
{
    public int IeCd { get; set; }

    public string? YrMth { get; set; }

    public byte? UnregCalls { get; set; }

    public string? Region { get; set; }

    public string? Createdby { get; set; }

    public string? Updatedby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public decimal? Isdeleted { get; set; }

    public virtual T09Ie IeCdNavigation { get; set; } = null!;
}
