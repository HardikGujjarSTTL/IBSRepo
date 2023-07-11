using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T34RailPrice
{
    public byte RailId { get; set; }

    public string? RailDesc { get; set; }

    public byte? RailLengthMeter { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual ICollection<T35RailPriceDetail> T35RailPriceDetails { get; set; } = new List<T35RailPriceDetail>();
}
