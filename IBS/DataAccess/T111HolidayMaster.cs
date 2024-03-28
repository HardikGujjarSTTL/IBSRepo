using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T111HolidayMaster
{
    public int Id { get; set; }

    public string? FinancialYear { get; set; }

    public DateTime? FyFromDt { get; set; }

    public DateTime? FyToDt { get; set; }

    public string? UserId { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public string? Region { get; set; }
}
