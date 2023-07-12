using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T19CallCancel
{
    public string CaseNo { get; set; } = null!;

    public DateTime CallRecvDt { get; set; }

    public short CallSno { get; set; }

    public byte? CancelCd1 { get; set; }

    public byte? CancelCd2 { get; set; }

    public byte? CancelCd3 { get; set; }

    public byte? CancelCd4 { get; set; }

    public byte? CancelCd5 { get; set; }

    public byte? CancelCd6 { get; set; }

    public byte? CancelCd7 { get; set; }

    public byte? CancelCd8 { get; set; }

    public byte? CancelCd9 { get; set; }

    public byte? CancelCd10 { get; set; }

    public byte? CancelCd11 { get; set; }

    public string? CancelDesc { get; set; }

    public DateTime? CancelDate { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? DocsSubmitted { get; set; }

    public virtual T17CallRegister Ca { get; set; } = null!;
}
