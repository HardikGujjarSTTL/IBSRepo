using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T109LabSampleInfoHistory
{
    public string? CaseNo { get; set; }

    public DateTime? CallRecvDt { get; set; }

    public int? CallSno { get; set; }

    public byte? IeCd { get; set; }

    public string? Status { get; set; }

    public DateTime? SampleRecvDt { get; set; }

    public long? TestingCharges { get; set; }

    public DateTime? LikelyDtReport { get; set; }

    public string? Remarks { get; set; }

    public string? DepositSlipUploaded { get; set; }

    public string? FeeDepositConfirm { get; set; }

    public DateTime? LabRepUploadedDt { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
