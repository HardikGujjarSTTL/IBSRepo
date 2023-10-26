using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T29JvDetail
{
    public string? VchrNo { get; set; }

    public int? AccCd { get; set; }

    public decimal? Amount { get; set; }

    public string? Narration { get; set; }

    public string? IuAdvNo { get; set; }

    public DateTime? IuAdvDt { get; set; }

    public int Id { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public virtual T27Jv? VchrNoNavigation { get; set; }
}
