using IBS.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class MAapproveModel
    {
        public string CaseNo { get; set; } = null!;

        public string? PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        public DateTime? PoDt { get; set; }

        public string? RlyNonrly { get; set; }

        public string? RlyCd { get; set; }

        public string MaNo1 { get; set; } = null!;

        public string encryptMaNo { get; set; } = null!;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        public DateTime? MaDt { get; set; }

        public string? PoOrLetter { get; set; }

        public string? PoSrc { get; set; }

        public virtual ICollection<VendPoMaDetail> VendPoMaDetails { get; set; } = new List<VendPoMaDetail>();

        public byte MaSno { get; set; }
        public string? MaField { get; set; }
        public string? MaDesc { get; set; }
        public string? OldPoValue { get; set; }

        public string? NewPoValue { get; set; }
        public string? MADoc { get; set; }
        public string? MaStatus { get; set; }
        public string? MaDtc { get; set; }

        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedDatetime { get; set; }

        public string? MaRemarks { get; set; }
    }
}
