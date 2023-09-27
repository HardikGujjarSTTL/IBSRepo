using IBS.Models;

namespace IBS.Interfaces
{
    public interface IRly_Online_Check_Posting_Form_Repository
    {
        public Rly_Online_Check_Posting_Form_Model GetTextboxValues(string BankNameDropdown, string CHQ_NO, string CHQ_DT, string region);
        public DTResult<Rly_Online_Check_Posting_Form_Model> BillList(DTParameters dtParameters, string Region);
        public string Submit(RequestDataModel requestData, string Uname);
    }
}
