using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IVendorFeedbackReportRepository
    {
        public VendorFeedbackReportModel GetVendorFeedbackReport(string Region);
    }
}
