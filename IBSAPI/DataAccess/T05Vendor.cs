using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T05Vendor
{
    public int VendCd { get; set; }

    public string? VendName { get; set; }

    public string? VendAdd1 { get; set; }

    public string? VendAdd2 { get; set; }

    public int? VendCityCd { get; set; }

    public string? VendContactPer1 { get; set; }

    public string? VendContactTel1 { get; set; }

    public string? VendContactPer2 { get; set; }

    public string? VendContactTel2 { get; set; }

    public string? VendApproval { get; set; }

    public DateTime? VendApprovalFr { get; set; }

    public DateTime? VendApprovalTo { get; set; }

    public string? VendStatus { get; set; }

    public DateTime? VendStatusDtFr { get; set; }

    public DateTime? VendStatusDtTo { get; set; }

    public string? VendRemarks { get; set; }

    public string? VendCdAlpha { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? VendEmail { get; set; }

    public string? VendInspStopped { get; set; }

    public string? VendPwd { get; set; }

    public string? OnlineCallStatus { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public string? OfflineCallStatus { get; set; }

    public string? VendGstno { get; set; }

    public string? VendTanno { get; set; }

    public string? VendPanno { get; set; }

    public virtual ICollection<T13PoMaster> T13PoMasters { get; set; } = new List<T13PoMaster>();

    public virtual ICollection<T17CallRegister> T17CallRegisters { get; set; } = new List<T17CallRegister>();

    public virtual ICollection<T40ConsigneeComplaint> T40ConsigneeComplaints { get; set; } = new List<T40ConsigneeComplaint>();

    public virtual ICollection<T41NcMaster> T41NcMasters { get; set; } = new List<T41NcMaster>();

    public virtual ICollection<T47IeWorkPlan> T47IeWorkPlans { get; set; } = new List<T47IeWorkPlan>();

    public virtual ICollection<T60IePoiMapping> T60IePoiMappings { get; set; } = new List<T60IePoiMapping>();

    public virtual ICollection<T80PoMaster> T80PoMasters { get; set; } = new List<T80PoMaster>();

    public virtual T03City? VendCityCdNavigation { get; set; }
}
