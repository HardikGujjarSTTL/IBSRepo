using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewGetBpodetail
{
    public string BpoCd { get; set; } = null!;

    public string? BpoName { get; set; }

    public string? BpoRly { get; set; }

    public string? BpoAdd { get; set; }

    public string? GstinNo { get; set; }

    public string? Audesc { get; set; }

    public string? City { get; set; }

    public string? SapCustCdBpo { get; set; }

    public byte? Isdeleted { get; set; }
}
