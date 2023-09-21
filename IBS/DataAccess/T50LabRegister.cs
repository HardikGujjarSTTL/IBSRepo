using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T50LabRegister
{
    public string SampleRegNo { get; set; } = null!;

    public DateTime? SampleRegDt { get; set; }

    public DateTime? SampleDrawlDt { get; set; }

    public DateTime? SampleRecieptDt { get; set; }

    public DateTime? SampleDispatchDt { get; set; }

    public int? IeCd { get; set; }

    public string? CaseNo { get; set; }

    public DateTime? CallRecvDt { get; set; }

    public int? CallSno { get; set; }

    public int? VendCd { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? TestingType { get; set; }

    public long? TotalTestingFee { get; set; }

    public int? TotalHandlingCharges { get; set; }

    public int? TotalServiceTax { get; set; }

    public long? TotalLabCharges { get; set; }

    public long? AmountRecieved { get; set; }

    public decimal? Tds { get; set; }

    public DateTime? TdsDt { get; set; }

    public string? CodeNo { get; set; }

    public DateTime? CodeDt { get; set; }

    public decimal Id { get; set; }

    public virtual T17CallRegister? Ca { get; set; }

    public virtual ICollection<T55LabInvoice> T55LabInvoices { get; set; } = new List<T55LabInvoice>();
}
