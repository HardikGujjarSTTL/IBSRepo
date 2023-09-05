using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class MenuMaster
{
    public int Id { get; set; }

    public int? Parentid { get; set; }

    public string? Controllername { get; set; }

    public string? Actionname { get; set; }

    public string? Title { get; set; }

    public string? Menudescription { get; set; }

    public int? RoleId { get; set; }

    public string? Iconcssclass { get; set; }

    public bool? Isactive { get; set; }

    public int? Sortorder { get; set; }

    public string? Iconpath { get; set; }

    public bool? Isdisplay { get; set; }

    public virtual Role? Role { get; set; }
}
