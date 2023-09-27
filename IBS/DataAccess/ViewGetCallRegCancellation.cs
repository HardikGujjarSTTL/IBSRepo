using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class ViewGetCallRegCancellation
{
    public string CaseNo { get; set; } = null!;

    public DateTime CallRecvDt { get; set; }

    public byte? CallInstallNo { get; set; }

    public int CallSno { get; set; }

    public string? CallStatus { get; set; }

    public string? CallLetterNo { get; set; }

    public string? Remarks { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? IeSname { get; set; }

    public string? Vendor { get; set; }

    public string? RegionCode { get; set; }
}
