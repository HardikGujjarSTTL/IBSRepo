using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T04Uom
{
    public byte UomCd { get; set; }

    public string? UomLDesc { get; set; }

    public string? UomSDesc { get; set; }

    public decimal? UomFactor { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? ImmsUomCd { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }

    public virtual ICollection<T15PoDetail> T15PoDetails { get; set; } = new List<T15PoDetail>();
}
