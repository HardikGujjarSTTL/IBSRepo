using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T35RailPriceDetail
{
    public byte RailId { get; set; }

    public byte IdSrno { get; set; }

    public int? RailPricePerMt { get; set; }

    public int? PackingCharge { get; set; }

    public DateTime? PriceDateFr { get; set; }

    public DateTime? PriceDateTo { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual T34RailPrice Rail { get; set; } = null!;
}
