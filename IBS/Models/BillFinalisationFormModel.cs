namespace IBS.Models
{
    public class BillFinalisationFormModel
    {
        public string BillNo { get; set; } = null!;

        public DateTime? BillDt { get; set; }

        public decimal? InspFee { get; set; }

        public decimal? Cgst { get; set; }

        public decimal? Sgst { get; set; }

        public decimal? Igst { get; set; }

        public decimal? BillAmount { get; set; }

        public string? InvoiceNo { get; set; }

        public string? BPO { get; set; }

        public string? Consignee { get; set; }

        public string? RecipientGstinNo { get; set; }
    }
}
