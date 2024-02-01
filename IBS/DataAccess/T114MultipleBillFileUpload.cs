using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T114MultipleBillFileUpload
{
    public int Id { get; set; }

    public string? BillNo { get; set; }

    public string? FileName { get; set; }

    public DateTime? Createddate { get; set; }

    public string? Createdby { get; set; }

    public DateTime? Updateddate { get; set; }

    public string? Updatedby { get; set; }
}
