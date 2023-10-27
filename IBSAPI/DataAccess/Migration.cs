using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class Migration
{
    public int Id { get; set; }

    public string Migration1 { get; set; } = null!;

    public long Batch { get; set; }
}
