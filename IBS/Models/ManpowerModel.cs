namespace IBS.Models
{
    public class ManpowerModel
    {
        public int ID { get; set; }
        public string Region { get; set; }
        public string EmpName { get; set; }
        public string EmpNo { get; set; }
        public string Desig { get; set; }
        public string? Cadre { get; set; }
        public string Discp { get; set; }
        public string Status { get; set; }
        public DateTime? Dob { get; set; }
        public DateTime? RitesDt { get; set; }
        public DateTime? RioDt { get; set; }
        public DateTime? DrrtDt { get; set; }

        public string? Working { get; set; }
        public string? Staff { get; set; }
        public string? PlacePosting { get; set; }
        public string? ProjectName { get; set; }

        public virtual int UserID { get; set; }
        public virtual string UserName { get; set; }
        public List<ManpowerDetailModel> lstManpowerDetailModel { get; set; }
    }

    public class ManpowerDetailModel
    {
        public int DetailID { get; set; }
        public int ManpowerID { get; set; }
        public string Working { get; set; }
        public string WorkingText { get; set; }
        public string Staff { get; set; }
        public string StaffText { get; set; }
        public string PlacePosting { get; set; }
        public int? ProjectName { get; set; }
        public string ProjectNameText { get; set; }
        public int? UserID { get; set; }
    }
}
