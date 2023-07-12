using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T105LoLogin
{
    public string? LoName { get; set; }

    public string? Designation { get; set; }

    public string Mobile { get; set; } = null!;

    public string? Email { get; set; }

    public string? Pwd { get; set; }

    public string? Status { get; set; }

    public DateTime? LoPerFr { get; set; }

    public DateTime? LoPerTo { get; set; }
}
