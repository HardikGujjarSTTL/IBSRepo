using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T74DocumentType
{
    public string DocType { get; set; } = null!;

    public string? DocTypeDesc { get; set; }

    public virtual ICollection<T75DocSubType> T75DocSubTypes { get; set; } = new List<T75DocSubType>();
}
