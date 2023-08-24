using IBS.Models;

namespace IBS.Interfaces
{
    public interface IConsigneeComplaintsRepository
    {
        public ConsigneeComplaints FindByID(string CASE_NO,string BK_NO,string SET_NO);
        //DTResult<ConsigneeComplaints> GetconsigneeComplaintsList(DTParameters dtParameters);
        //List<ConsigneeComplaints> serachconsigneeComplaintsList(string PONO, string PODT);
        List<ConsigneeComplaints> GetDataListConsignee(string poNo, string poDt);
        List<ConsigneeComplaints> GetDataListComplaint(string poNo, string poDt);
        //int ComplaintsDetailsInsertUpdate(ConsigneeComplaints model);
    }
}
