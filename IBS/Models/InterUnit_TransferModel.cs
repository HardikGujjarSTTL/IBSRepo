using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class InterUnit_TransferModel
    {
        public string VCHR_NO { get; set; }
        public string VCHR_DT { get; set; }
        public int SNO { get; set; }
        public string CHQ_NO { get; set; }
        public string CHQ_DT { get; set; }
        public string BANK_NAME { get; set; }
        public int BANK_CD { get; set; }
        public string BPO { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal AMT_TRANSFERRED { get; set; }
        public decimal SUSPENSE_AMT { get; set; }


        public string JV_NO { get; set; }
        public string JV_DT { get; set; }
        
        public InterUnitTransferRegionModel Transfer { get; set; }
        public string Region_ID { get; set; }       
    }

    public class InterUnitTransferRegionModel
    {
        public int ID { get; set; }
        public string ACC_CD { get; set; }

        [MaxLength(12)]
        [MinLength(1)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid Amount")]
        public string R_AMOUNT { get; set; }
        public string NARRATION { get; set; }
        public string RNO { get; set; }
        public string RDT { get; set; }
    }

    public class InterUnit_Transfer_JVModel
    {
        public string JV_NO { get; set; }
        public string JV_DT { get; set; }
        public string VCHR_NO { get; set; }

        public string VCHR_DT { get; set; }

    }
}
