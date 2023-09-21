using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T100Contract
{
    public int Id { get; set; }

    public string? LetterNo { get; set; }

    public DateTime? LetterDate { get; set; }

    public DateTime? Tpfrom { get; set; }

    public DateTime? Tpto { get; set; }

    public string? Clienttype { get; set; }

    public string? Clientname { get; set; }

    public decimal? Inspfee { get; set; }

    public decimal? Mandaybasis { get; set; }

    public decimal? Lotofinsp { get; set; }

    public decimal? Materialvalue { get; set; }

    public decimal? Minpoval { get; set; }

    public decimal? Maxpoval { get; set; }

    public decimal? Callcancelation { get; set; }

    public string? Materialdescription { get; set; }

    public DateTime? Updatedate { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Createdby { get; set; }

    public int? Updatedby { get; set; }

    public int? Isdeleted { get; set; }
}
