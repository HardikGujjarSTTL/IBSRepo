using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class UserMaster
{
    public long Id { get; set; }

    public string? UserId { get; set; }

    public string? Name { get; set; }

    public string? UserType { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public string? Createdby { get; set; }
}
