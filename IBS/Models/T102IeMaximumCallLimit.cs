using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T102IeMaximumCallLimit
{
    public string RegionCode { get; set; } = null!;

    public byte? MaximumCall { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
