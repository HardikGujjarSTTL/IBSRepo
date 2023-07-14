using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T96Message
{
    public decimal MessageId { get; set; }

    public string? Message { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Createdby { get; set; }

    public string? Updatedby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public decimal? Isdeleted { get; set; }
}
