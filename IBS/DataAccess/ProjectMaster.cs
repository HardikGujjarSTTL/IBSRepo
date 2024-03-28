using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class ProjectMaster
{
    public int Id { get; set; }

    public string? Projectname { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Completiondate { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public string? SanctionedFile { get; set; }
}
