using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T101IeClusterHistory
{
    public int? IeCode { get; set; }

    public string? DepartmentCode { get; set; }

    public int? ClusterCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
