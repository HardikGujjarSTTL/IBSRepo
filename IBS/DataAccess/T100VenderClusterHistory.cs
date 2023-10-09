using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T100VenderClusterHistory
{
    public int? VendorCode { get; set; }

    public string? DepartmentName { get; set; }

    public int? ClusterCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
