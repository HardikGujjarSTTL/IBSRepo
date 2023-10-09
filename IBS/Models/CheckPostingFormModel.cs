using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBS.Models
{

    public class CheckPostingFormModel
    {
        [Key]
        public string? CHQ_NO { get; set; }
        public DateTime CHQ_DATE { get; set; }

        public decimal AMOUNT_RECIEVED { get; set; }

        public double TDS { get; set; }
        public string BILL_NO { get; set; }
        public string BPO_CD { get; set; }
        public double BILL_AMOUNT { get; set; }
        public double Retention_Money { get; set; }
        public double Cnote { get; set; }

        public DateTime BILL_DATE { get; set; }
        public double AMOUNT_CLEARED { get; set; }
        public double BILL_AMOUNT_CLEARED { get; set; }
        public string POSTING_DATE { get; set; }
        public double WriteOffAmount { get; set; }
        public string CASE_NO { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string BPO_NAME { get; set; }


        public double Amount_Adjusted { get; set; }
        public double Suspense_Amt { get; set; }

        public int BANK_CD { get; set; }
        public string BANK_NAME { get; set; }
        public int ACC_CD { get; set; } 

        public DateTime DATETIME { get; set; }
    }
    public class CheckPostingHeader
    {
        public string VCHR_NO { get; set; }

        public DateTime VCHR_DT { get; set; }

        public double Cheaque_amount { get; set; }
        public double posted_amount { get; set; }

        public string paying_authority { get; set; }

        public double amount_transferred { get; set; }

        public double un_adjusted_advance { get; set; }

        public string? CHQ_NO { get; set; }
        public DateTime CHQ_DATE { get; set; }

        public int BANK_CD { get; set; }

        public double Amount_Adjusted { get; set; }
        public double Suspense_Amt { get; set; }

        public string BPO_CD { get; set; }
        public int ACC_CD { get; set; }
    }  

    public class CheckPostingbillInvoice
    {
        public string BILL_NO { get; set; }
        public string Invoice_NO { get; set; }
        public string Bill_DT { get; set; }
        public string CASE_NO { get; set; }

        public string BK_NO { get; set; }

        public string SET_NO { get; set; }

        public string BPO_NAME { get; set; }

        public decimal BILL_AMOUNT { get; set; }

        public decimal AMOUNT_RECIEVED { get; set; }

        public decimal BILL_AMT_CLEARED { get; set; }

        public decimal TDS { get; set; }

        public decimal RETENTION_MONEY { get; set; }

        public decimal WRITE_OFF_AMT { get; set; }

        public decimal CNOTE_AMT { get; set; }

        public string BPO_CD { get; set; }
        public double Amount_Adjusted { get; set; }

        public DateTime POSTING_DATE { get; set; }

        public double AMOUNT_CLEARED { get; set; }



    }
}
