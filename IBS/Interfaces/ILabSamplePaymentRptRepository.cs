using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabSamplePaymentRptRepository
    {

        LabSamplePaymentRptModel GetPaymentReport(string ReportType, string wFrmDtO, string wToDt, string Status, string rbsrdt, string Regin);
    }
}
