using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class HolidayMasterModel
    {
        public int? ID { get; set; }
        [Required(ErrorMessage = "Financial Year is required")]
        [MaxLength(9)]
        [RegularExpression("^[0-9]{1,4}[-]{1,1}[0-9]{1,4}$", ErrorMessage = "Financial Year invalid")]
        public string Finance_Year { get; set; }
        [Required(ErrorMessage = "Financial From Date is required")]
        public DateTime? FY_FR_DT { get; set; }
        [Required(ErrorMessage = "Financial To Date is required")]
        public DateTime? FY_TO_DT { get; set; }
        public string User_Name { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Region { get; set; }
    }

    public class HolidayDetailModel
    {
        public int? ID { get; set; }
        public int? HOLIDAY_ID { get; set; }
        [Required(ErrorMessage = "Holiday Date is required")]        
        public DateTime? HOLIDAY_DT { get; set; }
        [Required(ErrorMessage = "Holiday Description is required")]
        public string HOLIDAY_DESC { get; set; }
        public int? USER_ID { get; set; }
        public string? USER_NAME { get; set; }
        //public int? CREATEDBY { get; set; }
        //public DateTime? CREATEDDATE { get; set; }
        //public int? UPDATEDBY { get; set; }
        //public DateTime? UPDATEDDATE { get; set; }
        //public int? ISDELETED { get; set; }

        public string? FINANCIAL_YEAR { get; set; }
        public string REGION { get; set; }

    }
}
