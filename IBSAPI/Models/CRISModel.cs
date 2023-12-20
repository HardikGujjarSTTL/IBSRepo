namespace IBSAPI.Models
{
    public class CRISModel
    {
        public string BillNo { get; set; } = null!;

        public DateTime? BillDt { get; set; }

        public string? BpoCd { get; set; }

        public string? BpoType { get; set; }

        public string? BpoRly { get; set; }

        public string? BpoName { get; set; }

        public string? BpoOrgn { get; set; }

        public string? BpoAdd { get; set; }

        public string? BpoCity { get; set; }

        public string? CaseNo { get; set; }

        public string? RegionCode { get; set; }

        public string? PoNo { get; set; }

        public DateTime? PoDt { get; set; }

        public int? VendCd { get; set; }

        public string? VendName { get; set; }

        public string? VendAdd1 { get; set; }

        public string? VendAdd2 { get; set; }

        public string? VendorCity { get; set; }

        public int ConsigneeCd { get; set; }

        public string? Consignee { get; set; }

        public string? ConsigneeAdd1 { get; set; }

        public string? ConsigneeAdd2 { get; set; }

        public string? ConsigneeCity { get; set; }

        public int? IeCd { get; set; }

        public int? IeCoCd { get; set; }

        public string? IcNo { get; set; }

        public DateTime? IcDt { get; set; }

        public string? BkNo { get; set; }

        public string? SetNo { get; set; }

        public string? CallInstalmentNo { get; set; }

        public decimal? MaterialValue { get; set; }

        public int? Visits { get; set; }

        public decimal? InspFee { get; set; }

        public decimal? Cgst { get; set; }

        public decimal? Sgst { get; set; }

        public decimal? Igst { get; set; }

        public decimal? BillAmount { get; set; }

        public string? InvoiceNo { get; set; }

        public string? RecipientGstinNo { get; set; }

        public string? Au { get; set; }

        public string? GstinNo { get; set; }

        public string? RlyPartyCd { get; set; }

        public string? PartyName { get; set; }

        public string? BankAccNo { get; set; }

        public string? IfscCode { get; set; }

        public string? BankName { get; set; }

        public bool? BillResentCount { get; set; }

        public string? IrfcFunded { get; set; }

    }
}
