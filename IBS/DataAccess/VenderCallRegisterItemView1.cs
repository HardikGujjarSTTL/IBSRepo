using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class VenderCallRegisterItemView1
{
    public string? Status { get; set; }

    public int ItemSrnoPo { get; set; }

    public string? ItemDescPo { get; set; }

    public decimal? QtyOrdered { get; set; }

    public decimal? CumQtyPrevOffered { get; set; }

    public decimal? CumQtyPrevPassed { get; set; }

    public decimal? QtyToInsp { get; set; }

    public decimal? QtyPassed { get; set; }

    public decimal? QtyRejected { get; set; }

    public decimal? QtyDue { get; set; }

    public string? Consignee { get; set; }

    public string? DelvDate { get; set; }

    public string CaseNo { get; set; } = null!;

    public DateTime CallRecvDt { get; set; }

    public int CallSno { get; set; }
}
