using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class LabRegisterFormModel
    {
        public string SampleRegNo { get; set; }
        public string CaseNo { get; set; }
        public decimal AmountReceived { get; set; }
        public decimal TDSAmount { get; set; }
        public DateTime TDSDate { get; set; }
        public decimal TotalLabCharges { get; set; }
    }

}
