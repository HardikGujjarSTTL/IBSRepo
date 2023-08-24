using IBS.Models;

namespace IBS.Interfaces
{
    public interface IRailwaysDesignationRepository
    {
        public Rly_Designation_FormModel FindByID(string RlyDesigCd);

        DTResult<Rly_Designation_FormModel> GetRailwaysDesignationList(DTParameters dtParameters);

        public bool IsDuplicate(Rly_Designation_FormModel model);

        public void SaveDetails(Rly_Designation_FormModel model);

        public bool Remove(string RlyDesigCd);

        public string IsExistsRailwayDesignationCode(string RlyDesigCd);

    }
}
