using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T74ChecksheetCatalog
{
    public string ChkSheetId { get; set; } = null!;

    public string? FileExt { get; set; }

    public string? ChkSheetName { get; set; }

    public string? Discipline { get; set; }

    public string? DocumentNo { get; set; }

    public DateTime? IssueDt { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
