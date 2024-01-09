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

    public DateTime? DesireDt { get; set; }

    public string? IeName { get; set; }

    public string? IePhoneNo { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string CaseNo { get; set; } = null!;

    public DateTime? CallLetterDt { get; set; }

    public string? CallLetterNo { get; set; }

    public string? CallStatus1 { get; set; }

    public string? CallStatus { get; set; }

    public DateTime? CallStatusDt { get; set; }

    public string? CallCancelStatus { get; set; }

    public int? CallCancelCharges { get; set; }

    public string? CallCancelChargesStatus { get; set; }

    public decimal? CallCancelAmount { get; set; }

    public string? CallCancelChargesStatus { get; set; }

    public decimal? CallCancelAmount { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public string? Remarks { get; set; }

    public string? Hologram { get; set; }

    public string? UpdateAllowed { get; set; }

    public string? MfgPers { get; set; }

    public string? MfgPhone { get; set; }

    public int CallSno { get; set; }

    public decimal? Count { get; set; }

    public int ItemSrnoPo { get; set; }
}
