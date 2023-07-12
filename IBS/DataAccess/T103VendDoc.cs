using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T103VendDoc
{
    public int VendCd { get; set; }

    public string DocType { get; set; } = null!;

    public DateTime? Datetime { get; set; }

    public virtual ICollection<T104VendEquipClbrCert> T104VendEquipClbrCerts { get; set; } = new List<T104VendEquipClbrCert>();
}
