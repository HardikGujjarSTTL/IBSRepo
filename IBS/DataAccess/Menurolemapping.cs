using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class Menurolemapping
{
    public decimal Id { get; set; }

    public decimal? Roleid { get; set; }

    public decimal? Menuid { get; set; }

    public bool? Isactive { get; set; }
}
