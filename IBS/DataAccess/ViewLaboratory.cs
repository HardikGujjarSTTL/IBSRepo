using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class ViewLaboratory
{
    public byte LabId { get; set; }

    public string? LabName { get; set; }

    public string? LabAddress { get; set; }

    public string? City { get; set; }
}
