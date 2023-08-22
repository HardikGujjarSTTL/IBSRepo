using IBS.Models;

namespace IBS.Interfaces
{
    public interface IBill_Paying_Officer_Form 
    {
        public Bill_Paying_Officer_FormModel FindByID(int BpoCd);
        DTResult<Bill_Paying_Officer_FormModel> GetBPOList(DTParameters dtParameters);
        bool Remove(int BpoCd, int UserID);
        int BPODetailsInsertUpdate(Bill_Paying_Officer_FormModel model);
    }
}
