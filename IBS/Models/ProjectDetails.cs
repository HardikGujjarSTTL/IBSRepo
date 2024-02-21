namespace IBS.Models
{
    public class ProjectDetails
    {
        public int In_ID { get; set; }

        public string ProjectName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public int Numbers { get; set; }

        public string Disc { get; set; }

        public string SancStrength { get; set; }

        public List<ProjectDetails> lstProjectDetails { get; set; }
    }
}
