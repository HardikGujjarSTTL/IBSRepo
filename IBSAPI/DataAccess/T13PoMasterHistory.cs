using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T13PoMasterHistory
{
    public string? CaseNo { get; set; }

    public int? PurchaserCd { get; set; }

    public string? StockNonstock { get; set; }

    public string? RlyNonrly { get; set; }

    public string? PoOrLetter { get; set; }

    public string? PoNo { get; set; }

    public string? L5noPo { get; set; }

    public DateTime? PoDt { get; set; }

    public DateTime? RecvDt { get; set; }

    public int? VendCd { get; set; }

    public string? RlyCd { get; set; }

    public string? RegionCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Remarks { get; set; }

    public string? InspectingAgency { get; set; }

    public int? PoiCd { get; set; }

    public string? PoSource { get; set; }

    public byte? PendingCharges { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Updateddate { get; set; }

    public int? Updatedby { get; set; }

    public byte? Isdeleted { get; set; }

    public byte? Ispricevariation { get; set; }

    public byte? Isstageinspection { get; set; }

    public int? Contractid { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
