using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class V40ConsigneeComplaint
{
    public string ComplaintId { get; set; } = null!;

    public DateTime? ComplaintDt { get; set; }

    public string? RejMemoNo { get; set; }

    public DateTime? RejMemoDt { get; set; }

    public string? CaseNo { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? L5noPo { get; set; }

    public string? BpoType { get; set; }

    public string? RlyCd { get; set; }

    public string? IcNo { get; set; }

    public DateTime? IcDt { get; set; }

    public string? InspRegion { get; set; }

    public string? InspRegionName { get; set; }

    public int? IeCd { get; set; }

    public string? IeName { get; set; }

    public int? IeCoCd { get; set; }

    public string? IeCoName { get; set; }

    public string? CompRecvRegion { get; set; }

    public string? CompRecvRegnName { get; set; }

    public int? ConsigneeCd { get; set; }

    public string? ConsigneeName { get; set; }

    public string? ConsigneeAddr { get; set; }

    public string? ConsigneeCity { get; set; }

    public int? VendCd { get; set; }

    public string? Vendor { get; set; }

    public int? ItemSrnoPo { get; set; }

    public string? ItemDesc { get; set; }

    public decimal? QtyOffered { get; set; }

    public decimal? QtyRejected { get; set; }

    public int? UomCd { get; set; }

    public string? UomSDesc { get; set; }

    public decimal? Rate { get; set; }

    public decimal? RejectionValue { get; set; }

    public string? RejectionReason { get; set; }

    public string? Status { get; set; }

    public string? JiRequired { get; set; }

    public string? JiRegion { get; set; }

    public byte? JiIeCd { get; set; }

    public string? Remarks { get; set; }

    public string? JiApprovedBy { get; set; }

    public DateTime? JiApprovalDt { get; set; }

    public string? JiSno { get; set; }

    public DateTime? JiFixDt { get; set; }

    public string? DefectCd { get; set; }

    public byte? JiStatusCd { get; set; }

    public DateTime? JiDt { get; set; }

    public DateTime? ConclusionDt { get; set; }

    public string? ActionProposed { get; set; }

    public DateTime? ActionProposedDt { get; set; }

    public string? PenaltyType { get; set; }

    public DateTime? PenaltyDt { get; set; }

    public string? Action { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? IeJiRemarks { get; set; }

    public DateTime? IeJiRemarksDt { get; set; }

    public string? JiIeRemarks { get; set; }

    public DateTime? JiIeRemarksDt { get; set; }

    public string? NoJiReason { get; set; }

    public string? RootCauseAnalysis { get; set; }

    public string? TechRef { get; set; }

    public string? ChksheetStatus { get; set; }

    public string? AnyOther { get; set; }

    public string? CapaStatus { get; set; }

    public string? DandarStatus { get; set; }
}
