using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T100VenderCluster
{
    public int VendorCode { get; set; }

    public string DepartmentName { get; set; } = null!;

    public byte? ClusterCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
