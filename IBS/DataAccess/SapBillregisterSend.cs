using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class SapBillregisterSend
{
    public decimal Id { get; set; }

    public string Batchid { get; set; } = null!;

    public DateTime? BatchDt { get; set; }

    public string? SapAcknowledgementId { get; set; }

    public DateTime? SapAcknowledgementDt { get; set; }
}
