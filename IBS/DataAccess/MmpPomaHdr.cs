using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class MmpPomaHdr
{
    public string Rly { get; set; } = null!;

    public int Makey { get; set; }

    public DateTime? MakeyDate { get; set; }

    public int? Pokey { get; set; }

    public string? PoNo { get; set; }

    public string? MaNo { get; set; }

    public DateTime? MaDate { get; set; }

    public string? MaType { get; set; }

    public string? Vcode { get; set; }

    public string? Subject { get; set; }

    public string? RefNo { get; set; }

    public DateTime? RefDate { get; set; }

    public string? Remarks { get; set; }

    public string? MaSignOff { get; set; }

    public int? RequestId { get; set; }

    public long? AuthSeq { get; set; }

    public long? AuthSeqFin { get; set; }

    public string? Curuser { get; set; }

    public string? CuruserInd { get; set; }

    public long? SignId { get; set; }

    public long? ReqId { get; set; }

    public string? FinStatus { get; set; }

    public string? RecInd { get; set; }

    public string? Flag { get; set; }

    public string? Status { get; set; }

    public string? PurDiv { get; set; }

    public string? PurSec { get; set; }

    public long? OldPoValue { get; set; }

    public long? NewPoValue { get; set; }

    public string? PoMaSrno { get; set; }

    public string? PublishFlag { get; set; }

    public DateTime? Sent4vet { get; set; }

    public DateTime? VetDate { get; set; }

    public string? VetBy { get; set; }
}
