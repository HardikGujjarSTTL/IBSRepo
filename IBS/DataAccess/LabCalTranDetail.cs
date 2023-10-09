using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class LabCalTranDetail
{
    public int CalTranDetailId { get; set; }

    public int? CalTranHeaderId { get; set; }

    public int? DisciplineId { get; set; }

    public string? TestName { get; set; }

    public int? Price { get; set; }

    public int? Qty { get; set; }
}
