using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T02User
{
    public string UserId { get; set; } = null!;

    public string? UserName { get; set; }

    public string? RitesEmp { get; set; }

    public string? EmpNo { get; set; }

    public string? Region { get; set; }

    public string? Password { get; set; }

    public byte? AuthLevl { get; set; }

    public string? Status { get; set; }

    public string? AllowPo { get; set; }

    public string? AllowUpChksht { get; set; }

    public string? AllowDnChksht { get; set; }

    public string? CallMarking { get; set; }

    public string? CallRemarking { get; set; }

    public string? UserType { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public string? Createdby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public string? Updatedby { get; set; }

    public decimal? Isdeleted { get; set; }

    public int? Id { get; set; }

    public string? Migtype { get; set; }

    public string? Mobile { get; set; }

    public int? CoCd { get; set; }

    public string? UserEmail { get; set; }
}
