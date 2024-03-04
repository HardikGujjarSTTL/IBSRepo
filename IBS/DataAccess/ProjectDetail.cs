using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class ProjectDetail
{
    public int Id { get; set; }

    public int ProjId { get; set; }

    public string? Sanctionedstrength { get; set; }

    public string? Department { get; set; }

    public decimal? Nos { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }
}
