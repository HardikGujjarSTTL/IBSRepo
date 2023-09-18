using IBS.Models;

namespace IBS.Interfaces
{
    public interface IJITopsheetReportRepository
    {
        public ConsigneeComplaints GetComplaintReportDetails(string JISNO, string Region);
    }
}
