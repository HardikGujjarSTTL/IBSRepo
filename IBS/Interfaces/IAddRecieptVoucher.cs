using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Interfaces
{
    public interface IAddRecieptVoucher 
    {
        DTResult<AddRecieptVoucherModel> GetVoucherList(DTParameters dtParameters);
        public AddRecieptVoucherModel FindByID(string VCHR_NO, int BANK_CD, string CHQ_NO, string CHQ_DT);

        string VoucherDetailsSave(AddRecieptVoucherModel model, string Region);

         string ChkCSNO(string txtCSNO, string lstBPO, out string Narrt);

        public List<BPOlist> GetDistinctBPOsByCaseNo(string txtCSNO);






        }
}
