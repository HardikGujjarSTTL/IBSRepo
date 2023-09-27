using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class TDSEntryModel
    {
        public int ID { get; set; } 

        public string BILL_NO { get; set; }

        public string CASE_NO { get; set; }

        public decimal TDS { get; set; }

        public decimal Retention_Money{ get; set; }

        public decimal WrtOffAmount { get; set; }

        public decimal Bill_Amount	 { get; set; }

        public decimal TDSCGST { get; set; }

        public decimal TDSSGST	 { get; set; }

        public decimal TDSIGST	 { get; set; }

        [Display(Name = "TDS Posting Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [Required]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? TdsDt { get; set; }

        public int? Createdby { get; set; }

    }
}
