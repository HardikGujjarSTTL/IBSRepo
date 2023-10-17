using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class IC_Bookset_FormModel
    {
        public int Id { get; set; }

        [Display(Name = "Book No")]
        [Required]
        public string? BkNo { get; set; }

        [Display(Name = "Set No. From")]
        [Required]
        public string? SetNoFr { get; set; }

        [Display(Name = "Set No. From")]
        [SetNoGreaterThen("SetNoFr", ErrorMessage = "Set No. To Must Greater Than Set No. From")]
        [Required]
        public string? SetNoTo { get; set; }

        [Display(Name = "IE To Whom Issued")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [Required]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? IssueDt { get; set; }

        [Display(Name = "IE To Whom Issued")]
        [Required]
        public int? IssueToIecd { get; set; }

        public string? IeName { get; set; }

        public string? BkSubmitted { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? BkSubmitDt { get; set; }

        public string? Region { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        [Display(Name = "Cut Off Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? CutOffDt { get; set; }

        [CutOffSetBetween(ErrorMessage = "Cut Off Set No. Should Be Between Set No. From & Set No. To.")]
        public string? CutOffSet { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }

        public string? _SetNoFr { get; set; }

        public string? _SetNoTo { get; set; }

        [Display(Name = "IC Type")]
        [Required]
        public string? ICType { get; set; }

    }

    public class SetNoGreaterThenAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public SetNoGreaterThenAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = Convert.ToInt32(value);

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = Convert.ToInt32(property.GetValue(validationContext.ObjectInstance));

            if (currentValue < comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }

    public class CutOffSetBetweenAttribute : ValidationAttribute
    {
        int SetNoFr = 0;
        int SetNoTo = 0;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;

            if(value == null) return ValidationResult.Success;

            var currentValue = Convert.ToInt32(value);

            var model = (IC_Bookset_FormModel)validationContext.ObjectInstance;

            if (!string.IsNullOrEmpty(model.SetNoFr)) SetNoFr = Convert.ToInt32(model.SetNoFr);
            if (!string.IsNullOrEmpty(model.SetNoTo)) SetNoTo = Convert.ToInt32(model.SetNoTo);

            if (((currentValue >= SetNoFr) && (currentValue <= SetNoTo)) == false)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }

}