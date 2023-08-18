using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class HologramAccountalModel
    {

        public string CASE_NO { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string IE_SNAME { get; set; }
        public string HG_NO_MATERIAL { get; set; }
        public string HG_NO_SAMPLE { get; set; }
        public string HG_NO_TEST { get; set; }
        public string HG_NO_IC { get; set; }
        
        public string HG_NO_OT { get; set; }

        public string CALL_DT { get; set; } //DateTime?
        public string CALL_SNO { get; set; }
        public string CONSIGNEE_CD { get; set; }
        public string IE_NAME { get; set; }
        public string IE_CD { get; set; }
        public string CALL_RECV_DT { get; set; }

        public string REC_NO { get; set; }

        public string HG_REGION { get; set; }

        [Required(ErrorMessage ="Hologram no. material from is required")]
        public string HG_NO_MATERIAL_FR { get; set; }

        [Required(ErrorMessage = "Hologram no. material to is required")]
        public string HG_NO_MATERIAL_TO { get; set; }

        [Required(ErrorMessage = "Hologram no. sample from is required")]
        public string HG_NO_SAMPLE_FR { get; set; }

        [Required(ErrorMessage = "Hologram no. sample to is required")]
        public string HG_NO_SAMPLE_TO { get; set; }

        [Required(ErrorMessage = "Hologram no. test from is required")]
        public string HG_NO_TEST_FR { get; set; }

        [Required(ErrorMessage = "Hologram no. test to is required")]
        public string HG_NO_TEST_TO { get; set; }
        public string HG_NO_IC_FR { get; set; }

        [Required(ErrorMessage = "Hologram no. ic from is required")]
        public string HG_NO_IC_TO { get; set; }

        [Required(ErrorMessage = "Hologram no. ic to is required")]
        public string HG_NO_IC_DOC { get; set; }
        public string HG_OT_DESC { get; set; }
        public string HG_NO_OT_FR { get; set; }
        public string HG_NO_OT_TO { get; set; }
        public int USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string DATETIME { get; set; }
    }



    public class HologramAccountalSearchModel
    {
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
    }

    public class HologramAccountalFilter
    {
        public string CASE_NO { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string IE_SNAME { get; set; }
        public string CALL_DT { get; set; }
        public string CALL_SNO { get; set; }
        public string CONSIGNEE_CD { get; set; }
        public string REGION { get; set; }
    }

    public class HologramAccountalAddModel
    {
        public string CASE_NO { get; set; }
        public string CALL_RECV_DT { get; set; }
        public string CONSIGNEE_CD { get; set; }
        public string CALL_SNO { get; set; }
        public string REC_NO { get; set; }
        public string HG_REGION { get; set; }
        public string HG_NO_MATERIAL_FR { get; set; }
        public string HG_NO_MATERIAL_TO { get; set; }
        public string HG_NO_SAMPLE_FR { get; set; }
        public string HG_NO_SAMPLE_TO { get; set; }
        public string HG_NO_TEST_FR { get; set; }
        public string HG_NO_TEST_TO { get; set; }
        public string HG_NO_IC_FR { get; set; }
        public string HG_NO_IC_TO { get; set; }
        public string HG_NO_IC_DOC { get; set; }
        public string HG_OT_DESC { get; set; }
        public string HG_NO_OT_FR { get; set; }
        public string HG_NO_OT_TO { get; set; }
        public string USER_ID { get; set; }
        public string DATETIME { get; set; }
    }
}
