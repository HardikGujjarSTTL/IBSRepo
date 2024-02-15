using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class MultipleFileUploadModel
    {
        //public List<IFormFile> Files { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        public List<IFormFile> Files { get; set; }

        public string CaseNo { get; set; }

        public DateTime? CallRecvDt { get; set; }

        public string CallSno { get; set; }

        public string PoNo { get; set; }

        public string PoDt { get; set; }

        public string BillNO { get; set; }

        public string FileName { get; set; }

        public string IC_NO { get; set; }

        public DateTime? BillDT { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? IC_DT { get; set; }

        public string BKNO { get; set; }

        public string SetNo { get; set; }
    }
}
