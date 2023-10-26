using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class Token
{
    public int Tokenid { get; set; }

    public string? UserId { get; set; }

    public string? Authtoken { get; set; }

    public DateTimeOffset? Issueon { get; set; }

    public DateTimeOffset? Expireson { get; set; }
}
