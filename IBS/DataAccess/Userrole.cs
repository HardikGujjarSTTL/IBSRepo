using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class Userrole
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int UserId { get; set; }

    public string? Usertype { get; set; }
}
