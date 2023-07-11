using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class UserModel
    {
        public string UserId { get; set; } = null!;

        public string? UserName { get; set; }

        public string? RitesEmp { get; set; }

        public string? EmpNo { get; set; }

        public string? Region { get; set; }

        public string? Password { get; set; }

        public byte? AuthLevl { get; set; }

        public string? Status { get; set; }

        public string? AllowPo { get; set; }

        public string? AllowUpChksht { get; set; }

        public string? AllowDnChksht { get; set; }

        public string? CallMarking { get; set; }

        public string? CallRemarking { get; set; }

        public string? UserType { get; set; }
    }

    public class UserAuthorizationModel
    {
        public int ModuleID { get; set; }

        public int SubModuleID { get; set; }

        public string ModuleName { get; set; }

        public string SubModuleName { get; set; }

        public bool ViewAccess { get; set; }

        public bool AddEdit { get; set; }
    }

    public class UserAppoitmentModel
    {
        public int JTS_AppointmentCategoryId { get; set; }

        public string CategoryName { get; set; }

        public bool Permission { get; set; }
    }
}
