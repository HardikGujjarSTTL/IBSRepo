using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T104VendEquipClbrCert
{
    public int VendCd { get; set; }

    public string DocType { get; set; } = null!;

    public byte? EquipClbrCertSno { get; set; }

    public string? EquipName { get; set; }

    public string EquipMkSl { get; set; } = null!;

    public string? EquipRange { get; set; }

    public string? CalibratedBy { get; set; }

    public string CalibCertNo { get; set; } = null!;

    public DateTime? DtOfCalib { get; set; }

    public DateTime? NextDtCalib { get; set; }

    public string? EquipDesc { get; set; }

    public string? NablAccDet { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual T103VendDoc T103VendDoc { get; set; } = null!;
}
