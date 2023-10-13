using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T27JvHistory
{
    public string? VchrNo { get; set; }

    public DateTime? VchrDt { get; set; }

    public string? RvVchrNo { get; set; }

    public byte? RvSno { get; set; }

    public byte? BankCd { get; set; }

    public string? ChqNo { get; set; }

    public DateTime? ChqDt { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
