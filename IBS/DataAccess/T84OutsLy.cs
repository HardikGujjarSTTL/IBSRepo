using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T84OutsLy
{
    public string LyPer { get; set; } = null!;

    public decimal? LyOuts { get; set; }

    public string RegionCode { get; set; } = null!;

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public byte? Isdeleted { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }
}
