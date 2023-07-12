using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T56LabPayment
{
    public string PaymentId { get; set; } = null!;

    public DateTime? PaymentDt { get; set; }

    public byte? BankCd { get; set; }

    public string? ChqNo { get; set; }

    public DateTime? ChqDt { get; set; }

    public decimal? Amount { get; set; }

    public byte? LabId { get; set; }

    public string? Remarks { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
