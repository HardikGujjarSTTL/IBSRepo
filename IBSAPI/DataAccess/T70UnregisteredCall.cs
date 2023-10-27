using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T70UnregisteredCall
{
    public int IeCd { get; set; }

    public string? YrMth { get; set; }

    public byte? UnregCalls { get; set; }

    public string? Region { get; set; }

    public virtual T09Ie IeCdNavigation { get; set; } = null!;
}
