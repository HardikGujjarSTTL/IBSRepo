using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class RptPrmIcLine
{
    public string CaseNo { get; set; } = null!;

    public byte ItemSrnoPo { get; set; }

    public decimal? PrevQtyOffered { get; set; }

    public decimal? PrevQtyPassed { get; set; }

    public DateTime? RequestTs { get; set; }
}
