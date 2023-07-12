using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T76DocumentCatalog
{
    public string? DocType { get; set; }

    public string? DocSubType { get; set; }

    public string FileId { get; set; } = null!;

    public string? FileExt { get; set; }

    public string? DocumentName { get; set; }

    public string? DocumentNo { get; set; }

    public DateTime? IssueDt { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Region { get; set; }
}
