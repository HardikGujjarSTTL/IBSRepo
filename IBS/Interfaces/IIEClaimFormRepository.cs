using IBS.Models;

namespace IBS.Interfaces
{
    public interface IIEClaimFormRepository
    {
        public DTResult<IECliamFormModel> IE_List(DTParameters dtParameters);
        public IECliamFormModel FindByID(string CLAIM_NO , string ACTION , decimal ID);
        public string InsertIE(IECliamFormModel model, string Region, int uname);

        public DTResult<IECliamFormModel> Manage_grid(DTParameters dtParameters);
        public string Payment_Save(string CLAIM_NO ,string  VCHR_NO ,string  VCHR_DT); 
    }
}
