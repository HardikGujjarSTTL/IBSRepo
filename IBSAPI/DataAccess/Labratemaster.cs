using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class Labratemaster
{
    public decimal Labrateid { get; set; }

    public int? DisciplineId { get; set; }

    public string? TestName { get; set; }

    public int? Price { get; set; }
}
