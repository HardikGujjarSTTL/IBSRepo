using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class OauthAuthCode
{
    public string Id { get; set; } = null!;

    public long UserId { get; set; }

    public int ClientId { get; set; }

    public string? Scopes { get; set; }

    public bool Revoked { get; set; }

    public DateTime? ExpiresAt { get; set; }
}
