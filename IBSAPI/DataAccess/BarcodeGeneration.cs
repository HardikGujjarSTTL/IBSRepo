using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class BarcodeGeneration
{
    public int Id { get; set; }

    public string Barcode { get; set; } = null!;

    public string? CaseNo { get; set; }

    public DateTime? CallRecvDt { get; set; }

    public int? CallSno { get; set; }

    public string? ItemSrnoPo { get; set; }

    public string? VendCd { get; set; }

    public string? CustomerName { get; set; }

    public string? SealingType { get; set; }

    public string? CustomerGstn { get; set; }

    public string? Description { get; set; }

    public DateTime? TargetedDate { get; set; }

    public DateTime? CurrentDate { get; set; }

    public string? InspectorCustomer { get; set; }

    public string? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public string? Userid { get; set; }

    public string? Ipaddress { get; set; }

    public string? Rate { get; set; }

    public string? Rtax { get; set; }
}
