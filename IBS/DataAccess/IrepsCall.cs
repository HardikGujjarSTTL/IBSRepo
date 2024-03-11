using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class IrepsCall
{
    public int CallId { get; set; }

    public string? Zone { get; set; }

    public byte? IeCd { get; set; }

    public byte? CoCd { get; set; }

    public DateTime? CallDt { get; set; }

    public int? VendCd { get; set; }

    public string? VendDesc { get; set; }

    public int? PoiCd { get; set; }

    public string? PoiDesc { get; set; }

    public string? CallStatus { get; set; }

    public DateTime? IcDt { get; set; }

    public decimal? PoValue { get; set; }

    public decimal? InspValue { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public decimal? InspFee { get; set; }

    public int? IcNo { get; set; }

    public byte? PoiStateCd { get; set; }

    public virtual ICollection<IrepsWorkPlan> IrepsWorkPlans { get; set; } = new List<IrepsWorkPlan>();
}
