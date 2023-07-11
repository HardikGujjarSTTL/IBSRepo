using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class OnlinePayment
{
    public string MerTxnRef { get; set; } = null!;

    public short? OrderInfo { get; set; }

    public string? TransactionNo { get; set; }

    public string? RrnNo { get; set; }

    public string? AuthCd { get; set; }

    public string? CaseNo { get; set; }

    public DateTime? CallRecvDt { get; set; }

    public short? CallSno { get; set; }

    public int? VendCd { get; set; }

    public decimal? Amount { get; set; }

    public string? ChargesType { get; set; }

    public string? Status { get; set; }

    public DateTime? Datetime { get; set; }
}
