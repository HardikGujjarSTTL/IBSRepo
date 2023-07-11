using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T58ClientContact
{
    public DateTime VisitDt { get; set; }

    public string? ClientOfficerName { get; set; }

    public string? Designation { get; set; }

    public string? ClientType { get; set; }

    public string Client { get; set; } = null!;

    public byte RitesOfficerCd { get; set; }

    public string? Highlights { get; set; }

    public string? OverallOutcome { get; set; }

    public string? RegionCd { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string TypeCb { get; set; } = null!;

    public decimal? OutAmt { get; set; }
}
