using IBS.Models;

namespace IBS.Interfaces
{
    public interface IComplaintsJIRequiredReportRepository
    {
        string GetItems(string Clientwise);
        DTResult<JIRequiredReport> GetJIRequiredList(DTParameters dtParameters,string Region);
    }
}
