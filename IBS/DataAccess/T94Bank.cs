using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T94Bank
{
    public byte BankCd { get; set; }

    public string? BankName { get; set; }

    public byte? FmisBankCd { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
