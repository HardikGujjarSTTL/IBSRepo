﻿using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class Tblexception
{
    public decimal Id { get; set; }

    public string? Controllername { get; set; }

    public string? Actionname { get; set; }

    public string? Exceptionmessage { get; set; }

    public string? Exception { get; set; }

    public DateTime? Createddate { get; set; }

    public decimal? Createdby { get; set; }

    public string? Createip { get; set; }
}
