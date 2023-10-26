using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T16IcCancel
{
    public string BkNo { get; set; } = null!;

    public string SetNo { get; set; } = null!;

    public int? IssueToIecd { get; set; }

    public string? IcStatus { get; set; }

    public DateTime? StatusDt { get; set; }

    public string Region { get; set; } = null!;

    public string? Remarks { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public byte? Status { get; set; }

    public int? Id { get; set; }

    public virtual T09Ie? IssueToIecdNavigation { get; set; }
}
