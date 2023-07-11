using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class ReturnedBillsBpoChange
{
    public string? BillNo { get; set; }

    public string? OldBpoCd { get; set; }

    public string? NewBpoCd { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? OldAccGroup { get; set; }

    public string? NewAccGroup { get; set; }

    public string? OldIrfcBpoCd { get; set; }

    public string? NewIrfcBpoCd { get; set; }

    public string? OldRecipientGstinNo { get; set; }

    public string? NewRecipientGstinNo { get; set; }
}
