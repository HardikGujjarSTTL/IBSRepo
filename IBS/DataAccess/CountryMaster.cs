using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class CountryMaster
{
    public byte Id { get; set; }

    public string? CountryName { get; set; }

    public byte? CountryCode { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }
}
