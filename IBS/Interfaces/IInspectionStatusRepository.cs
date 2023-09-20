using IBS.DataAccess;
using IBS.Models;
using System.Data;

namespace IBS.Interfaces
{
    public interface IInspectionStatusRepository
    {
        public SummaryConsigneeWiseInspModel SummaryConsigneeWiseInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string MaterialValue, string FromDate, string ToDate, string ForParticular, string lstParticular, string Regin);
        public SummaryVendorWiseInspModel SummaryVendorWiseInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string MaterialValue, string FromDate, string ToDate, string ForParticular, string lstParticular, string Regin);
    }
}
