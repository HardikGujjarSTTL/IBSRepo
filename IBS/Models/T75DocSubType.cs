using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T75DocSubType
{
    public string DocType { get; set; } = null!;

    public string DocSubType { get; set; } = null!;

    public string? DocSubTypeDesc { get; set; }

    public virtual T74DocumentType DocTypeNavigation { get; set; } = null!;
}
