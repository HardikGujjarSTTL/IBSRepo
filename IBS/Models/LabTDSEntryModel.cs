using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class LabTDSEntryModel
    {
        public string SampleRegNo { get; set; }
        public string CaseNo { get; set; }
        public string AmountReceived { get; set; }
        public string TDSAmount { get; set; }
        public string TDSDate { get; set; }
        public string TotalLabCharges { get; set; }
    }

}
