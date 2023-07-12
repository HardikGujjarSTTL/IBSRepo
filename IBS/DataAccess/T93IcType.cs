using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T93IcType
{
    public bool IcTypeId { get; set; }

    public string? IcType { get; set; }

    public virtual ICollection<T20Ic> T20Ics { get; set; } = new List<T20Ic>();
}
