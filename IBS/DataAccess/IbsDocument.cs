using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class IbsDocument
{
    public int Id { get; set; }

    public string? Documentname { get; set; }

    public int? Documentcategory { get; set; }

    public string Allowedfileextensions { get; set; } = null!;

    public int? Maxcontentlengthinkb { get; set; }

    public byte Ismandatory { get; set; }

    public byte Isvisible { get; set; }

    public byte Verifydsc { get; set; }

    public string? Staticlink { get; set; }

    public int? Workflowstatusid { get; set; }

    public byte? Isdownloadtemplate { get; set; }

    public string? Folderpath { get; set; }

    public byte Isvideo { get; set; }

    public virtual ICollection<IbsAppdocument> IbsAppdocuments { get; set; } = new List<IbsAppdocument>();
}
