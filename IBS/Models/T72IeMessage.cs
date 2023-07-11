using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T72IeMessage
{
    public byte MessageId { get; set; }

    public string? LetterNo { get; set; }

    public DateTime? LetterDt { get; set; }

    public string? Message { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string RegionCode { get; set; } = null!;

    public DateTime? MessageDt { get; set; }
}
