﻿using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class WriteOffMaster
{
    public int Id { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }
}
