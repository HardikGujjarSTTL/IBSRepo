using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IOngoingContractsReportRepository
    {
        public OngoingContrcatsReportModel Getongoingcontractdetails(string StatusOffer, string Region, string StatusOffertxt, string Regiontxt, string rdoregionwise);
    }
}
