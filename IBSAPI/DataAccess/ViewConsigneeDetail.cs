﻿using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewConsigneeDetail
{
    public int ConsigneeCd { get; set; }

    public string? ConsigneeName { get; set; }

    public string CaseNo { get; set; } = null!;
}
