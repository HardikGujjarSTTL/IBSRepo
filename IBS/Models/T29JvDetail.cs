using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T29JvDetail
{
    public string? VchrNo { get; set; }

    public byte? AccCd { get; set; }

    public decimal? Amount { get; set; }

    public string? Narration { get; set; }

    public string? IuAdvNo { get; set; }

    public DateTime? IuAdvDt { get; set; }

    public virtual T27Jv? VchrNoNavigation { get; set; }
}
