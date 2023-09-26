namespace IBS.Models.Reports
{
    public class IEWiseTrainingReportModel
    {
        public string IENAME { get; set; }
        public string TrainingArea { get; set; }
        public string Mechanical { get; set; }
        public string Electrical { get; set; }
        public string Civil { get; set; }
        public string Regular { get; set; }
        public string Deputaion { get; set; }
        public string Particularie { get; set; }
        public string ParticularArea { get; set; }
        public string ReportTitle { get; set; }

        public List<IEWISETRAINING> lstIEWISETRAINING { get; set; }
    }

    public class IEWISETRAINING
    {
        public string NAME { get; set; }
        public string EMP_NO { get; set; }
        public string CATEGORY { get; set; }
        public string QUAL { get; set; }
        public string T_TYPE { get; set; }
        public string T_FEILD { get; set; }
        public string COURSE_NAME { get; set; }
        public string COURSE_INSTITUTE { get; set; }
        public string C_DUR_FR { get; set; }
        public string C_DUR_TO { get; set; }
        public string CERTIFICATE { get; set; }
        public string FEES { get; set; }
        public string VALIDITY { get; set; }
    }
}
