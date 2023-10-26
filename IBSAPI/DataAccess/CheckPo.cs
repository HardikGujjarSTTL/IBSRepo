using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class CheckPo
{
    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? PoOrLetter { get; set; }

    public int? VendCd { get; set; }

    public string CaseNo { get; set; } = null!;

    public string? RefNo { get; set; }

    public string? VendContactTel1 { get; set; }

    public string? RlyCd { get; set; }

    public DateTime? ExtDelvDt { get; set; }
}
