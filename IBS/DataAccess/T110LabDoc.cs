using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T110LabDoc
{
    public string? CaseNo { get; set; }

    public DateTime? CallRecvDt { get; set; }

    public int? CallSno { get; set; }

    public long? TestingCharges { get; set; }

    public string? DocStatusVend { get; set; }

    public DateTime? DocInitDatetime { get; set; }

    public string? DocUpdBy { get; set; }

    public string? DocStatusFin { get; set; }

    public DateTime? DocAppDatetime { get; set; }

    public string? DocAppBy { get; set; }

    public string? DocRejRemark { get; set; }

    public int? Tds { get; set; }

    public string? UtrNo { get; set; }

    public DateTime? UtrDt { get; set; }

    public virtual T17CallRegister? Ca { get; set; }
}
