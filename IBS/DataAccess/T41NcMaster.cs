using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T41NcMaster
{
    public string? NcNo { get; set; }

    public DateTime? NcDt { get; set; }

    public string CaseNo { get; set; }

    public DateTime CallRecvDt { get; set; }

    public short CallSno { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public int? VendCd { get; set; }

    public int? ConsigneeCd { get; set; }

    public byte ItemSrnoPo { get; set; }

    public string? ItemDescPo { get; set; }

    public decimal? QtyPassed { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? IcNo { get; set; }

    public DateTime? IcDt { get; set; }

    public byte? IeCd { get; set; }

    public byte? CoCd { get; set; }

    public string? RegionCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual T06Consignee? ConsigneeCdNavigation { get; set; }

    public virtual T05Vendor? VendCdNavigation { get; set; }
}
