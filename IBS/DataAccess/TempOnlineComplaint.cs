using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class TempOnlineComplaint
{
    public string? TempComplaintId { get; set; }

    public DateTime? TempComplaintDt { get; set; }

    public string? ConsigneeName { get; set; }

    public string? ConsigneeDesig { get; set; }

    public string? ConsigneeEmail { get; set; }

    public string? ConsigneeMobile { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public string? CaseNo { get; set; }

    public int? VendCd { get; set; }

    public int? ConsigneeCd { get; set; }

    public byte? IeCd { get; set; }

    public byte? CoCd { get; set; }

    public string? InspRegion { get; set; }

    public string? RejMemoNo { get; set; }

    public DateTime? RejMemoDt { get; set; }

    public byte? ItemSrnoPo { get; set; }

    public string? ItemDesc { get; set; }

    public decimal? QtyOffered { get; set; }

    public decimal? QtyRejected { get; set; }

    public byte? UomCd { get; set; }

    public decimal? Rate { get; set; }

    public decimal? RejectionValue { get; set; }

    public string? RejectionReason { get; set; }

    public string? Remarks { get; set; }

    public string? Status { get; set; }

    public string? ComplaintId { get; set; }

    public string? TempCompRejReason { get; set; }
}
