using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabRegFormRepository
    {

        public LABREGISTERModel LoaddataModify(string RegNo);
        List<LABREGISTERModel> GetLabRegDtl(string RegNo,string SNO);
        public LABREGISTERModel LabDtlModify(string RegNo, string SNO);
        List<LABREGISTERModel> LabPaymentModify(string CaseNo, string VCode);
        List<LABREGISTERModel> LapIndexData(string CaseNo, string CallRdt, string RegNo);
        public LABREGISTERModel LabRegisterFormNew(string CaseNo, string CallDt, string CallSno);
        bool SaveDataDetails(LABREGISTERModel LABREGISTERModel);
        bool InsertLabReg(LABREGISTERModel LABREGISTERModel);
        bool PrintInvoice(string RegNo, LABREGISTERModel LABREGISTERModel);
        bool PostAmount(LABREGISTERModel LABREGISTERModel);
    }
}
