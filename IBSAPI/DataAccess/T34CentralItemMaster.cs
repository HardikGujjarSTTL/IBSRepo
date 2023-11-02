using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T34CentralItemMaster
{
    public int Id { get; set; }

    public string? RailCd { get; set; }

    public string? RailDesc { get; set; }

    public string? RailLengthMeter { get; set; }

    public string? UserId { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public virtual ICollection<T35CentralItemDetail> T35CentralItemDetails { get; set; } = new List<T35CentralItemDetail>();
}
