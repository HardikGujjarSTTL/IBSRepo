using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T13T15View
{
    public string Caseid { get; set; } = null!;

    public DateTime? RecvDt { get; set; }

    public decimal? Value { get; set; }
}
