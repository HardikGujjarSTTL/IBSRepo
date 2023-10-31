namespace IBSAPI.Models
{
    public class TodayInspectionModel
    {
        public string Case_No { get; set; }
        public DateTime Date { get; set; }
        public int Call_Sno { get; set; }
        public string Name { get; set; }
        public string Vend_Name { get; set; }
        public decimal? Qty { get; set; }        
        public string Status { get; set; }
    }
}
