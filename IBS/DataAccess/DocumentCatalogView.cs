using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class DocumentCatalogView
{
    public string? DocType { get; set; }

    public string? DocSubType { get; set; }

    public decimal? FId { get; set; }

    public string FileId { get; set; } = null!;
}
