using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class ViewGetCallinspectionPrintReport
{
    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public string? MfgName { get; set; }

    public string? MfgAdd { get; set; }

    public string? Purchaser { get; set; }

    public string? Consignee { get; set; }

    public string CaseNo { get; set; } = null!;

    public DateTime CallRecvDt { get; set; }

    public short CallSno { get; set; }

    public string? CallLetterNo { get; set; }

    public string? CallLetterDt { get; set; }

    public byte? CallInstallNo { get; set; }

    public string? OnlineCall { get; set; }

    public string? FinalOrStage { get; set; }

    public string? Remarks { get; set; }

    public string? ItemRdso { get; set; }

    public string? VendRdso { get; set; }

    public string? VendAppFr { get; set; }

    public string? VendAppTo { get; set; }

    public string? StagDp { get; set; }

    public string? LotDp1 { get; set; }

    public string? LotDp2 { get; set; }

    public string? IeName { get; set; }

    public string? ItemDescPo { get; set; }

    public decimal? QtyOrdered { get; set; }

    public decimal? QtyToInsp { get; set; }

    public decimal? CumQtyPrevOffered { get; set; }

    public decimal? CumQtyPrevPassed { get; set; }

    public string? VendContactPer1 { get; set; }

    public string? VendContactTel1 { get; set; }

    public string? VendEmail { get; set; }

    public string? Bpo { get; set; }

    public DateTime? DelvDt { get; set; }

    public string? ItemCd { get; set; }

    public string? IrfcFunded { get; set; }
}
