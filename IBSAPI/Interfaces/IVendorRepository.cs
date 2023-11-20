using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface IVendorRepository
    {
        //List<CallRegiModel> GetCaseDetailsforvendor(int UserID);
        PODetailsModel GetPODetailsforvendor(string CaseNo,int UserID);
        List<CallRegiModel> GetCaseDetailsforClient(string UserID, string Organisation, string OrgnType);
        public VenderCallRegisterModel FindByAddDetails(string CaseNo, DateTime? CallRecvDt, string CallStage, int UserId);
        string GetMatch(string CaseNo, string UserName);
        List<VenderCallRegisterModel> GetVenderListM(RequestVenderCallRegisterModel model);
        public VenderCallRegisterModel GetValidate(VenderCallRegisterModel model);
        string RegiserCallSave(VenderCallRegisterModel model);
        string UpdateCallDetails(VenderCallRegisterModel model, int ItemSrnoPo);
        int DetailsInsertUpdate(RequestUpdateManufacturerDetailsModel model);
    }
}
