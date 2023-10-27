using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T98Servicetax
{
    public decimal? StaxRate { get; set; }

    public decimal? EduCess { get; set; }

    public decimal? SheCess { get; set; }

    public decimal? NetStaxRate { get; set; }

    public DateTime? DtFrom { get; set; }

    public DateTime? DtTo { get; set; }

    public decimal? SwachhBharatCess { get; set; }

    public decimal? KrishiKalyanCess { get; set; }
}
