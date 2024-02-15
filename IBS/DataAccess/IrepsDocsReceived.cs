using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class IrepsDocsReceived
{
    public int CallId { get; set; }

    public int IcNo { get; set; }

    public DateTime? RecvDt { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
