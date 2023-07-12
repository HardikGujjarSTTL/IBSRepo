using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T101IeCluster
{
    public int? IeCode { get; set; }

    public string DepartmentCode { get; set; } = null!;

    public byte ClusterCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
