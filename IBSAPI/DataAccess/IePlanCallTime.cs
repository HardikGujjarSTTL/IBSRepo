﻿using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class IePlanCallTime
{
    public byte? IeCd { get; set; }

    public string? CaseNo { get; set; }

    public DateTime? CallRecvDt { get; set; }

    public int? CallSno { get; set; }

    public DateTime? CallStarttime { get; set; }

    public DateTime? CallEndtime { get; set; }

    public virtual T17CallRegister? Ca { get; set; }
}