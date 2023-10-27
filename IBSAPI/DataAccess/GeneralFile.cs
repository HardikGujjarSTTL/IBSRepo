using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class GeneralFile
{
    public int? UnitCode { get; set; }

    public byte? CurryCode { get; set; }

    public int? VchrNumb { get; set; }

    public DateTime? VchrDate { get; set; }

    public int? Tc { get; set; }

    public int? AccCode { get; set; }

    public string? SubCode { get; set; }

    public byte? RefNo { get; set; }

    public int? ProjectCode { get; set; }

    public int? SbuCode { get; set; }

    public string? Narration { get; set; }

    public decimal? Amount { get; set; }

    public string? ChequeNo { get; set; }

    public int? BankCode { get; set; }

    public decimal? BankOb { get; set; }

    public decimal? BankCb { get; set; }

    public decimal? AccOb { get; set; }

    public decimal? AccCb { get; set; }

    public byte? SupplNo { get; set; }

    public string? UserCode { get; set; }

    public DateTime? FcurTcDate { get; set; }

    public byte? Module { get; set; }

    public string? PartyName { get; set; }

    public byte? PrepBy { get; set; }

    public byte? CheckBy { get; set; }

    public string? Region { get; set; }

    public string? VchrNoT25 { get; set; }

    public int? SnoT25 { get; set; }

    public string? PostingStatus { get; set; }

    public DateTime? PostingDt { get; set; }

    public int Id { get; set; }
}
