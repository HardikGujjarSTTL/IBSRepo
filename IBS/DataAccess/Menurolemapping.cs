using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class Menurolemapping
{
    public int Id { get; set; }

    public int? Roleid { get; set; }

    public int? Menuid { get; set; }

    public bool? Isactive { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public bool? Isadd { get; set; }

    public bool? Isedit { get; set; }

    public bool? Pisdelete { get; set; }

    public bool? Isview { get; set; }
}
