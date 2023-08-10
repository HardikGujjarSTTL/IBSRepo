using IBS.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class VendEquipClbrCertModel
    {
        public int VendCd { get; set; }
        [Required]
        public string DocType { get; set; } = null!;
        [Required]
        public byte? EquipClbrCertSno { get; set; }

        public string? EquipName { get; set; }
        [Required]
        public string EquipMkSl { get; set; } = null!;

        public string? EquipRange { get; set; }

        public string? CalibratedBy { get; set; }
        [Required]
        public string CalibCertNo { get; set; } = null!;

        public DateTime? DtOfCalib { get; set; }

        public DateTime? NextDtCalib { get; set; }

        public string? EquipDesc { get; set; }

        public string? NablAccDet { get; set; }

        public DateTime? Datetime { get; set; }

        public virtual T103VendDoc T103VendDoc { get; set; } = null!;

        public int? ID { get; set; }

    }

    public class VendEquipClbrCertListModel
    {
        public string VEND_CD { get; set; }
        public string DOC_TYPE { get; set; }
        public string EQUIP_MK_SL { get; set; }
        public string CALIB_CERT_NO { get; set; }
        public string EQUIP_CLBR_CERT_SNO { get; set; }
        public string EQUIP_NAME { get; set; }
        public string EQUIP_RANGE { get; set; }
        public string CALIBRATED_BY { get; set; }
        public DateTime? DT_OF_CALIB { get; set; }
        public DateTime? NEXT_DT_CALIB { get; set; }
        public string EQUIP_DESC { get; set; }
        public string NABL_ACC_DET { get; set; }
        public string V_DOC { get; set; }
    }
}
