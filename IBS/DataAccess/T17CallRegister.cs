using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T17CallRegister
{
    public string CaseNo { get; set; } = null!;

    public DateTime CallRecvDt { get; set; }

    public short CallSno { get; set; }

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

    public byte? CoCd { get; set; }

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

    public byte? ClusterCode { get; set; }

    public string? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public string? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public virtual T09Ie? IeCdNavigation { get; set; }

    public virtual T05Vendor? MfgCdNavigation { get; set; }

    public virtual T01Region? RegionCodeNavigation { get; set; }

    public virtual T19CallCancel? T19CallCancel { get; set; }

    public virtual ICollection<T20Ic> T20Ics { get; set; } = new List<T20Ic>();

    public virtual T44SuperSurprise? T44SuperSurprise { get; set; }

    public virtual ICollection<T47IeWorkPlan> T47IeWorkPlans { get; set; } = new List<T47IeWorkPlan>();

    public virtual ICollection<T50LabRegister> T50LabRegisters { get; set; } = new List<T50LabRegister>();
}
