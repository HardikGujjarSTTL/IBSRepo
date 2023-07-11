using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class RitesBillDtl
{
    public string BillNo { get; set; } = null!;

    public DateTime Invoicedate { get; set; }

    public string? BpoCd { get; set; }

    public string? BpoType { get; set; }

    public string? BpoRly { get; set; }

    public string? BpoName { get; set; }

    public string? BpoOrgn { get; set; }

    public string? BpoAdd { get; set; }

    public string? BpoCity { get; set; }

    public string CaseNo { get; set; } = null!;

    public string? RegionCode { get; set; }

    public string? PoNo { get; set; }

    public DateTime? PoDt { get; set; }

    public int? VendCd { get; set; }

    public string? VendName { get; set; }

    public string? VendAdd1 { get; set; }

    public string? VendAdd2 { get; set; }

    public string? VendorCity { get; set; }

    public int? ConsigneeCd { get; set; }

    public string? Consignee { get; set; }

    public string? ConsigneeAdd1 { get; set; }

    public string? ConsigneeAdd2 { get; set; }

    public string? ConsigneeCity { get; set; }

    public byte? IeCd { get; set; }

    public byte? IeCoCd { get; set; }

    public string? IcNo { get; set; }

    public DateTime? IcDt { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public string? CallInstalmentNo { get; set; }

    public decimal? MaterialValue { get; set; }

    public int? Visits { get; set; }

    public decimal? Gsttaxableamt { get; set; }

    public decimal? Cgstamt { get; set; }

    public decimal? Sgstamt { get; set; }

    public decimal? Igstamt { get; set; }

    public decimal? Amount { get; set; }

    public string Invoiceno { get; set; } = null!;

    public string? Rlygstin { get; set; }

    public string? Partygstin { get; set; }

    public string? Partycode { get; set; }

    public string? Partyname { get; set; }

    public byte? ItemSrno { get; set; }

    public string? Itemdesc { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Rate { get; set; }

    public string? Unitcode { get; set; }

    public string? UomFactor { get; set; }

    public decimal? BasicValue { get; set; }

    public decimal? Value { get; set; }

    public string? Pdffile { get; set; }

    public string? Billdesc { get; set; }

    public string? Partystate { get; set; }

    public string? Reversecharge { get; set; }

    public string? Isgstregistered { get; set; }

    public string? Gsttdsdeduction { get; set; }

    public string? Compositetaxable { get; set; }

    public string? Hsnsac { get; set; }

    public string? Hsnsaccode { get; set; }

    public string? Itceligible { get; set; }

    public decimal? Sgstrate { get; set; }

    public decimal? Cgstrate { get; set; }

    public decimal? Ugstrate { get; set; }

    public decimal? Ugstamt { get; set; }

    public decimal? Igstrate { get; set; }

    public string? Statesupply { get; set; }

    public DateTime? RecvDate { get; set; }

    public DateTime? UpdDate { get; set; }

    public string? Status { get; set; }

    public string? Co6No { get; set; }

    public DateTime? Co6Date { get; set; }

    public string? Co6Status { get; set; }

    public DateTime? Co6StatusDate { get; set; }

    public decimal? PassedAmt { get; set; }

    public decimal? DeductedAmt { get; set; }

    public decimal? NetAmt { get; set; }

    public DateTime? Bookdate { get; set; }

    public string? ReturnReason { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string? PdfLink { get; set; }

    public DateTime? PaymentDt { get; set; }

    public string? Co7No { get; set; }

    public DateTime? Co7Date { get; set; }

    public string? BankAcctNo { get; set; }

    public string? Ifsccode { get; set; }

    public string? BankName { get; set; }

    public string? Bankaddress { get; set; }

    public string? IcPdf { get; set; }

    public string? InvoicePdf { get; set; }

    public string Au { get; set; } = null!;

    public string? PoPdf { get; set; }

    public bool BillResentCount { get; set; }

    public string? IrfcFunded { get; set; }

    public string? InvoiceSuppDocs { get; set; }
}
