using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T81CrRej
{
    public string? RejDt { get; set; }

    public string? CaseNo { get; set; }

    public string? Consignee { get; set; }

    public string? DesCom { get; set; }

    public string? Conclusion { get; set; }

    public string? Region { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public int Id { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }
}
