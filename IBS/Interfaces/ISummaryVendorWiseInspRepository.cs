using IBS.Models;

namespace IBS.Interfaces
{
    public interface ISummaryVendorWiseInspRepository
    {
        public SummaryVendorWiseInspModel SummaryVendorWiseInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string MaterialValue, string FromDate, string ToDate, string ForParticular, string lstParticular, string Regin);

    }
}
