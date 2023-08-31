using IBS.Models;

namespace IBS.Interfaces
{
    public interface IBill_Paying_Officer_Form 
    {
        public Bill_Paying_Officer_FormModel FindByID(string BpoCd);

        DTResult<Bill_Paying_Officer_FormModel> GetBPOList(DTParameters dtParameters);

        bool Remove(int BpoCd, int UserID);

        string BPOSave(Bill_Paying_Officer_FormModel model);

        string GetState (int BpoCityCd);
    }
}
