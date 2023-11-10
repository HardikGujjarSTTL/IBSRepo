using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T18CallDetailsHistory
{
    public string? CaseNo { get; set; }

    public DateTime? CallRecvDt { get; set; }

    public int? CallSno { get; set; }

    public int? ItemSrnoPo { get; set; }

    public string? ItemDescPo { get; set; }

    public int? ConsigneeCd { get; set; }

    public decimal? QtyOrdered { get; set; }

    public decimal? CumQtyPrevOffered { get; set; }

    public decimal? CumQtyPrevPassed { get; set; }

    public decimal? QtyToInsp { get; set; }

    public decimal? QtyPassed { get; set; }

    public decimal? QtyRejected { get; set; }

    public decimal? QtyDue { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public string? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public decimal? Isdeleted { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }

    public string? SuppBFlag { get; set; }

    public decimal? SuppNewRate { get; set; }
}
