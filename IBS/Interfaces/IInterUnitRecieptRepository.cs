using IBS.Models;

namespace IBS.Interfaces
{
    public interface IInterUnitRecieptRepository
    {
        public List<BPOlist> GetDistinctBPOsByCaseNo(string txtCSNO, string txtBPOtype, string BPOCD);
        public List<BPOlist> fill_BPO(string txtCSNO, string lstBPO, string txtBPOtype);
        public List<BPOlist> fill_BPO01(string txtCSNO, string lstBPO, string txtBPOtype);
        public List<BPOlist> ChkCSNO(string txtCSNO);
        public string InterUnitRecieptSave(InterUnitRecieptModel model, string Region);
        public InterUnitRecieptModel FindByID(string VCHR_NO, int BANK_CD, string CHQ_NO, string CHQ_DT, string VCHR_DT);
        public DTResult<InterUnitRecieptModel> RecieptList(DTParameters dtParameters, string Region);

        public bool Remove(string VCHR_NO, string CHQ_NO, string CHQ_DT, int BANK_CD, int U_ID);



    }
}
