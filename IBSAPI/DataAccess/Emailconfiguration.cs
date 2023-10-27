using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class Emailconfiguration
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Displayname { get; set; }

    public string? Host { get; set; }

    public int? Port { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public bool? Enablessl { get; set; }

    public bool? Usedefaultcredentials { get; set; }
}
