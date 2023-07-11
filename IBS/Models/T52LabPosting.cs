using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T52LabPosting
{
    public string? SampleRegNo { get; set; }

    public byte? BankCd { get; set; }

    public string? ChqNo { get; set; }

    public DateTime? ChqDt { get; set; }

    public decimal? AmtCleared { get; set; }

    public long? TotalLabCharges { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual T25RvDetail? T25RvDetail { get; set; }
}
