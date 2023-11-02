using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface IDepartmentRepository
    {
        List<DepartmentModel> GetDepartmentList();
    }
}
