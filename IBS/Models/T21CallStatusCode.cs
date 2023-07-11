using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T21CallStatusCode
{
    public string CallStatusCd { get; set; } = null!;

    public string? CallStatusDesc { get; set; }

    public string? CallStatusColor { get; set; }
}
