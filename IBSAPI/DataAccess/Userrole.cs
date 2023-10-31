using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class Userrole
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string? Usertype { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public string? UserId { get; set; }
}
