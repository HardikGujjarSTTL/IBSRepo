using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class Temp25RvDetail
{
    public string? RegionCode { get; set; }

    public string? BpoCd { get; set; }

    public string? VchrNo { get; set; }

    public byte? Sno { get; set; }

    public DateTime? VchrDt { get; set; }

    public byte? BankCd { get; set; }

    public string? ChqNo { get; set; }

    public DateTime? ChqDt { get; set; }

    public decimal? Amount { get; set; }

    public byte? AccCd { get; set; }

    public decimal? AmountPosted { get; set; }

    public decimal? AmtTransferred { get; set; }

    public decimal? Suspense { get; set; }
}
