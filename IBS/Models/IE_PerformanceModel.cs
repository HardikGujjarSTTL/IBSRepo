using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class IE_PerformanceModel
    {
        public string IE_NAME { get; set; }
        public string DEPT { get; set; }
        public string C3 { get; set; }
        public string C7 { get; set; }
        public string CM7 { get; set; }
        public string C10 { get; set; }
        public string C0 { get; set; }
        public string INSP_FEE { get; set; }
        public string MATERIAL_VALUE { get; set; }
        public string AVERAGE_FEE { get; set; }
        public string CALLS { get; set; }
        public string CALL_CANCEL { get; set; }
        public string REJECTIONS { get; set; }
    }

    public class IEFromToDate
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? FromDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ToDt { get; set; }
    }

    public class IEPerformanceFilter
    {
        public string Region { get; set; }
        public string UserName { get; set; }
        public string IE_CD { get; set; }
    }

    public class IEPerformanceSummary
    {
        public string Rejection { get; set; }
        public string NoOfIcs { get; set; }
        public string CallsWithin { get; set; }
        public string CallsBeyond { get; set; }
        public List<IEPerformanceSummaryFooter> IEPerSumFooter { get; set; }
    }

    public class IEPerformanceSummaryFooter
    {
        public string RLY_NONRLY { get; set; }
        public int IC_COUNT { get; set; }
        public decimal MATERIAL_VALUE { get; set; }
    }
}
