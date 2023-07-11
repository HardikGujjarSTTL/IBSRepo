using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T62MasterItemPlno
{
    public string? ItemCd { get; set; }

    public string PlNo { get; set; } = null!;

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
