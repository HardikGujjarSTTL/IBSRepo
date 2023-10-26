using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class CheckCall
{
    public string CaseNo { get; set; } = null!;

    public string? CallStatus { get; set; }

    public string? IcNo { get; set; }

    public DateTime? IcDt { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public DateTime CallRecvDt { get; set; }

    public short CallSno { get; set; }

    public string? IeName { get; set; }

    public string? IePhoneNo { get; set; }

    public int? VendCd { get; set; }

    public string? VendContactTel1 { get; set; }

    public string? RlyCd { get; set; }

    public byte? PendingCharges { get; set; }

    public DateTime? ExtDelvDt { get; set; }
}
