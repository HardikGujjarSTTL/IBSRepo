using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICallsReportRepository
    {
        public Statement_IeVendorWiseModel Statement_IeVendorWise(string ReportType, string frmDate, string toDate, string Region);
        public Statement_IeVendorWiseModel Statement_OverdueCalls(string ReportType, string WiseRadio, string IeStatus, int Days, string includeNSIC, string pendingCallsOnly, string Region);
        public Statement_IeVendorWiseModel Statement_ApprovalReport(string ReportType, string frmDate, string toDate, string Region);
        public List<railway_dropdown1> GetValue(string selectedValue);
        public List<railway_dropdown1> GetValue2(string selectedValue);
        public DTResult<Statement_IeVendorWiseModel> gridData(DTParameters dtParameters);
        public Statement_IeVendorWiseModel Statement_SpecificPO(string ReportType, string PO_NO, string PO_DT, string RLY_NONRLY, string RLY_CD);
        public Statement_IeVendorWiseModel Statement_CallMarked(string ReportType, string frmDate, string toDate, string wSortkEy, string Region);
    }
}
