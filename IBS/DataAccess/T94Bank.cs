﻿using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T94Bank
{
    public int BankCd { get; set; }

    public string? BankName { get; set; }

    public byte? FmisBankCd { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }
}
