using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T45ClaimMaster
{
    public string ClaimNo { get; set; } = null!;

    public DateTime? ClaimDt { get; set; }

    public DateTime? ReceiveDt { get; set; }

    public int? IeCd { get; set; }

    public int? PeriodFrom { get; set; }

    public int? PeriodTo { get; set; }

    public string? RegionCode { get; set; }

    public string? PaymentVchrNo { get; set; }

    public DateTime? PaymentVchrDt { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public decimal Id { get; set; }

    public virtual T09Ie? IeCdNavigation { get; set; }

    public virtual ICollection<T46ClaimDetail> T46ClaimDetails { get; set; } = new List<T46ClaimDetail>();
}
