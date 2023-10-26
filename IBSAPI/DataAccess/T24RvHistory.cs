using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T24RvHistory
{
    public string? VchrNo { get; set; }

    public DateTime? VchrDt { get; set; }

    public int? BankCd { get; set; }

    public string? VchrType { get; set; }

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
