using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T01Region
{
    public string RegionCode { get; set; } = null!;

    public string? Region { get; set; }

    public string? Address { get; set; }

    public string? GstinNo { get; set; }

    public string? RlyPartyCd { get; set; }

    public string? BankAccNo { get; set; }

    public string? IfscCode { get; set; }

    public string? BankName { get; set; }

    public string? PartyName { get; set; }

    public virtual ICollection<T08IeControllOfficer> T08IeControllOfficers { get; set; } = new List<T08IeControllOfficer>();

    public virtual ICollection<T09Ie> T09Ies { get; set; } = new List<T09Ie>();

    public virtual ICollection<T12BillPayingOfficer> T12BillPayingOfficers { get; set; } = new List<T12BillPayingOfficer>();

    public virtual ICollection<T13PoMaster> T13PoMasters { get; set; } = new List<T13PoMaster>();

    public virtual ICollection<T17CallRegister> T17CallRegisters { get; set; } = new List<T17CallRegister>();

    public virtual ICollection<T80PoMaster> T80PoMasters { get; set; } = new List<T80PoMaster>();
}
