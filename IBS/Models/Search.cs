namespace IBS.Models
{
    public class Search
    {
        public string CaseNo { get; set; }
        public string PONO { get; set; }
        public DateTime? PODT { get; set; }
        public string Consignee { get; set; }
        public int? BPO { get; set; }
        public DateTime? Calldt { get; set; }
        public int? CallSno { get; set; }
        public string BKNO { get; set; }
        public string SetNo { get; set; }
        public string BillNo { get; set; }
        public DateTime? BillDt { get; set; }
        public string IEName { get; set; }
        public decimal? BillAmount { get; set; }
        public decimal? InspFee { get; set; }

    }
}
