using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class ViewGetCallStatusDetail
{
    public string? VendName { get; set; }

    public string? Consignee { get; set; }

    public string? ItemDescPo { get; set; }

    public DateTime? CallMarkDt { get; set; }

    public DateTime CallRecvDt { get; set; }

    public string? IeName { get; set; }

    public string? IePhoneNo { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string CaseNo { get; set; } = null!;

    public string? CallStatus1 { get; set; }

    public string? CallStatus { get; set; }

    public string? UpdateAllowed { get; set; }

    public string? MfgPers { get; set; }

    public string? MfgPhone { get; set; }

    public short CallSno { get; set; }

    public decimal? Count { get; set; }

    public int ItemSrnoPo { get; set; }
}
