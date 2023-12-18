using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T112HolidayDetail
{
    public int Id { get; set; }

    public string? HolidayId { get; set; }

    public DateTimeOffset? HolidayDt { get; set; }

    public string? HolidayDesc { get; set; }

    public string? UserId { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }
}
