using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class WriteOffDetail
{
    public int? Id { get; set; }

    public int WriteOffMasterId { get; set; }

    public string BillNo { get; set; } = null!;

    public decimal WriteOffAmount { get; set; }
}
