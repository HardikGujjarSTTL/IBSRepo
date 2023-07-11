using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class TraineeEmployeeMaster
{
    public byte? IeCd { get; set; }

    public string? Name { get; set; }

    public DateTime? Dob { get; set; }

    public DateTime? Doj { get; set; }

    public string? EmpNo { get; set; }

    public string? Descipline { get; set; }

    public string? Category { get; set; }

    public string? CategoryOther { get; set; }

    public string? Qualification { get; set; }

    public string? QualOther { get; set; }

    public string? QualInstitute { get; set; }

    public string? Region { get; set; }
}
