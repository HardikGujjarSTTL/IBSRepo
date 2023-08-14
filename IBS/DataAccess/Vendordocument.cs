using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class Vendordocument
{
    public int Id { get; set; }

    public int? Vendcd { get; set; }

    public string? Documenttype { get; set; }
}
