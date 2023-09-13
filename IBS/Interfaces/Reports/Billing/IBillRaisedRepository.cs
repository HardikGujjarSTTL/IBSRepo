using IBS.Models;

namespace IBS.Interfaces.Reports.Billing
{
    public interface IBillRaisedRepository
    {
        DTResult<BillRaisedModel> GetDataList(DTParameters dtParameters);

        List<BillRaisedModel> GetReportList(BillRaisedModel model);
    }
}
