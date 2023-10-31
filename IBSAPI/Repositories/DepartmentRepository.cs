using IBSAPI.Interfaces;
using IBSAPI.Models;

namespace IBSAPI.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        public List<DepartmentModel> GetDepartmentList()
        {
            List<DepartmentModel> deptList = new List<DepartmentModel>()
            {
                new DepartmentModel(){ ID = "M", Name = "Mechanical"},
                new DepartmentModel(){ ID = "E", Name = "Electrical"},
                new DepartmentModel(){ ID = "C", Name = "Civil"},
                new DepartmentModel(){ ID = "L", Name = "Metallurgy"},
                new DepartmentModel(){ ID = "T", Name = "Textiles"},
                new DepartmentModel(){ ID = "P", Name = "Power Engineering"}
            };
            return deptList;
        }
    }
}
