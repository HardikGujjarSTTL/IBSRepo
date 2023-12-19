using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T112HolidayDetail
{
    public int Id { get; set; }

    public int? HolidayId { get; set; }

    public DateTime? HolidayDt { get; set; }

    public string? HolidayDesc { get; set; }

    public string? UserId { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }
}
