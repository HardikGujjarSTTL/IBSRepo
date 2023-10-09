using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class TestTable
{
    public int Labrateid { get; set; }

    public int? DisciplineId { get; set; }

    public string? TestName { get; set; }

    public int? Price { get; set; }
}
