using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T43CallReturn
{
    public string? ReturnNo { get; set; }

    public DateTime? ReturnDt { get; set; }

    public int? VendCd { get; set; }

    public string? VendEmail { get; set; }

    public string? VendContactNo { get; set; }

    public string? RlyNonrly { get; set; }

    public string? RlyCd { get; set; }

    public string? CallLetterNo { get; set; }

    public DateTime? CallLetterDt { get; set; }

    public DateTime? DtOfReciept { get; set; }

    public bool? ReturnReason1 { get; set; }

    public bool? ReturnReason2 { get; set; }

    public bool? ReturnReason3 { get; set; }

    public bool? ReturnReason4 { get; set; }

    public bool? ReturnReason5 { get; set; }

    public bool? ReturnReason6 { get; set; }

    public bool? ReturnReason7 { get; set; }

    public bool? ReturnReason8 { get; set; }

    public bool? ReturnReason9 { get; set; }

    public string? ReturnRemarks { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
