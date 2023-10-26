using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ReturnedBillsBpoChangeHistory
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

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
