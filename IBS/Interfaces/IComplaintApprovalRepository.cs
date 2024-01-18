using IBS.Models;

namespace IBS.Interfaces
{
    public interface IComplaintApprovalRepository
    {
        DTResult<OnlineComplaints> GetRejComplaints(DTParameters dtParameters, string Region);
        public OnlineComplaints FindByID(string TEMP_COMPLAINT_ID, string SetNo, string BKNo, string CaseNo);
        string AcceptComplaint(OnlineComplaints model);
        string SubmitAcceptRecord(OnlineComplaints model);
        string RejectComp(OnlineComplaints model);
        string GetItems(string InspRegionDropdown);
    }
}
