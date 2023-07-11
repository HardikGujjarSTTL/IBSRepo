using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T99ClusterMaster
{
    public byte ClusterCode { get; set; }

    public string? ClusterName { get; set; }

    public string? GeographicalPartition { get; set; }

    public string? DepartmentName { get; set; }

    public string? RegionCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? HqArea { get; set; }
}
