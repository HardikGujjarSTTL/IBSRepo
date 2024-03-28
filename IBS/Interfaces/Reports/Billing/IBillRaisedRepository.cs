using IBS.Models;

namespace IBS.Interfaces.Reports.Billing
{
    public interface IBillRaisedRepository
    {
        List<BillRaisedModel> GetReportList(BillRaisedModel model);

        public BillRaisedModel GetBillingClient(int FromMn, int FromYr, int ToMn, int ToYr, string ActionType, string rdo, string Region);

        public BillRaisedModel GetBillingSector(int FromMn, int FromYr, int ToMn, int ToYr, string ActionType, string rdo, string Region, string IncRites);

        public BillRaisedModel GetRailwayOnline(string ClientType, string rdoSummary, string BpoRly, string rdoBpo, int FromMn, int FromYr, DateTime? FromDt, DateTime? ToDt, string ActionType, string Region, string chkRegion);

        public BillRaisedModel GetBillsNotCris(DateTime FromDate, DateTime ToDate, string chkRegion, string ClientType, string lstAU, string actiontype, string Region, string rdbPRly, string rdbPAU);

        public BillRaisedModel GetCNoteInvoice(DateTime? CnoteFromDt, DateTime? CnoteToDt, string ActionType, string Region);
    }
}
