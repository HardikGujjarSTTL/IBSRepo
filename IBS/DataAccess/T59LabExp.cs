﻿using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T59LabExp
{
    public string RegionCode { get; set; } = null!;

    public string LabBillPer { get; set; } = null!;

    public decimal? LabExp { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }
}
