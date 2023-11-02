using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class Regionalhrdataofie
{
    public int Id { get; set; }

    public int IeCd { get; set; }

    public string? Disclipline { get; set; }

    public DateTime? Joiningdate { get; set; }

    public DateTime? Postingdate { get; set; }

    public DateTime? Retirementdate { get; set; }

    public DateTime? Transferdate { get; set; }

    public DateTime? Deputationfromdate { get; set; }

    public DateTime? Deputationtodate { get; set; }

    public DateTime? Repetriationdate { get; set; }

    public DateTime? Ietenurefromdate { get; set; }

    public DateTime? Ietenuretodate { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public virtual T09Ie IeCdNavigation { get; set; } = null!;
}
