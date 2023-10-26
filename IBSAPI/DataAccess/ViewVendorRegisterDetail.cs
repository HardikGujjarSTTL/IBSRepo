using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewVendorRegisterDetail
{
    public string CaseNo { get; set; } = null!;

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

    public DateTime? DelvDate { get; set; }

    public string? Bpo { get; set; }

    public int ConsigneeCd { get; set; }
}
