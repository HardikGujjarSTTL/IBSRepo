using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class TrainingCourseMaster
{
    public string? CourseId { get; set; }

    public string? TrainingType { get; set; }

    public string? TrainingField { get; set; }

    public string? CourseName { get; set; }

    public string? CourseInstitute { get; set; }

    public DateTime? CourseDurFr { get; set; }

    public DateTime? CourseDurTo { get; set; }

    public string? Certificate { get; set; }

    public decimal? Fees { get; set; }

    public string? Validity { get; set; }

    public string? Region { get; set; }

    public string? TrainingCategory { get; set; }
}
