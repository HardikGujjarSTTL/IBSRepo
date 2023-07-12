using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T60IePoiMapping
{
    public int? IeCd { get; set; }

    public int? PoiCd { get; set; }

    public virtual T09Ie? IeCdNavigation { get; set; }

    public virtual T05Vendor? PoiCdNavigation { get; set; }
}
