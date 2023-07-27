using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T65LaboratoryMasterBak
{
    public int? LabId { get; set; }

    public string? LabName { get; set; }

    public string? LabAddress { get; set; }

    public int? LabCity { get; set; }

    public string? LabContactPer { get; set; }

    public string? LabContactTel { get; set; }

    public string? LabEmail { get; set; }

    public string? LabApproval { get; set; }

    public DateTime? LabApprovalFr { get; set; }

    public DateTime? LabApprovalTo { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }
}
