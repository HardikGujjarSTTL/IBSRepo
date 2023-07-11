using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class V252709
{
    public string? Region { get; set; }

    public int? VendCd { get; set; }

    public string? VendName { get; set; }

    public byte BankCd { get; set; }

    public string ChqNo { get; set; } = null!;

    public DateTime ChqDt { get; set; }

    public decimal? Amount { get; set; }

    public decimal? AmountAdjusted { get; set; }

    public decimal? Balance { get; set; }

    public DateTime? VchrDt { get; set; }

    public string? CaseNo { get; set; }
}
