namespace IBS.Models
{
    public class LabPerfomanceReport
    {
        public string LAB { get; set; }
        public string NO_OF_TEST { get; set; }
        public string NO_OF_SAMPLES { get; set; }
        public string NO_OF_FAILURE { get; set; }
        public string NO_OF_FAIL_SAMPLES { get; set; }
        public string NO_OFNOCOMMENTS { get; set; }
        public string MAXM_DAYS { get; set; }
        public string MIN_DAYS { get; set; }
        public string AVG_DAYS { get; set; }
        public string TOTAL_FEE { get; set; }
        
        public string Region { get; set; }
    }
}
