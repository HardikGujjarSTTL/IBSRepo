using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class LabCalTranHeader
{
    public int CalTranHeaderId { get; set; }

    public string? BarcodeId { get; set; }

    public string? TypeofGst { get; set; }

    public int? Sgst { get; set; }

    public int? Cgst { get; set; }

    public int? Igst { get; set; }

    public int? Total { get; set; }

    public int? Tax { get; set; }

    public int? Grandtotal { get; set; }

    public int? HandlingCharge { get; set; }

    public int? ExtraCharge { get; set; }

    public int? Rtotal { get; set; }
}
