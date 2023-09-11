using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class Feedbacksuggestion
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Region { get; set; }

    public string? Name { get; set; }

    public string? Mobileno { get; set; }

    public string? Subject { get; set; }

    public string? Description { get; set; }
}
