﻿using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class BpoCheck
{
    public string CaseNo { get; set; } = null!;

    public string? BpoCd { get; set; }

    public string? VendName { get; set; }
}
