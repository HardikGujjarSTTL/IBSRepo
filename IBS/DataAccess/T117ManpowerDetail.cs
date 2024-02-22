using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T117ManpowerDetail
{
    public int Id { get; set; }

    public string? Working { get; set; }

    public string? Staff { get; set; }

    public string? PlacePosting { get; set; }

    public string? ProjectName { get; set; }

    public string? UserId { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }
}
