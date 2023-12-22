using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class IcIntermediate
{
    public string CaseNo { get; set; } = null!;

    public DateTime CallRecvDt { get; set; }

    public short CallSno { get; set; }

    public string BkNo { get; set; } = null!;

    public string SetNo { get; set; } = null!;

    public int ConsigneeCd { get; set; }

    public string? File1 { get; set; }

    public string? File2 { get; set; }

    public string? File3 { get; set; }

    public string? File4 { get; set; }

    public string? File5 { get; set; }

    public string? File6 { get; set; }

    public string? File7 { get; set; }

    public string? File8 { get; set; }

    public string? File9 { get; set; }

    public string? File10 { get; set; }

    public int ItemSrnoPo { get; set; }

    public string? ItemDescPo { get; set; }

    public decimal? QtyOrdered { get; set; }

    public decimal? CumQtyPrevOffered { get; set; }

    public decimal? CumQtyPrevPassed { get; set; }

    public decimal? QtyToInsp { get; set; }

    public decimal? QtyPassed { get; set; }

    public decimal? QtyRejected { get; set; }

    public decimal? QtyDue { get; set; }

    public string? ConsgnCallStatus { get; set; }

    public string? PoNo { get; set; }

    public DateTime? LabTstRectDt { get; set; }

    public string? ReasonOfRejection { get; set; }

    public string? Remark { get; set; }

    public string? Hologram { get; set; }

    public string? IeStamp { get; set; }

    public string? IeStamp2 { get; set; }

    public byte? IeStampCd { get; set; }

    public string? VisitsDates { get; set; }

    public string? Amendment1 { get; set; }

    public string? Amendment2 { get; set; }

    public string? Amendment3 { get; set; }

    public string? Amendment4 { get; set; }

    public string? ItemRemark { get; set; }

    public byte[]? IeStampImage { get; set; }

    public byte[]? IeStampImage1 { get; set; }

    public int? IeCd { get; set; }

    public int? NumVisits { get; set; }

    public string? IeStampsDetail { get; set; }

    public string? IeStampsDetail2 { get; set; }

    public string? PassedInstNo { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? ConsigneeDtl { get; set; }

    public string? BpoDtl { get; set; }

    public string? PurDtl { get; set; }

    public string? PurAutDtl { get; set; }

    public string? OffInstNoDtl { get; set; }

    public string? UnitDtl { get; set; }

    public string? DispatchPackingNo { get; set; }

    public string? InvoiceNo { get; set; }

    public string? NameOfIe { get; set; }

    public string? GovBillAuth { get; set; }

    public string? ManType { get; set; }

    public string? ConsigneeDesg { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }
}
