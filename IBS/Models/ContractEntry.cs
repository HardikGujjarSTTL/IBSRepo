namespace IBS.Models
{
    public class ContractEntry
    {
        public int ID { get; set; }
        public int? LETTER_NO { get; set; }
        public DateTime? LETTER_DATE { get; set; }
        public DateTime? TPFROM { get; set; }
        public DateTime? TPTO { get; set; }
        public string? CLIENTTYPE { get; set; }
        public string? CLIENTNAME { get; set; }
        public int? INSPFEE { get; set; }
        public int? MANDAYBASIS { get; set; }
        public int? LOTOFINSP { get; set; }
        public int? MATERIALVALUE { get; set; }
        public int? MINPOVAL { get; set; }
        public int? MAXPOVAL { get; set; }
        public string? Materialdescription { get; set; }
        public int? CALLCANCELATION { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? updatedDate { get; set; }
    }
}
