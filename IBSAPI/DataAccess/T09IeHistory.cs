using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T09IeHistory
{
    public int IeCd { get; set; }

    public string? IeName { get; set; }

    public string? IeSname { get; set; }

    public string? IeEmpNo { get; set; }

    public int? IeDesig { get; set; }

    public int? IeSealNo { get; set; }

    public string? IeDepartment { get; set; }

    public int? IeCityCd { get; set; }

    public string? IePhoneNo { get; set; }

    public int? IeCoCd { get; set; }

    public DateTime? IeJoinDt { get; set; }

    public string? IeStatus { get; set; }

    public DateTime? IeStatusDt { get; set; }

    public string? IeType { get; set; }

    public string? IeRegion { get; set; }

    public string? IePwd { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? IeEmail { get; set; }

    public DateTime? IeDob { get; set; }

    public int? AltIe { get; set; }

    public string? IeCallMarking { get; set; }

    public int? AltIeTwo { get; set; }

    public int? AltIeThree { get; set; }

    public DateTime? CallMarkingStoppingDt { get; set; }

    public DateTime? DscExpiryDt { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }

    public DateTime? CallMarkingStartDt { get; set; }

    public DateTime? InspectionStartDt { get; set; }

    public DateTime? RepatriationDt { get; set; }

    public string? IeJobType { get; set; }

    public string? Actiontype { get; set; }

    public DateTime? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
