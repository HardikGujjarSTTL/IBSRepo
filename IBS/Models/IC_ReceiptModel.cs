using System.Drawing;

namespace IBS.Models
{
    public class IC_ReceiptModel
    {
        public string REGION { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public int IE_CD { get; set; }
        public string? IC_SUBMIT_DT { get; set; } //DateTime
        public string BILL_NO { get; set; }
        public string USER_ID { get; set; }
        public DateTime DATETIME { get; set; }
        public string REMARKS { get; set; }
        public string? REMARKS_DT { get; set; } //DateTime

        public string RDT { get; set; }
    }

    public class IC_Receipt_Grid_Model
    {
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string IE_NAME { get; set; }
        public string IC_SUBMIT_DT { get; set; }
    }

    public class IC_StatusModel
    {
        public int IE_CD { get; set; }
        public string IE_NAME { get; set; }
    }
}
