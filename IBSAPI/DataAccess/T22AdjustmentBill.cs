using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T22AdjustmentBill
{
    public int Aid { get; set; }

    public string? BillNoN { get; set; }

    public string? BillNoO { get; set; }

    public string? CaseNo { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Billadtype { get; set; }
}
