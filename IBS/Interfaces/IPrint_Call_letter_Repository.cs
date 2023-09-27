using IBS.Models;

namespace IBS.Interfaces
{
    public interface IPrint_Call_letter_Repository
    {
        //public DTResult<Print_Call_letter_Model> query1(string caseNo = "", string callRecvDate = "", string callSno = "");
        public Print_Call_letter_Model query1(string caseNo = "", string callRecvDate = "", string callSno = "");
        public Print_Call_letter_Model query2(string caseNo = "", string callRecvDate = "", string callSno = "");
        public Print_Call_letter_Model CombinedQuery(string caseNo = "", string callRecvDate = "", string callSno = "");
    }
}
