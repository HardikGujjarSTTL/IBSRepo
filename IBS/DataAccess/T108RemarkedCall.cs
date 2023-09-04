using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T108RemarkedCall
{
    public string? CaseNo { get; set; }

    public DateTime? CallRecvDt { get; set; }

    public short? CallSno { get; set; }

    public string? CallRemarkStatus { get; set; }

    public string? RemarkReason { get; set; }

    public byte? FrIeCd { get; set; }

    public byte? ToIeCd { get; set; }

    public string? RemInitBy { get; set; }

    public DateTime? RemInitDatetime { get; set; }

    public string? RemAppBy { get; set; }

    public DateTime? RemAppDatetime { get; set; }

    public string? RemarkingStatus { get; set; }

    public string? RemRejRemark { get; set; }

    public byte? FrIePendingCalls { get; set; }

    public byte? ToIePendingCalls { get; set; }

    public int Id { get; set; }
}
