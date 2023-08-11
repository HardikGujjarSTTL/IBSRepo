using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class LABREGISTERModel
    {
        public string SampleRegNo { get; set; }
        public DateTime SampleRegDate { get; set; }
        public string CaseNo { get; set; }
        public DateTime CallDateAndSno { get; set; }
        public string Vendor { get; set; }
        public string VendorCode { get; set; }
        public string IE { get; set; }
        public string IECode { get; set; }
        public DateTime SampleDispatchDate { get; set; }
        public DateTime SampleDrawalDate { get; set; }
        public DateTime SampleReceiptDate { get; set; }
        public string TestingType { get; set; }
        public string TotalTestingFee { get; set; }
        public string TotalHandlingCharges { get; set; }
        public string TotalServiceTax { get; set; }
        public string TotalLabCharges { get; set; }
        public string TotalTDS	 { get; set; }
        public string AmountRecieved { get; set; }
        public string TotalAmountCleared { get; set; }
        public string CodeNo { get; set; }
        public DateTime CodeDate { get; set; }
    }

}
