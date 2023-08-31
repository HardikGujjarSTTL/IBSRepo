using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class VendorModel
    {
        public int VendCd { get; set; }

        public string? VendName { get; set; }

        public string? VendAdd1 { get; set; }

        public string? VendAdd2 { get; set; }

        public int? VendCityCd { get; set; }

        public string? VendContactPer1 { get; set; }

        public string? VendContactTel1 { get; set; }

        public string? VendContactPer2 { get; set; }

        public string? VendContactTel2 { get; set; }

        public string? VendApproval { get; set; }

        [Display(Name = "Approval Period From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        //[RequiredIf(nameof(VendApproval), "E,R", ErrorMessage = "Approval Period From Required")]
        [RequiredIf("VendApproval", "E", "enter your age")]
        public DateTime? VendApprovalFr { get; set; }

        [Display(Name = "Approval Period To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? VendApprovalTo { get; set; }

        public string? VendStatus { get; set; }

        [Display(Name = "Status Date From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? VendStatusDtFr { get; set; }

        [Display(Name = "Status Date To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? VendStatusDtTo { get; set; }

        public string? VendStatusDtFrST { get; set; }

        public string? VendStatusDtToST { get; set; }

        public string? VendRemarks { get; set; }

        public string? VendCdAlpha { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string? VendEmail { get; set; }

        public string? VendInspStopped { get; set; }

        public string? VendPwd { get; set; }

        public string? OnlineCallStatus { get; set; }

    }


    public class RequiredIfAttribute : ValidationAttribute
    {
        private String PropertyName { get; set; }
        private String ErrorMessage { get; set; }
        private Object DesiredValue { get; set; }

        public RequiredIfAttribute(String propertyName, Object desiredvalue, String errormessage)
        {
            this.PropertyName = propertyName;
            this.DesiredValue = desiredvalue;
            this.ErrorMessage = errormessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (proprtyvalue.ToString() == DesiredValue.ToString() && value == null)
            //if (proprtyvalue.ToString() == DesiredValue.ToString() && value.ToString() == "N/A")
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-requirediftrue", errorMessage);
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }
    }

    public class VendorlistModel
    {
        public int VEND_CD { get; set; }

        public string? VEND_NAME { get; set; }

        public string? VEND_CITY_CD { get; set; }

        public string? VEND_ADD { get; set; }

        public string? VEND_CONT_NO { get; set; }

        public string? VEND_EMAIL { get; set; }
    }

}
