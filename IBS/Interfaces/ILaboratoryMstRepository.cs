using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILaboratoryMstRepository
    {

        DTResult<LaboratoryMstModel> GetLaboratoryMstList(DTParameters dtParameters);
        public LaboratoryMstModel FindByID(int LabID);

        int LabDetailsInsertUpdate(LaboratoryMstModel model);
    }
}
