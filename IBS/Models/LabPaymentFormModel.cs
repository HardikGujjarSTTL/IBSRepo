using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class LabPaymentFormModel
    {
        public LabPaymentFormModel SampleDetails { get; set; }
        public string PaymentID { get; set; }

        public string PaymentDt { get; set; }

        public string LabCd { get; set; }

        public string Lab { get; set; }
        public string Amount { get; set; }
        public string Regin { get; set; }

        public string CHQ_NO { get; set; }
        public string CHQ_DT { get; set; }
        public string SAMPLE_REG_NO { get; set; }
        public string CASE_NO { get; set; }
        public string SNO { get; set; }
        public string TOT_CHARGES { get; set; }
        public string TDS_AMT { get; set; }
        public string TESTING_FEE { get; set; }
        public string AMT_CLEARED { get; set; }
        public string IAMOUNT { get; set; }
        public string Bank { get; set; }
        public string Remarks { get; set; }
        public string UserId { get; set; }


    }

}
