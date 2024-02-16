using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class IrepsWorkPlan
{
    public int CallId { get; set; }

    public DateTime VisitDt { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual IrepsCall Call { get; set; } = null!;
}
