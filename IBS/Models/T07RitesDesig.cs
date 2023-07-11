using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T07RitesDesig
{
    public byte RDesigCd { get; set; }

    public string? RDesignation { get; set; }

    public virtual ICollection<T08IeControllOfficer> T08IeControllOfficers { get; set; } = new List<T08IeControllOfficer>();

    public virtual ICollection<T09Ie> T09Ies { get; set; } = new List<T09Ie>();
}
