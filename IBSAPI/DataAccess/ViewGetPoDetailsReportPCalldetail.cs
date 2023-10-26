using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewGetPoDetailsReportPCalldetail
{
    public DateTime CallRecvDt { get; set; }

    public DateTime? CallLetterDt { get; set; }

    public short CallSno { get; set; }

    public byte? CallInstallNo { get; set; }

    public string? IeName { get; set; }

    public string? CallStatus { get; set; }

    public string? ReasonReject { get; set; }

    public string? Reason { get; set; }

    public string CaseNo { get; set; } = null!;
}
