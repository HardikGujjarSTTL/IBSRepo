using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class BillRegisterModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? FromDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ToDt { get; set; }

        [Required(ErrorMessage = "SAP Bill Status is required")]
        public string? BillStatus { get; set; }

        [Required(ErrorMessage = "Region is required")]
        public string? Region { get; set; }

        public string? BILL_NO { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? BILL_DT { get; set; }

        public string? BK_NO { get; set; }

        public int? SET_NO { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? IC_DT { get; set; }

        public decimal? BILL_AMOUNT { get; set; }

        public decimal? SERVICE_TAX { get; set; }

        public decimal? EDU_CESS { get; set; }

        public decimal? SHE_CESS { get; set; }

        public decimal? SWACHH_BHARAT_CESS { get; set; }

        public decimal? KRISHI_KALYAN_CESS { get; set; }

        public decimal? INSP_FEE { get; set; }

        public decimal? IGST { get; set; }

        public decimal? SGST { get; set; }

        public decimal? CGST { get; set; }

        public string INVOICE_NO { get; set; }

        public string RECIPIENT_GSTIN_NO { get; set; }

        public string BPO_TYPE { get; set; }

        public string IRN_NO { get; set; }

        public string? SENTTOSAP { get; set; }

        public string FINALISED { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[DataType(DataType.Date)]
        public string ACK_DT { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[DataType(DataType.Date)]
        public string DIG_BILL_GEN_DT { get; set; }

        public string BPO { get; set; }

        public decimal MATERIAL_VALUE { get; set; }

        public string STATE { get; set; }

        public string CASE_NO { get; set; }

        public string CheckValue { get; set; }

        public string QR_CODE { get; set; }

        public bool IsSentToSAP { get; set; }
    }
    
}
