using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class Menurolemapping
{
    public decimal Id { get; set; }

    public int? Roleid { get; set; }

    public int? Menuid { get; set; }

    public bool? Isactive { get; set; }
}
