using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabReportsRepository
    {
        public LabReportsModel LabRegisterReport(string ReportType, string wFrmDtO, string wToDt, string rdbIEWise, string rdbPIE, string rdbVendWise, string rdbPVend, string rdbLabWise, string rdbPLab, string rdbPending, string rdbPaid, string rdbDue, string rdbPartlyPaid, string lstTStatus, string lstIE, string ddlVender, string lstLab, string Disciplinewise, string rdbPDis, string Discipline, string Regin);
        public LabReportsModel LabPerformanceReport(string ReportType, string wFrmDtO, string wToDt, string Regin);
        public LabReportsModel LabPostingReport(string ReportType, string wFrmDtO, string wToDt, string Regin);
        public LabReportsModel OnlinePaymentReport(string ReportType, string wFrmDtO, string wToDt, string Regin);
        public LabReportsModel LabInvoiceReport(string ReportType, string wFrmDtO, string wToDt, string Regin);
        public LabReportsModel LabSamplePaymentReport(string ReportType, string wFrmDtO, string wToDt, string Regin, string lstStatus, string rdbrecvdt);
        public LabReportsModel BarcodeReport(string ReportType, string wFrmDtO, string wToDt, string Regin);
    }
}
