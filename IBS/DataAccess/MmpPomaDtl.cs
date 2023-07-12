using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class MmpPomaDtl
{
    public string Rly { get; set; } = null!;

    public int Makey { get; set; }

    public byte Slno { get; set; }

    public string? MaFld { get; set; }

    public string? MaFldDescr { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string? NewValueInd { get; set; }

    public string? NewValueFlag { get; set; }

    public string? PlNo { get; set; }

    public string? PoSr { get; set; }

    public byte? ExpSr { get; set; }

    public string? ExpCode { get; set; }

    public byte? CondSlno { get; set; }

    public string? CondNo { get; set; }

    public string? CondCode { get; set; }

    public string? Status { get; set; }

    public string? MaSrNo { get; set; }

    public DateTime? OrigDp { get; set; }

    public string? PaymentYear { get; set; }

    public string? MaStatus { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedDatetime { get; set; }
}
