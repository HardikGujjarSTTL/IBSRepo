﻿using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T25RvDetailsHistory
{
    public string? VchrNo { get; set; }

    public int? Sno { get; set; }

    public int? BankCd { get; set; }

    public string? ChqNo { get; set; }

    public DateTime? ChqDt { get; set; }

    public decimal? Amount { get; set; }

    public int? AccCd { get; set; }

    public decimal? AmountAdjusted { get; set; }

    public decimal? SuspenseAmt { get; set; }

    public string? Narration { get; set; }

    public string? SampleNo { get; set; }

    public DateTime? PostDt { get; set; }

    public string? Status { get; set; }

    public string? BpoCd { get; set; }

    public string? BpoType { get; set; }

    public string? CaseNo { get; set; }

    public decimal? AmtTransferred { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public string? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
