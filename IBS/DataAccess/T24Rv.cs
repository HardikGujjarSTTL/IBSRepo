using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T24Rv
{
    public string VchrNo { get; set; } = null!;

    public DateTime? VchrDt { get; set; }

    public byte? BankCd { get; set; }

    public string? VchrType { get; set; }

    public virtual ICollection<T25RvDetail> T25RvDetails { get; set; } = new List<T25RvDetail>();
}
