using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

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

    public string? CustEmail { get; set; }

    public string? CustMobile { get; set; }

    public string? MerId { get; set; }

    public DateTimeOffset? MerTxnDate { get; set; }

    public string? MerTxnId { get; set; }

    public string? AtomTxnId { get; set; }

    public string? CustAccNo { get; set; }

    public DateTimeOffset? TxnCompleteDate { get; set; }

    public string? BankTxnId { get; set; }

    public string? BankName { get; set; }

    public string? SubChannel { get; set; }

    public string? Description { get; set; }

    public string? StatusCd { get; set; }

    public string? TokId { get; set; }
}
