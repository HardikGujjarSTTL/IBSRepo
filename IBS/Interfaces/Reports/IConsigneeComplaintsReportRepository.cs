using IBS.Models;

namespace IBS.Interfaces.Reports
{
    public interface IConsigneeComplaintsReportRepository
    {        
        List<ConsigneeComplaintsReportModel> Get_Consignee_Complaints(string FromDate, string ToDate, UserSessionModel model);
    }
}
