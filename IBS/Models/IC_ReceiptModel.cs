using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace IBS.Models
{
    public class IC_ReceiptModel
    {
        public string REGION { get; set; }

        [Required(ErrorMessage ="Book No is required")]
        public string BK_NO { get; set; }

        [Required(ErrorMessage = "Set No is required")]
        public string SET_NO { get; set; }

        [Required(ErrorMessage = "Please selec IE CD")]
        public int IE_CD { get; set; }
        public string IC_SUBMIT_DT { get; set; } //DateTime
        public string BILL_NO { get; set; }
        public int USER_ID { get; set; }
        public string USER_NAME { get; set; }
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

    public class ICFromToModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? FromDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ToDt { get; set; }

        public string ICType { get; set; }
    }

    public class ICReportModel
    {
        public string SUBMIT_DATE { get; set; }
        public string IC_SUBMIT_DATE { get; set; }
        public string IE_NAME { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string CLIENT_TYPE { get; set; }
        public string REMARKS { get; set; }
        public string REMARKS_DATE { get; set; }
        public string IC_DATE { get; set; }
    }

    public class ICIssueNotReceiveModel
    {
        public string IC_ISSUED_DT { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string IE_NAME { get; set; }
        public string CO_NAME { get; set; }
        public string CASE_NO { get; set; }
        public string PO_SOURCE { get; set; }
        public string PO_YR { get; set; }
        public string PO_NO { get; set; }
        public string IMMS_RLY_CD { get; set; }
        public string RLY_NONRLY { get; set; }
    }
}
