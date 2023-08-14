using IBS.Models;

namespace IBS.Interfaces.Reports
{
    public interface IBillRegisterRepository
    {
        DTResult<BillRegisterModel> GetDataList(DTParameters dtParameters);

        int DetailsSave(BillRegisterModel model, string commaSeparatedString, string Region);
    }
}
