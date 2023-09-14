using IBS.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace IBS.Interfaces.InspectionBilling
{
    public interface ICallRegisterIBRepository
    {
        DTResult<VenderCallRegisterModel> GetDataList(DTParameters dtParameters, string GetRegionCode);

        public VenderCallRegisterModel FindByID(string CaseNo, DateTime? CallRecvDt, string CallSno, string GetRegionCode);

        DTResult<VenderCallRegisterModel> FindByModifyDetail(string CaseNo, string CallRecvDt, int CallSno, string GetRegionCode);

        DTResult<VenderCallRegisterModel> FindMatchDetail(string CaseNo, string CallRecvDt, int CallSno, string GetRegionCode);

        public string GetRegionValue(string CaseNo, string CallRecvDt, string CallSno);

        public VenderCallRegisterModel FindByManageID(string CaseNo, string CallRecvDt, int CallSno, string ActionType, string GetRegionCode);

        DTResult<VenderCallRegisterModel> FindByVenderDetail1(int MfgCd, string CaseNo);

        string RegiserCallSave(VenderCallRegisterModel model);

        Task<string> send_IE_smsAsync(VenderCallRegisterModel model);

        string send_Vendor_Email(VenderCallRegisterModel model);

        string RegiserCallDelete(VenderCallRegisterModel model);

        public VenderCallRegisterModel FindAddDetails(string CaseNo);

        public string GetMatch(string CaseNo, string GetRegionCode);

        public int show2(string CaseNo, string CallRecvDt, int CallSno);

        public string GetCaseNoFind(string CaseNo, string CallRecvDt, int CallSno);

        public VenderCallCancellationModel CancelCallFindByID(string CaseNo, string CallRecvDt, int CallSno, string ActionType);

        public VenderCallCancellationModel CancelCallFindByIDM(string CaseNo, string CallRecvDt, int CallSno, string ActionType);

        string CallCancellationSave(VenderCallCancellationModel model,  string UserName);

        string CallCancelDelete(string CaseNo, string CallRecvDt, int CallSno);

        public VenderCallStatusModel FindCallStatus(string CaseNo, DateTime? CallRecvDt, int CallSno);

        string Save(VenderCallStatusModel model);

        public VendrorCallDetailsModel CallDetailsFindByID(string CaseNo, string CallRecvDt, int CallSno, int ItemSrNoPo);

        DTResult<VendrorCallDetailsModel> GetCallDetailsList(DTParameters dtParameters);

        int CallDetailsSave(VendrorCallDetailsModel model, string UserName);

        bool CallDetailsRemove(VendrorCallDetailsModel model);
    }
}
