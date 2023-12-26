using IBS.Models;

namespace IBS.Interfaces
{
    public interface IConsigneeComplaintsRepository
    {
        public ConsigneeComplaints FindByID(string CASE_NO, string BK_NO, string SET_NO);
        public ConsigneeComplaints FindByCompID(string ComplaintId);
        DTResult<ConsigneeComplaints> GetDataListConsignee(DTParameters dtParameters);
        DTResult<ConsigneeComplaints> GetDataListComplaint(DTParameters dtParameters);
        string ComplaintsDetailsInsertUpdate(ConsigneeComplaints model);
        string JIChoice(ConsigneeComplaints model);
        string CancelJI(ConsigneeComplaints model);
        string JIOutCome(ConsigneeComplaints model);
        string FinalDisposal(ConsigneeComplaints model);
    }
}
