using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class BillCheckPostingModel
    {
        public string? ChqNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? ChqDt { get; set; }

        public string BankName { get; set; }

        public string VcharNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? VcharDt { get; set; }

        public double ChqAmount { get; set; }

        public double PostedAmount { get; set; }

        public string PayingAuthority { get; set; }

        public double AmountTransferred { get; set; }

        public double UnAdjustedAdvance { get; set; }
    }
}
