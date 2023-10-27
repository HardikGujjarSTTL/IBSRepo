using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T46ClaimDetail
{
    public string ClaimNo { get; set; } = null!;

    public string ClaimHead { get; set; } = null!;

    public int? AmtClaimed { get; set; }

    public int? AmtAdmitted { get; set; }

    public int? AmtDisallowed { get; set; }

    public string? Remarks { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public decimal? Id { get; set; }

    public virtual T45ClaimMaster ClaimNoNavigation { get; set; } = null!;
}
