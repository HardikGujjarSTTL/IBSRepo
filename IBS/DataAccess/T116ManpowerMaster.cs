using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T116ManpowerMaster
{
    public int Id { get; set; }

    public string? Region { get; set; }

    public string? EmpName { get; set; }

    public string? EmpNo { get; set; }

    public string? Desig { get; set; }

    public string? Cadre { get; set; }

    public string? Discp { get; set; }

    public string? Status { get; set; }

    public DateTime? Dob { get; set; }

    public DateTime? RitesDt { get; set; }

    public DateTime? RioDt { get; set; }

    public DateTime? DrrtDt { get; set; }

    public string? UserId { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }
}
