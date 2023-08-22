using IBS.Models;

namespace IBS.Interfaces
{
    public interface IComplaintApprovalRepository
    {
        DTResult<OnlineComplaints> GetRejComplaints(DTParameters dtParameters);
        public OnlineComplaints FindByID(string TEMP_COMPLAINT_ID, string SetNo, string BKNo, string CaseNo);
    }
}
