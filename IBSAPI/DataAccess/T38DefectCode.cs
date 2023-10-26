using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T38DefectCode
{
    public string DefectCd { get; set; } = null!;

    public string? DefectDesc { get; set; }

    public virtual ICollection<T40ConsigneeComplaint> T40ConsigneeComplaints { get; set; } = new List<T40ConsigneeComplaint>();
}
