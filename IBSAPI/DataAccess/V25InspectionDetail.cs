using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class V25InspectionDetail
{
    public string? Consignee { get; set; }

    public int ConsigneeCd { get; set; }

    public int? MfgCd { get; set; }

    public int ItemSrnoPo { get; set; }

    public string? ItemDescPo { get; set; }

    public string? IcNo { get; set; }

    public string? IcDt { get; set; }

    public string? BpoCd { get; set; }

    public string? PoNo { get; set; }

    public string? PoDt { get; set; }

    public int? PurchaserCd { get; set; }

    public decimal? MaterialValue { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public int IeCd { get; set; }

    public string? IeName { get; set; }

    public decimal? BillAmount { get; set; }
}
