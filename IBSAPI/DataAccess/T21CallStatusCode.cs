using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T21CallStatusCode
{
    public string CallStatusCd { get; set; } = null!;

    public string? CallStatusDesc { get; set; }

    public string? CallStatusColor { get; set; }
}
