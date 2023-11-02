using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T02UsersHistory
{
    public string? UserId { get; set; }

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

    public string? Migtype { get; set; }

    public string? Mobile { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
