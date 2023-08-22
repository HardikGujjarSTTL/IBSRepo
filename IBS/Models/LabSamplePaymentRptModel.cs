using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class LabSamplePaymentRptModel
    {
        public string ReportStatus { get; set; }
        public string CaseNo { get; set; }
        public string CallRecvDate { get; set; }
        public string SampleRegNo { get; set; }
        public string CallSno { get; set; }
        public string VendorName { get; set; }
        public string ManufacturerName { get; set; }
        public string IEName { get; set; }
        public string Remarks { get; set; }
        public string LabStatus { get; set; }
        public string DocRejRemark { get; set; }
        public string LikelyDateReport { get; set; }
        public string TestingChargesByLab { get; set; }
        public string TestingChargesByVendor { get; set; }
        public string TdsChargesByVendor { get; set; }
        public string CallDocDate { get; set; }
        public string DocStatusFin { get; set; }
        public string VendorInitDate { get; set; }
        public string FinInitDate { get; set; }
        public string SampleRecvDate { get; set; }
        public string UTRNo { get; set; }
        public string UTRDate { get; set; }
    }

}
