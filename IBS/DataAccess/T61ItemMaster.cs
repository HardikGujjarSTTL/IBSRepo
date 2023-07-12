using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T61ItemMaster
{
    public string ItemCd { get; set; } = null!;

    public string? ItemDesc { get; set; }

    public string? Department { get; set; }

    public byte? TimeForInsp { get; set; }

    public string? Checksheet { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public byte? Ie { get; set; }

    public byte? Cm { get; set; }

    public DateTime? CreationRevDt { get; set; }
}
