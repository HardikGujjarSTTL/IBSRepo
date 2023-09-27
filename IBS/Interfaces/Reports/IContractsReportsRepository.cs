using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IContractsReportsRepository
    {
        public ContractReportModel GetContractDetails(string FromDate, string ToDate, string Region, string clientname);
    }
}
