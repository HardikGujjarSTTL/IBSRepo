using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class RptPrmInspectionCertificate
{
    public string CaseNo { get; set; } = null!;

    public DateTime CallRecvDt { get; set; }

    public short CallSno { get; set; }

    public short? NumVisits { get; set; }

    public string? VisitDates { get; set; }

    public DateTime? RequestTs { get; set; }

    public int ConsigneeCd { get; set; }
}
