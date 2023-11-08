using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class V22bOutstandingBill
{
    public string? RegionCode { get; set; }

    public string? BpoType { get; set; }

    public string? BpoRegion { get; set; }

    public string? BpoRly { get; set; }

    public string? BpoCd { get; set; }

    public string? BpoName { get; set; }

    public string? BpoOrgn { get; set; }

    public string? BpoAdd { get; set; }

    public string? BpoCity { get; set; }

    public string? SapCustCdBpo { get; set; }

    public string? Au { get; set; }

    public string? LoRemarks { get; set; }

    public string BillNo { get; set; } = null!;

    public DateTime? BillDt { get; set; }

    public string? CaseNo { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public string? PoOrLetter { get; set; }

    public int? VendCd { get; set; }

    public string? VendName { get; set; }

    public string? VendorCity { get; set; }

    public int ConsigneeCd { get; set; }

    public string? Consignee { get; set; }

    public string? ConsigneeAdd1 { get; set; }

    public string? ConsigneeAdd2 { get; set; }

    public string? ConsigneeCity { get; set; }

    public int? IeCd { get; set; }

    public decimal? BillAmount { get; set; }

    public string? CnoteBillNo { get; set; }

    public string? IcNo { get; set; }

    public DateTime? IcDt { get; set; }

    public string? RecipientGstinNo { get; set; }

    public DateTime ChqDt { get; set; }

    public string? Narration { get; set; }

    public decimal? ChqAmt { get; set; }

    public string? AuDesc { get; set; }

    public decimal? Tds { get; set; }

    public decimal? TdsSgst { get; set; }

    public decimal? TdsCgst { get; set; }

    public decimal? TdsIgst { get; set; }

    public decimal? RetentionMoney { get; set; }

    public decimal? CnoteAmount { get; set; }

    public decimal? WriteOffAmt { get; set; }

    public decimal? AmountPosted { get; set; }

    public decimal? AmountRealised { get; set; }

    public decimal? AmountOutstanding { get; set; }

    public string? FinYr { get; set; }

    public string? YrMth { get; set; }
}
