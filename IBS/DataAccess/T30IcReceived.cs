using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T30IcReceived
{
    public string Region { get; set; } = null!;

    public string BkNo { get; set; } = null!;

    public string SetNo { get; set; } = null!;

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

    public virtual T22Bill? BillNoNavigation { get; set; }
}
