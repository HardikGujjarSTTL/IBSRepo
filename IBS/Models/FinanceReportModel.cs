using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class FinanceReportModel
    {
        public string Region { get; set; }

        public string Acc_Cd { get; set; }

        public string BANK_CD { get; set; }

        public string ActionType { get; set; }

        public string Title { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        public List<FinanceReportList> lstFinanceReport { get; set; }
    }

    public class FinanceReportList
    {
        public string VCHR_NO { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? VCHR_DT { get; set; }

        public string CHQ_NO { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CHQ_DT { get; set; }

        public string BANK_NAME { get; set; }

        public decimal? AMOUNT { get; set; }

        public string NARRATION { get; set; }

        //public string RV_VCHR_NO { get; set; }

        //public string RV_SNO { get; set; }

        //public string BANK_CD { get; set; }

        //public string BPO { get; set; }

        //public decimal? AMT_TRANSFERRED { get; set; }

        //public decimal? SUSPENSE_AMT { get; set; }

        //public string ACC_CD { get; set; }

        //public decimal? TRANS_AMOUNT { get; set; }

    }
}
