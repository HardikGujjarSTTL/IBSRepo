using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class ViewLaboratory
{
    public int LabId { get; set; }

    public string? LabName { get; set; }

    public string? LabAddress { get; set; }

    public string? City { get; set; }
}
