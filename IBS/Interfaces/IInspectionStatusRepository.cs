using IBS.Models;

namespace IBS.Interfaces
{
    public interface IInspectionStatusRepository
    {
        public InspectionStatusModel SummaryConsigneeWiseInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string MaterialValue, string FromDate, string ToDate, string ForParticular, string lstParticular, string Regin);
        public InspectionStatusModel SummaryVendorWiseInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string MaterialValue, string FromDate, string ToDate, string ForParticular, string lstParticular, string Regin);
        public InspectionStatusModel SummaryInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string MaterialValue, string FromDate, string ToDate, string ForParticular, string lstParticular, string Regin);
        public InspectionStatusModel VendorWiseInsp(string ReportType, string Month, string Year, string FromDate, string ToDate, string rdbGIE, string rdbForMonth, string ForGPer, string ddlVender, string Regin);
        public List<railway_dropdown> GetValue(string selectedValue);
        public List<railway_dropdown> GetValue2(string selectedValue);
        DTResult<InspectionStatusModel> gridData(DTParameters dtParameters);
        public InspectionStatusModel ICDetailsPO(string ReportType, string PO_NO, string PO_DT, string RLY_NONRLY, string RLY_CD);
    }
}
