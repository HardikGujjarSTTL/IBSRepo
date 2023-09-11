using IBS.Models;

namespace IBS.Interfaces
{
    public interface IJITopsheetReportRepository
    {
        DTResult<ConsigneeComplaints> GetComplaintReportDetails(DTParameters dtParameters, string Region);
    }
}
