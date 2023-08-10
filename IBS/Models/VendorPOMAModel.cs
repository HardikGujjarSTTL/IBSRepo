using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class VendorPOMAModel
    {
        public string? CASE_NO { get; set; }

        public string PO_NO { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PO_DT { get; set; }

        public string MA_NO { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? MA_DT { get; set; }

        public string RLY_NONRLY { get; set; }

        public string RLY_CD { get; set; }

        public string PO_OR_LETTER { get; set; }

        public string MA_SNO { get; set; }

        public string MA_FIELD { get; set; }

        public string MA_STATUS { get; set; }

        public string S_RCE { get; set; }

        public string VEND_CD { get; set; }

        public string VEND_CD_S { get; set; }

        public string BPO_TYPE { get; set; }

        public string MA_DESC { get; set; }

        public string OLD_PO_VALUE { get; set; }

        public string NEW_PO_VALUE { get; set; }


    }
}
