using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class IbsUsersOtp
{
    public string? UserId { get; set; }

    public string? Mobile { get; set; }

    public string? Otp { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public byte? Status { get; set; }

    public int Id { get; set; }
}
