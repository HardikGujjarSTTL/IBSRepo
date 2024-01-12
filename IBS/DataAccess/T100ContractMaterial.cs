using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T100ContractMaterial
{
    public decimal Id { get; set; }

    public int? ContractId { get; set; }

    public int? PerBasis { get; set; }

    public int? Manday { get; set; }

    public int? Lumpsum { get; set; }

    public int? Fromrs { get; set; }

    public int? Tors { get; set; }

    public string? Createdby { get; set; }

    public string? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updatedate { get; set; }

    public int? Isdeleted { get; set; }

    public string? Userid { get; set; }
}
