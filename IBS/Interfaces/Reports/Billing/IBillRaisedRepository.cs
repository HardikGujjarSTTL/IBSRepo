using IBS.Models;

namespace IBS.Interfaces.Reports.Billing
{
    public interface IBillRaisedRepository
    {
        List<BillRaisedModel> GetBillingData(BillRaisedModel model);

        DTResult<BillRaisedModel> GetDataList(DTParameters dtParameters);
    }
}
