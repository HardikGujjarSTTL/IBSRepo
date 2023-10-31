using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T27Jv
{
    public string VchrNo { get; set; } = null!;

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

    public virtual ICollection<T29JvDetail> T29JvDetails { get; set; } = new List<T29JvDetail>();
}
