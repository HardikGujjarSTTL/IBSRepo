using IBS.Models;

namespace IBS.Interfaces
{
    public interface IConsigneeComplaintsRepository
    {
        public ConsigneeComplaints FindByID(string CASE_NO,string BK_NO,string SET_NO);
        //DTResult<ConsigneeComplaints> GetconsigneeComplaintsList(DTParameters dtParameters);
        //List<ConsigneeComplaints> serachconsigneeComplaintsList(string PONO, string PODT);
        DTResult<ConsigneeComplaints> GetDataList(DTParameters dtParameters);
        //int ComplaintsDetailsInsertUpdate(ConsigneeComplaints model);
    }
}
