using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T89Gst
{
    public decimal? IgstRate { get; set; }

    public decimal? SgstRate { get; set; }

    public decimal? CgstRate { get; set; }

    public DateTime? DtFrom { get; set; }

    public DateTime? DtTo { get; set; }
}
