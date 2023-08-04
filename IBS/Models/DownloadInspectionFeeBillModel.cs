using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class DownloadInspectionFeeBillModel
    {
        public string CaseNo { get; set; }

        public string BkNo { get; set; }

        public string SetNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? CallRecvDt { get; set; }

        public string CallInstallNo { get; set; }

        public string? CallSNo { get; set; }

        public string? CallStatus { get; set; }

        public string PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        public string? IeSName { get; set; }

        public string? Vendor { get; set; }

        public string? BillNo { get; set; }
        
        public string? BillDt { get; set; }
    }
}
