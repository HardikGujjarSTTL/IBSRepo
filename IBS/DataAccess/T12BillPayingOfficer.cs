using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T12BillPayingOfficer
{
    public string BpoCd { get; set; } = null!;

    public string? BpoRegion { get; set; }

    public string? BpoType { get; set; }

    public string? BpoName { get; set; }

    public string? BpoRly { get; set; }

    public string? BpoAdd { get; set; }

    public int? BpoCityCd { get; set; }

    public string? BillPassOfficer { get; set; }

    public string? BpoFeeType { get; set; }

    public decimal? BpoFee { get; set; }

    public string? BpoTaxType { get; set; }

    public string? BpoFlg { get; set; }

    public string? BpoAdvFlg { get; set; }

    public string? BpoLocCd { get; set; }

    public string? BpoOrgn { get; set; }

    public string? BpoAdd1 { get; set; }

    public string? BpoAdd2 { get; set; }

    public string? BpoState { get; set; }

    public string? BpoPin { get; set; }

    public string? BpoPhone { get; set; }

    public string? BpoFax { get; set; }

    public string? BpoEmail { get; set; }

    public string? PayWindowId { get; set; }

    public string? BpoCdOld { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? GstinNo { get; set; }

    public string? Au { get; set; }

    public string? SapCustCdBpo { get; set; }

    public string? LegalName { get; set; }

    public string? PinCode { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }

    public string? Status { get; set; }

    public virtual T01Region? BpoRegionNavigation { get; set; }

    public virtual ICollection<T14PoBpo> T14PoBpos { get; set; } = new List<T14PoBpo>();

    public virtual ICollection<T20Ic> T20Ics { get; set; } = new List<T20Ic>();
}
