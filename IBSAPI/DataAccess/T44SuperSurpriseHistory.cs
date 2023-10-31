using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T44SuperSurpriseHistory
{
    public string? SuperSurpriseNo { get; set; }

    public DateTime? SuperSurpriseDt { get; set; }

    public string? CaseNo { get; set; }

    public DateTime? CallRecvDt { get; set; }

    public int? CallSno { get; set; }

    public int? IeCd { get; set; }

    public int? CoCd { get; set; }

    public string? ItemDesc { get; set; }

    public string? Discrepancy { get; set; }

    public string? Outcome { get; set; }

    public string? PreIntRej { get; set; }

    public string? NameScopeItem { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? SbuHeadRemarks { get; set; }

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
