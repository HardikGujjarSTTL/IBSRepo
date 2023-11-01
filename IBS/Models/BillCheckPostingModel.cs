using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class BillCheckPostingModel
    {
        public string? ChqNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? ChqDt { get; set; }

        public string BankName { get; set; }

        public string VcharNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? VcharDt { get; set; }

        public decimal? ChqAmount { get; set; }

        public string Bpo { get; set; }

        public string BpoCd { get; set; }

        public string BpoName { get; set; }

        public decimal? Amount { get; set; }

        public decimal? AmountCleared { get; set; }

        public decimal? AmountAdjusted { get; set; }

        public decimal? AmtTransferred { get; set; }

        public decimal? SuspenseAmt { get; set; }

        public int? AccCd { get; set; }

        public string BillInvoiceNo { get; set; }

        public string BillNo { get; set; }

        public string InvoiceNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? BillDt { get; set; }

        public decimal? BillAmount { get; set; }

        public decimal? AmtRecieved { get; set; }

        public decimal? BillAmtCleared { get; set; }

        public decimal? TDS { get; set; }

        public decimal? RetentionMoney { get; set; }

        public decimal? CNoteAmt { get; set; }

        public decimal? WriteOffAmt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? PostingDt { get; set; }

        public decimal? AmtToRec { get; set; }

        public decimal? AmtTOClear { get; set; }

        public string CaseNo { get; set; }

        public string BkNo { get; set; }

        public string SetNo { get; set; }

        public string UserId { get; set; }

        public string ActionType { get; set; }

        public string Status { get; set; }

        public DateTime? Datetime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

    }

    public class BillCheckPostingModelList
    {
        public string BankName { get; set; }

        public string? ChqNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? ChqDt { get; set; }

        public string BillNo { get; set; }

        public decimal? BillAmount { get; set; }

        public decimal? AmountCleared { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? PostingDt { get; set; }

        public decimal? BillAmtCleared { get; set; }

        public string BpoName { get; set; }

        public DateTime? Datetime { get; set; }
    }

}
