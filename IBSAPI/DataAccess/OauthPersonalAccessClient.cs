using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class OauthPersonalAccessClient
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
