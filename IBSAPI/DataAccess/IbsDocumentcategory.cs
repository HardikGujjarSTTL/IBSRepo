using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class IbsDocumentcategory
{
    public int Id { get; set; }

    public string? Categoryname { get; set; }

    public string? Groupname { get; set; }

    public string? Grouplabel { get; set; }

    public string? Categorylabel { get; set; }

    public byte Showlist { get; set; }

    public int? Groupid { get; set; }
}
