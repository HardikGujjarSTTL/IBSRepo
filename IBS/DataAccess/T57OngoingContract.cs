using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T57OngoingContract
{
    public int ContractId { get; set; }

    public string? ClientName { get; set; }

    public string? ContractNo { get; set; }

    public DateTime? ContPerFrom { get; set; }

    public DateTime? ContPerTo { get; set; }

    public string? ContractFee { get; set; }

    public byte? ContractCm { get; set; }

    public string? ContractSpecialCondn { get; set; }

    public string? ContractPanalty { get; set; }

    public string? RegionCode { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? ContInspFee { get; set; }

    public string? ScopeOfWork { get; set; }

    public decimal? ContractFeeNum { get; set; }

    public DateTime? OfferDt { get; set; }

    public string? ExpOr { get; set; }

    public string? Status { get; set; }

    public DateTime? ContSignDt { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }
}
