using IBS.Models;

namespace IBS.Interfaces
{
    public interface INCRRegisterRepository
    {
        DTResult<NCRRegister> GetDataList(DTParameters dtParameters);
        public NCRRegister FindByIDActionA(string CASE_NO, string BK_NO, string SET_NO,string NCNO);
        int SaveRemarks(NCRRegister model);
        int Saveupdate(NCRRegister model);
    }
}
