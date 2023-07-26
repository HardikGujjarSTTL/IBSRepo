using IBS.Models;

namespace IBS.Interfaces
{
    public interface IRly_Designation_Form
    {
        public Rly_Designation_FormModel FindByID(string RlyDesigCd);
        DTResult<Rly_Designation_FormModel> GetRly_Designation_FormList(DTParameters dtParameters);
        bool Remove(string RlyDesigCd, int UserID);
        string Rly_Designation_FormInsertUpdate(Rly_Designation_FormModel model);
    }
}
