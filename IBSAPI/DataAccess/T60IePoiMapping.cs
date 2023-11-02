using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T60IePoiMapping
{
    public int? IeCd { get; set; }

    public int? PoiCd { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public int Id { get; set; }

    public virtual T09Ie? IeCdNavigation { get; set; }

    public virtual T05Vendor? PoiCdNavigation { get; set; }
}
