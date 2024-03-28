using IBS.Models;

namespace IBS.Interfaces
{
    public interface ISuperSurpirseFormRepository
    {

        DTResult<SuperSurpirseFormModel> GetSuperFormData(DTParameters dtParameters, string Regin);
        SuperSurpirseFormModel LoadSuperData(SuperSurpirseFormModel SuperSurpirseFormModel, string Case_No, string CallDt, string CallSNo);
        bool Save(SuperSurpirseFormModel SuperSurpirseFormModel);

    }
}
