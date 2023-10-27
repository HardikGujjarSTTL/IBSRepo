using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T35CentralItemDetail
{
    public int Id { get; set; }

    public int RailId { get; set; }

    public string? RailPricePerMt { get; set; }

    public string? PackingCharge { get; set; }

    public DateTime? PriceDateFr { get; set; }

    public DateTime? PriceDateTo { get; set; }

    public byte? Isactive { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public virtual T34CentralItemMaster Rail { get; set; } = null!;
}
