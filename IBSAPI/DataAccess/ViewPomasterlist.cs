﻿using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewPomasterlist
{
    public int? VendCd { get; set; }

    public string CaseNo { get; set; } = null!;

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? RlyCd { get; set; }

    public string? RealCaseNo { get; set; }

    public string? VendName { get; set; }

    public string? ConsigneeSName { get; set; }

    public string? Remarks { get; set; }

    public decimal? Isdeleted { get; set; }

    public string? RlyNonrly { get; set; }

    public string? MainrlyCd { get; set; }

    public string? RlyCds { get; set; }

    public string? RegionCode { get; set; }
}
