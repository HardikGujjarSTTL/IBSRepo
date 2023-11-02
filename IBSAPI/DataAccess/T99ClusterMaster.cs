using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T99ClusterMaster
{
    public int ClusterCode { get; set; }

    public string? ClusterName { get; set; }

    public string? GeographicalPartition { get; set; }

    public string? DepartmentName { get; set; }

    public string? RegionCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? HqArea { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }
}
