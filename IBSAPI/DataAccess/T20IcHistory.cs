using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T20IcHistory
{
    public string? CaseNo { get; set; }

    public DateTime? CallRecvDt { get; set; }

    public int? CallSno { get; set; }

    public int? IcTypeId { get; set; }

    public int? ConsigneeCd { get; set; }

    public string? BpoCd { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public int? IeCd { get; set; }

    public string? IcNo { get; set; }

    public DateTime? IcDt { get; set; }

    public DateTime? CallDt { get; set; }

    public int? CallInstallNo { get; set; }

    public string? FullPart { get; set; }

    public int? NoOfInsp { get; set; }

    public DateTime? FirstInspDt { get; set; }

    public DateTime? LastInspDt { get; set; }

    public string? OtherInspDt { get; set; }

    public string? StampPattern { get; set; }

    public string? ReasonReject { get; set; }

    public DateTime? IcSubmitDt { get; set; }

    public string? BillNo { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Photo { get; set; }

    public int? CoCd { get; set; }

    public string? StampPatternCd { get; set; }

    public string? RecipientGstinNo { get; set; }

    public string? AccGroup { get; set; }

    public string? IrfcFunded { get; set; }

    public string? IrfcBpoCd { get; set; }

    public string? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public string? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public decimal? Isdeleted { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
