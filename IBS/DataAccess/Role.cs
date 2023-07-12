using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class Role
{
    public decimal RoleId { get; set; }

    public string? Rolename { get; set; }

    public string? Roledescription { get; set; }

    public byte? Issysadmin { get; set; }

    public byte? Isactive { get; set; }

    public byte? Isdeleted { get; set; }

    public DateTime? Createddate { get; set; }

    public decimal? Createdby { get; set; }

    public DateTime? Updateddate { get; set; }

    public decimal? Updatedby { get; set; }
}
