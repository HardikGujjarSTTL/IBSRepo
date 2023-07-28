using IBS.Models;

namespace IBS.Interfaces
{
    public interface IAddRecieptVoucher 
    {
        DTResult<AddRecieptVoucherModel> GetVoucherList(DTParameters dtParameters);
        public AddRecieptVoucherModel FindByID(string Vchr_No, string Case_No, string Chq_no);

        int VoucherDetailsSave(AddRecieptVoucherModel model, string Region);





    

            
        }
}
