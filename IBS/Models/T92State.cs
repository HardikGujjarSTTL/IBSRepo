using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T92State
{
    public byte StateCd { get; set; }

    public string? StateName { get; set; }

    public string? SapStateCd { get; set; }

    public virtual ICollection<T03City> T03Cities { get; set; } = new List<T03City>();
}
