using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T30IcReceivedHistory
{
    public string? Region { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public int? IeCd { get; set; }

    public DateTime? IcSubmitDt { get; set; }

    public string? BillNo { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Remarks { get; set; }

    public DateTime? RemarksDt { get; set; }

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
