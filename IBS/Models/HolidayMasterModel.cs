namespace IBS.Models
{
    public class HolidayMasterModel
    {
        public int? ID { get; set; }
        public string Finance_Year { get; set; }
        public DateTime FY_FR_DT { get; set; }
        public DateTime FY_TO_DT { get; set; }
        public string User_Name { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
