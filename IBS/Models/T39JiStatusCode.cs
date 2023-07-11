using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T39JiStatusCode
{
    public byte JiStatusCd { get; set; }

    public string? JiStatusDesc { get; set; }

    public virtual ICollection<T40ConsigneeComplaint> T40ConsigneeComplaints { get; set; } = new List<T40ConsigneeComplaint>();
}
