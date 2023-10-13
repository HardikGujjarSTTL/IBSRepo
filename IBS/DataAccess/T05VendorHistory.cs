using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T05VendorHistory
{
    public int? VendCd { get; set; }

    public string? VendName { get; set; }

    public string? VendAdd1 { get; set; }

    public string? VendAdd2 { get; set; }

    public int? VendCityCd { get; set; }

    public string? VendContactPer1 { get; set; }

    public string? VendContactTel1 { get; set; }

    public string? VendContactPer2 { get; set; }

    public string? VendContactTel2 { get; set; }

    public string? VendApproval { get; set; }

    public DateTime? VendApprovalFr { get; set; }

    public DateTime? VendApprovalTo { get; set; }

    public string? VendStatus { get; set; }

    public DateTime? VendStatusDtFr { get; set; }

    public DateTime? VendStatusDtTo { get; set; }

    public string? VendRemarks { get; set; }

    public string? VendCdAlpha { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? VendEmail { get; set; }

    public string? VendInspStopped { get; set; }

    public string? VendPwd { get; set; }

    public string? OnlineCallStatus { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public string? OfflineCallStatus { get; set; }

    public string? VendGstno { get; set; }

    public string? VendTanno { get; set; }

    public string? VendPanno { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
