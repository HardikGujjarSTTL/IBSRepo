using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class ViewGetCallinspectionPrintSubReport
{
    public int? VendCd { get; set; }

    public string? VendName { get; set; }

    public string? VendAddress { get; set; }

    public string? VendEmail { get; set; }

    public string? VendContactPer1 { get; set; }

    public string? VendContactTel1 { get; set; }

    public string? Source { get; set; }

    public string CaseNo { get; set; } = null!;
}
