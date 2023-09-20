using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class NonRlyClient
{
    public int Id { get; set; }

    public string? Clientname { get; set; }

    public string? Shortcode { get; set; }

    public string? Contactname { get; set; }

    public string? Contactdesignation { get; set; }

    public decimal? Mobileno { get; set; }

    public string? Emailid { get; set; }

    public DateTime? Updateddate { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public int? Createdby { get; set; }

    public int? Isdeleted { get; set; }
}
