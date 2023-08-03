namespace IBS.Models
{
    public class HologramAccountalModel
    {

        public string CASE_NO { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string IE_SNAME { get; set; }
        public string HG_NO_MATERIAL { get; set; }
        public string HG_NO_SAMPLE { get; set; }
        public string HG_NO_TEST { get; set; }
        public string HG_NO_IC { get; set; }
        public string HG_NO_IC_DOC { get; set; }
        public string HG_NO_OT { get; set; }
    }

    public class HologramAccountalSearchModel
    {
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
    }
}
