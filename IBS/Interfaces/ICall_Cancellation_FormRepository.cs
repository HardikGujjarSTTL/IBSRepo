using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICall_Cancellation_FormRepository
    {
        public Call_Cancellation_FormModel GetPOS(string caseNo = "", string callRecvDate = "", string callSno = "");
        public Call_Cancellation_FormModel GetINSP(string caseNo = "", string callRecvDate = "", string callSno = "");
        public Call_Cancellation_FormModel Combined(string caseNo = "", string callRecvDate = "", string callSno = "");
        public string SaveDetails(Call_Cancellation_FormModel model, string selectedvalues, string Uname);
        public string delete_details(string caseNo,string calldate, string callsno);
    }
}
