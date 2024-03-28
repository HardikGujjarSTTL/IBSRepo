using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabRegFormRepository
    {

        public LABREGISTERModel LoaddataModify(string RegNo);
        DTResult<LABREGISTERModel> GetLabRegDtl(DTParameters dtParameters);
        public LABREGISTERModel LabDtlModify(string RegNo, string SNO);
        List<LABREGISTERModel> LabPaymentModify(string CaseNo, string VCode);
        DTResult<LABREGISTERModel> LapIndexData(DTParameters dtParameters);
        public LABREGISTERModel LabRegisterFormNew(string CaseNo, string CallDt, string CallSno);
        bool SaveDataDetails(LABREGISTERModel LABREGISTERModel);
        bool InsertDataDetails(LABREGISTERModel LABREGISTERModel);
        bool InsertLabReg(LABREGISTERModel LABREGISTERModel);
        bool PrintInvoice(string RegNo, LABREGISTERModel LABREGISTERModel);
        bool PostAmount(LABREGISTERModel LABREGISTERModel);
    }
}
