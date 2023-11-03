using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Interfaces
{
    public interface INCRRegisterRepository
    {
        DTResult<NCRRegister> GetDataList(DTParameters dtParameters);
        DTResult<Remarks> GetRemarks(DTParameters dtParameters);
        public NCRRegister FindByIDActionA(string CASE_NO, string BK_NO, string SET_NO,string NCNO,string Actions);
        string SaveRemarks(string NCNO,string UserID, List<Remarks> model);
        string GetItems(string CaseNo, string BKNo, string SetNo);
        string GetQtyByItems(string CaseNo, string CALLRECVDT, string CALLSNO, string ItemSno);
        public NCRRegister Saveupdate(NCRRegister model,bool isRadioChecked,string extractedText);
        public NCRRegister SaveMoreNC(NCRRegister model,string extractedText);
        bool SendEmail(NCRRegister nCRRegister);
        int ShouldRemarkDisable(string NCNO);
        List<SelectListItem> GetNcrCd(string NCRClass);
    }
}
