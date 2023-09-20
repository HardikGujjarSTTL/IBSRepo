namespace IBS.Models
{
    public class JIRequiredList
    {
        public string IE { get; set; }
        public string Region { get; set; }
        public string DEFECT_DESC { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal NO_OF_INSPECTION { get; set; }
        public decimal MATERIAL_VALUE { get; set; }
        public int RECD { get; set; }
        public int FINALISED { get; set; }
        public int PENDING { get; set; }
        public int ACCEPTED { get; set; }
        public int UPHELD { get; set; }
        public int SORTING { get; set; }
        public int RECTIFICATION { get; set; }
        public int PRICE_REDUCTION { get; set; }
        public int LIFTED_BEFORE_JI { get; set; }
        public int TRANSIT_DEMAGE { get; set; }
        public int UNSTAMPED { get; set; }
        public int NOT_ON_RITES_AC { get; set; }
        public int Total { get; set; }
    }
}
