using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class Rly_Online_Check_Posting_Form_Model
    {
        public int BANK_CD { get; set; }

        public string BANK_NAME { get; set;}
        [Key]
        public string Bill_NO { get; set;}
        public string INVOICE_NO { get; set;}
        public decimal BILL_AMOUNT { get; set;}
        public decimal AMOUNT_PASSED { get; set;}
        public string C_07_NO { get; set;}
        public DateTime C_07_DT { get; set;}
        public DateTime PAYMENT_DATE { get; set;}
        public decimal BILL_AMOUNT_CLEARED { get; set;}





        public string CHQ_NO { get; set;}

        public string CHQ_DT { get; set;}
        public string VCHR_NO { get; set;}
        public string BPO_RLY { get; set;}

        public string VCHR_DT { get; set; }
        public decimal CHQ_AMOUNT { get; set; } 
        public decimal POSTED_AMOUNT { get; set; } 
        public string PAYING_AUTHORITY { get; set; } 
        public decimal AMOUNT_TRANSFERRED { get; set; } 
        public decimal SUSPENSE_AMOUNT { get; set; } 
        public decimal AMOUNT_ADJUSTED { get; set; } 
        public string BPO_CD { get; set; } 
        public int ACC_CD { get; set; } 

        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }

    }

    public class SelectedDataModel
    {
        public string BillNo { get; set; }
        public decimal BillAmount { get; set; }
        public decimal PassAmount { get; set; }
        public decimal AmountAdjustment { get; set; }
        public string BPO { get; set; }
    }

    public class RequestDataModel
    {
        public List<Rly_Online_Check_Posting_Form_Model> selectedData { get; set; }
        public Dictionary<string, string> additionalData { get; set; }
    }
}
