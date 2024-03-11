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

    public string? Clientcode { get; set; }

    public string? InspectionfeeType { get; set; }

    public int? PerBasisFlatfee { get; set; }

    public int? MandayFlatfee { get; set; }

    public int? LumpsumFlatfee { get; set; }

    public int? PerBasisCancellation { get; set; }

    public int? MandayCancellation { get; set; }

    public int? LumpsumCancellation { get; set; }

    public int? PerBasisRejection { get; set; }

    public int? MandayRejection { get; set; }

    public int? LumpsumRejection { get; set; }
}
