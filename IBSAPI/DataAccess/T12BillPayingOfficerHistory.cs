﻿using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T12BillPayingOfficerHistory
{
    public string? BpoCd { get; set; }

    public string? BpoRegion { get; set; }

    public string? BpoType { get; set; }

    public string? BpoName { get; set; }

    public string? BpoRly { get; set; }

    public string? BpoAdd { get; set; }

    public int? BpoCityCd { get; set; }

    public string? BillPassOfficer { get; set; }

    public string? BpoFeeType { get; set; }

    public decimal? BpoFee { get; set; }

    public string? BpoTaxType { get; set; }

    public string? BpoFlg { get; set; }

    public string? BpoAdvFlg { get; set; }

    public string? BpoLocCd { get; set; }

    public string? BpoOrgn { get; set; }

    public string? BpoAdd1 { get; set; }

    public string? BpoAdd2 { get; set; }

    public string? BpoState { get; set; }

    public string? BpoPin { get; set; }

    public string? BpoPhone { get; set; }

    public string? BpoFax { get; set; }

    public string? BpoEmail { get; set; }

    public string? PayWindowId { get; set; }

    public string? BpoCdOld { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? GstinNo { get; set; }

    public string? Au { get; set; }

    public string? SapCustCdBpo { get; set; }

    public string? LegalName { get; set; }

    public string? PinCode { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }

    public string? Actiontype { get; set; }

    public DateTimeOffset? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
