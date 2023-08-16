using Humanizer;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Reflection;

namespace IBS.Helper
{
    public class Enums
    {
        public enum FileUploaderMode : int
        {
            View = 1,
            Add_Edit = 2
        }

        public enum FilUploadMode
        {
            Single = 1,
            Multiple = 2,
        }

        public enum FolderPath
        {
            [Description("/Files/TempUploadedFiles")]
            TempFilePath = 1,
            [Description("/Files/UserRegistration")]
            UserRegistration = 2,
            [Description("/Files/AdminUserUploadDoc")]
            AdminUserUploadDoc = 3,
            [Description("/Files/Vendor")]
            Vendor = 4,
            [Description("/Files/VendorDocument")]
            VendorDocument = 5,
            [Description("/MA")]
            VendorMADocument = 6,
            [Description("/Files/ContractDocument")]
            ContractDocument = 7,
            [Description("/Files/TechnicalReferences")]
            TechnicalReferencesDoc = 8,
        }

        public enum DocumentCategory : int
        {
            UserRegi = 1,
            AdminUserUploadDoc = 2,
            Vendor = 3,
            VendorDocument = 4,
            VendorMADoc = 5,
            Contract = 6,
            TechnicalReferences = 8,
        }


        public enum DocumentCategory_CANRegisrtation : int
        {
            Address_Proof_Document = 5,
            Profile_Picture = 6,
            Document_Vendor_manufacturer_created = 23,
            Inernal_Records = 45,
            Firm_Certificate_Like_RDSO_Approval_Type_test_etc = 47,
            Raw_Material_Invoice = 48,
            Calibration_Records = 49,
            Contract_Documents_If_Any = 9,
            Upload_Tech_Ref = 11,
            Upload_Tech_Ref_Reply = 12,
        }

        public enum DocumentCategory_AdminUserUploadDoc : int
        {
            Browse_the_Document_to_Upload = 7,
        }
        public enum DocumentCategory_VendorMADoc : int
        {
            VendorMADoc = 8,
        }

        public static string GetEnumDescription(object enumValue)
        {
            string defDesc = string.Empty;
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return defDesc;
        }


        public enum RegionType
        {
            [Description("East Zone")]
            East_Zone = 1,
            [Description("West Zone")]
            West_Zone = 2,
            [Description("North Zone")]
            North_Zone = 3,
            [Description("South Zone")]
            South_Zone = 4,
        }

        public enum YesNoCommon
        {
            [Description("Yes")]
            Yes = 0,
            [Description("No")]
            No = 1,
        }
    }

    public class EnumUtility<T>
    {
        public static IEnumerable<SelectListItem> GetEnumList(Type EnumerationType)
        {
            List<SelectListItem> objDropDown = new();
            Dictionary<int, string> RetVal = new();
            var AllItems = Enum.GetValues(EnumerationType);

            foreach (var CurrentItem in AllItems)
            {
                DescriptionAttribute[] DescAttribute = (DescriptionAttribute[])EnumerationType.GetField(CurrentItem.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                string Description = DescAttribute.Length > 0 ? DescAttribute[0].Description : CurrentItem.ToString();
                RetVal.Add((int)CurrentItem, Description);
            }

            foreach (int Key in RetVal.Keys)
            {
                objDropDown.Add(new SelectListItem() { Value = Key.ToString(), Text = RetVal[Key] });
            }

            return objDropDown;
        }

        public static IEnumerable<DropDownDTO> GetEnumDropDownIntValue(Type EnumerationType)
        {
            List<DropDownDTO> objDropDown = new();
            Dictionary<int, string> RetVal = new();
            var AllItems = Enum.GetValues(EnumerationType);

            foreach (var CurrentItem in AllItems)
            {
                DescriptionAttribute[] DescAttribute = (DescriptionAttribute[])EnumerationType.GetField(CurrentItem.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                string Description = DescAttribute.Length > 0 ? DescAttribute[0].Description : CurrentItem.ToString();
                RetVal.Add((int)CurrentItem, Description);
            }

            foreach (int Key in RetVal.Keys)
            {
                objDropDown.Add(new DropDownDTO() { Value = Key, Text = RetVal[Key] });
            }

            return objDropDown;
        }
    }

    public static class EnumExtensions
    {
        public static string Description(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var field = enumType.GetField(enumValue.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length == 0
                ? enumValue.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }
    }

    public class DropDownDTO
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public string Group { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class TextValueDropDownDTO
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
