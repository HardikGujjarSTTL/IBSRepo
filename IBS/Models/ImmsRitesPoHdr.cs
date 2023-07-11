using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class ImmsRitesPoHdr
{
    public decimal PoId { get; set; }

    public int ImmsPokey { get; set; }

    public string? RitesCaseNo { get; set; }

    public int? PurchaserCd { get; set; }

    public string? ImmsPurchaserCd { get; set; }

    public string? ImmsPurchaserDetail { get; set; }

    public string? StockNonstock { get; set; }

    public string? RlyNonrly { get; set; }

    public string? PoOrLetter { get; set; }

    public string? PoNo { get; set; }

    public string? L5noPo { get; set; }

    public DateTime? PoDt { get; set; }

    public DateTime? RecvDate { get; set; }

    public int? VendCd { get; set; }

    public string? ImmsVendorCd { get; set; }

    public string? ImmsVendorName { get; set; }

    public string? ImmsVendorCity { get; set; }

    public string? ImmsVendorDetail { get; set; }

    public string? RlyCd { get; set; }

    public string? ImmsRlyCd { get; set; }

    public string? ImmsRlyShortname { get; set; }

    public string? RegionCode { get; set; }

    public string? Remarks { get; set; }

    public string? BpoCd { get; set; }

    public string? ImmsBpoCd { get; set; }

    public string? ImmsBpoName { get; set; }

    public string? ImmsBpoDetail { get; set; }

    public string? UserId { get; set; }

    public string? InspectingAgency { get; set; }

    public int? PoiCd { get; set; }

    public string? ImmsPoiName { get; set; }

    public string? ImmsPoiDetail { get; set; }

    public byte? AckFlag { get; set; }

    public bool? Valid { get; set; }

    public DateTime? Datetime { get; set; }
}
