namespace IBS.Models
{
    public class MyData
    {
        public int resultFlag { get; set; }
        public string message { get; set; }
        public List<SAPInvoicesExportModel> data { get; set; }
    }

    public class SAPInvoicesExportModel
    {
        public string invoiceType01 { get; set; }

        public string customer01 { get; set; }

        public string documentDate { get; set; }

        public string reference01 { get; set; }

        public string postingDate { get; set; }

        public string documentType01 { get; set; }

        public string amount01 { get; set; }

        public string currency { get; set; }

        public string calculateTax01 { get; set; }

        public string dateOfCompletion01 { get; set; }

        public string invoiceType02 { get; set; }

        public string customer02 { get; set; }

        public string reference02 { get; set; }

        public string documentType02 { get; set; }

        public string amount02 { get; set; }

        public string calculateTax02 { get; set; }

        public string businessPlace01 { get; set; }

        public string sectionCode01 { get; set; }

        public string text01 { get; set; }

        public string dateOfCompletion02 { get; set; }

        public string gstPartner01 { get; set; }

        public string placeOfSupply01 { get; set; }

        public string invoiceType03 { get; set; }

        public string customer03 { get; set; }

        public string reference03 { get; set; }

        public string documenyType03 { get; set; }

        public string amount03 { get; set; }

        public string calculateTax03 { get; set; }

        public string businessPlace02 { get; set; }

        public string sectionCode02 { get; set; }

        public string text02 { get; set; }

        public string dateOfCompletion03 { get; set; }

        public string gstPartner02 { get; set; }

        public string placeOfSupply02 { get; set; }

        public string glAccount { get; set; }

        public string amount04 { get; set; }

        public string gstTaxCode { get; set; }

        public string text03 { get; set; }

        public string wbs { get; set; }

        public string hsnSacCode { get; set; }

    }
}
