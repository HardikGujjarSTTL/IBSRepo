using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T17CallRegisterHistory
{
    public string? CaseNo { get; set; }

    public DateTime? CallRecvDt { get; set; }

    public int? CallSno { get; set; }

    public string? CallLetterNo { get; set; }

    public DateTime? CallLetterDt { get; set; }

    public DateTime? CallMarkDt { get; set; }

    public int? IeCd { get; set; }

    public DateTime? DtInspDesire { get; set; }

    public string? CallStatus { get; set; }

    public DateTime? CallStatusDt { get; set; }

    public string? CallRemarkStatus { get; set; }

    public byte? CallInstallNo { get; set; }

    public string? RegionCode { get; set; }

    public int? MfgCd { get; set; }

    public string? MfgPlace { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? UpdateAllowed { get; set; }

    public string? Remarks { get; set; }

    public string? CallCancelStatus { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public int? CoCd { get; set; }

    public short? CallCancelCharges { get; set; }

    public string? OnlineCall { get; set; }

    public string? ItemRdso { get; set; }

    public string? VendRdso { get; set; }

    public DateTime? VendApprovalFr { get; set; }

    public DateTime? VendApprovalTo { get; set; }

    public string? StaggeredDp { get; set; }

    public string? LotDp1 { get; set; }

    public string? LotDp2 { get; set; }

    public string? Hologram { get; set; }

    public DateTime? ExpInspDt { get; set; }

    public string? RejCanCall { get; set; }

    public string? DepartmentCode { get; set; }

    public string? AutomaticCall { get; set; }

    public string? FinalOrStage { get; set; }

    public string? Bpo { get; set; }

    public string? RecipientGstinNo { get; set; }

    public string? NewVendor { get; set; }

    public bool? CountDt { get; set; }

    public string? IrfcFunded { get; set; }

    public string? FifoVoilateReason { get; set; }

    public decimal? RejCharges { get; set; }

    public string? LocalOrOuts { get; set; }

    public int? ClusterCode { get; set; }

    public string? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public string? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public long Id { get; set; }

    public string? CmApproval { get; set; }

    public DateTime? CmApprovalDt { get; set; }

    public string? Isfinalizedstatus { get; set; }
}
