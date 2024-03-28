using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T117ManpowerDetail
{
    public int Id { get; set; }

    public string? Working { get; set; }

    public string? Staff { get; set; }

    public string? PlacePosting { get; set; }

    public int? ProjectName { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Manpowerid { get; set; }

    public string? Nameofcluster { get; set; }

    public DateTime? Fromdate { get; set; }

    public DateTime? Todate { get; set; }
}
