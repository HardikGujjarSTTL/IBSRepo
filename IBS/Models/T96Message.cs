using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T96Message
{
    public byte MessageId { get; set; }

    public string? Message { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
