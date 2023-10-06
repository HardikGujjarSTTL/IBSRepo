using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class RegionalHRDataOfIEModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "IE Name is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "IE Name is required")]
        public int IeCd { get; set; }
        public string? Disclipline { get; set; }

        public string IE_NAME { get; set; }

        [Required(ErrorMessage = "Joining Date is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? Joiningdate { get; set; }

        [Required(ErrorMessage = "Posting Date is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? Postingdate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? Retirementdate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? Transferdate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? Deputationfromdate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? Deputationtodate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? Repetriationdate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? Ietenurefromdate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? Ietenuretodate { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Updateddate { get; set; }

        public byte? Isdeleted { get; set; }
        public string EncryptedId { get; set; }
    }
}
