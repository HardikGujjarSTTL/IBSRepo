using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T69NcCode
{
    public string NcCd { get; set; } = null!;

    public string? NcDesc { get; set; }

    public string? NcClass { get; set; }

    public virtual ICollection<T42NcDetail> T42NcDetails { get; set; } = new List<T42NcDetail>();
}
