using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBS.Models
{
    public class AddRecieptVoucherModel 
    {
        
          
            public string? VCHR_NO { get; set; }
            public string VCHR_DT { get; set; }
            public string BANK_CD { get; set; }
            public string BANK_NAME { get; set; }
            public string VCHR_TYPE { get; set; }

           
            public int SNO { get; set; }
            public string CHQ_NO { get; set; }
            public string CHQ_DT { get; set; }
            public double AMOUNT { get; set; }
            public string ACC_CD { get; set; }
            public double AMOUNT_ADJUSTED { get; set; }
            public double SUSPENSE_AMT { get; set; }
            public string NARRATION { get; set; }
            public string SAMPLE_NO { get; set; }
            public DateTime POST_DT { get; set; }
            public string STATUS { get; set; }
            public string BPO_CD { get; set; }
            public string BPO_TYPE { get; set; }
            public string CASE_NO { get; set; }
            public double AMT_TRANSFERRED { get; set; }
            public string USER_ID { get; set; }
            public DateTime DATETIME { get; set; }
            public List<BPOlist> BPOList { get; set; }
             
            

    }
}
