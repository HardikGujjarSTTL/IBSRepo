using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces.Reports.Billing
{
    public interface IBillRaisedRepository
    {
        List<BillRaisedModel> GetReportList(BillRaisedModel model);

        public BillRaisedModel GetBillingClient(int FromMn, int FromYr, int ToMn, int ToYr, string ActionType, string rdo, string Region);

        public BillRaisedModel GetBillingSector(int FromMn, int FromYr, int ToMn, int ToYr, string ActionType, string rdo, string Region, string IncRites);

    }
}
