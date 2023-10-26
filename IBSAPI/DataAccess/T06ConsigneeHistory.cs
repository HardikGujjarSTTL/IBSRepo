using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T06ConsigneeHistory
{
    public int? ConsigneeCd { get; set; }

    public string? ConsigneeType { get; set; }

    public string? ConsigneeDesig { get; set; }

    public string? ConsigneeDept { get; set; }

    public string? ConsigneeFirm { get; set; }

    public string? ConsigneeAdd1 { get; set; }

    public string? ConsigneeAdd2 { get; set; }

    public int? ConsigneeCity { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? GstinNo { get; set; }

    public string? SapCustCdCon { get; set; }

    public string? LegalName { get; set; }

    public string? PinCode { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
