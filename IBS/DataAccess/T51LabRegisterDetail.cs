using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T51LabRegisterDetail
{
    public string? SampleRegNo { get; set; }

    public byte? Sno { get; set; }

    public string? ItemDesc { get; set; }

    public byte? Qty { get; set; }

    public string? TestCategoryCd { get; set; }

    public string? Test { get; set; }

    public int? LabId { get; set; }

    public decimal? TestingFee { get; set; }

    public decimal? ServiceTax { get; set; }

    public decimal? HandlingCharges { get; set; }

    public DateTime? TestReportReqDt { get; set; }

    public DateTime? TestReportRecDt { get; set; }

    public string? TestStatus { get; set; }

    public string? Remarks { get; set; }

    public decimal? AmountReceived { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public DateTime? SampleDispatchedToLabDt { get; set; }

    public string? PaymentId { get; set; }

    public virtual T65LaboratoryMaster? Lab { get; set; }

    public virtual T50LabRegister? SampleRegNoNavigation { get; set; }

    public virtual T64TestCategory? TestCategoryCdNavigation { get; set; }
}
