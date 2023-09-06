using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class VigilanceCasesMasterModel
    {
        public int Id { get; set; }

        public string? RefRegNo { get; set; }

        [Display(Name = "Vigilance Letter Ref Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [Required]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? RefDt { get; set; }

        public string? RefNo { get; set; }

        public string? RefDetails { get; set; }

        [Display(Name = "Ref Reply Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? RefReplyDt { get; set; }

        public string? PrelimInvDetails { get; set; }

        public string? ActionProposed { get; set; }

        [Display(Name = "Action Proposed Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? ActionProposedDt { get; set; }

        public string? FinalAction { get; set; }

        [Display(Name = "Final Action Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? FinalActionDt { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string? Region { get; set; }

        public List<VigilanceCasesListModel> lstVigilanceCasesList { get; set;}
    }

    public class VigilanceCasesListModel
    {
        public int Id { get; set; }

        public string CaseNo { get; set; }

        public string BkNo { get; set; }

        public string SetNo { get; set; }

        public int ConsigneeCd { get; set; }

        public string BpoCd { get; set; }

        public int IeCd { get; set; }

        public string BillNo { get; set; }

        public DateTime? BillDt { get; set; }
    }
}
