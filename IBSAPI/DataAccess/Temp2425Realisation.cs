using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class Temp2425Realisation
{
    public string? Region { get; set; }

    public string? BpoCd { get; set; }

    public string? BpoRly { get; set; }

    public string? BpoType { get; set; }

    public string? BpoOrgn { get; set; }

    public DateTime? RealisationDt { get; set; }

    public byte? BankCd { get; set; }

    public string? ChqNo { get; set; }

    public DateTime? ChqDt { get; set; }

    public decimal? InspectionFee { get; set; }

    public decimal? LabFee { get; set; }

    public decimal? AdvanceReceipt { get; set; }

    public decimal? AdvanceAdjusted { get; set; }

    public decimal? ReceiptFrWr { get; set; }

    public decimal? ReceiptFrNr { get; set; }

    public decimal? ReceiptFrEr { get; set; }

    public decimal? ReceiptFrSr { get; set; }

    public decimal? ReceiptFrCr { get; set; }

    public decimal? MiscReceipt { get; set; }

    public decimal? TransferToWr { get; set; }

    public decimal? TransferToNr { get; set; }

    public decimal? TransferToEr { get; set; }

    public decimal? TransferToSr { get; set; }

    public decimal? TransferToCr { get; set; }

    public decimal? TransferOldSystem { get; set; }

    public decimal? MiscTransfer { get; set; }

    public decimal? NetRealisation { get; set; }

    public decimal? AmountAdjusted { get; set; }

    public decimal? AmtTransferred { get; set; }

    public decimal? Suspense { get; set; }
}
