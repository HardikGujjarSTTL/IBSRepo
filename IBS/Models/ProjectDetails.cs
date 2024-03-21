using IBS.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class ProjectModel
    {
        public int Proj_ID { get; set; }

        public int ProjDetail_ID { get; set; }

        [Required(ErrorMessage = "Project Name is required")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Completion Date is required")]
        public DateTime? CompletionDate { get; set; }

        public int? Numbers { get; set; }

        public string? Disc { get; set; }

        public string? SancStrength { get; set; }

        public string SanctionedFile { get; set; }

        public int? Createdby { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public List<ProjectDetailsModel> lstProjectDetails { get; set; }

        public List<ProjectDetailsReport> lstprojectDetailsReports { get; set; }

        public List<ManpowerModel> lstManpowerModels { get; set; }
    }

    public class ProjectDetailsModel
    {
        public int DetailID { get; set; }

        public int ProjId { get; set; }

        public string? Sanctionedstrength { get; set; }
        public string? SanctionedstrengthText { get; set; }

        public string? Department { get; set; }
        public string? DepartmentText { get; set; }

        public decimal? Nos { get; set; }
    }

    public class ProjectDetailsReport
    {
        public string Sanctioned_Strength { get; set; }
        public int Mech { get; set; }
        public int Elect { get; set; }
        public int Civil { get; set; }
        public int M_C { get; set; }
        public int Others { get; set; }
        public int Total { get; set; }
    }

    public class IEAndCallReport
    {
        public string RIO { get; set; }
        public int Mechanical { get; set; }
        public int Electrical { get; set; }
        public int Civil { get; set; }
        public int Metallurgy { get; set; }
        public int Textiles { get; set; }
        public int PowerEngineering { get; set; }
        public int TotalNoofIEs { get; set; }
        public decimal TotalNoofcalls { get; set; }
        public decimal NoofcallsperIEduringtheperiod { get; set; }
    }
}
