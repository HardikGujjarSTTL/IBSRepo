namespace IBS.Models
{
    public class ProjectDetails
    {
        public int In_ID { get; set; }

        public int Proj_ID { get; set; }

        public int ProjDetail_ID { get; set; }

        public string ProjectName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public int Numbers { get; set; }

        public string Disc { get; set; }

        public string SancStrength { get; set; }

        public string SanctionedFile { get; set; }

        public int? Createdby { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public List<ProjectDetails> lstProjectDetails { get; set; }
    }
}
