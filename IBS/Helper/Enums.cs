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
            [Description("/MASTER_ITEMS_CHECKSHEETS")]
            MasterItemDoc = 9,
            [Description("/CALLS_DOCUMENTS")]
            CallRegistrationDoc = 10,
            [Description("/Online_Comp_Document")]
            OnlineComplaints = 11,
            [Description("/Consignee_Comp_Document")]
            ConsigneeComplaints = 12,
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
            MasterItemDoc = 9,
            CallRegistrationDoc = 10,
            OnlineComplaints=11,
            ConsigneeComplaints = 12,
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
            Upload_Rejection_Memo=50,
            Upload_JI_Case=51,
            Upload_JI_Report=52,
            Upload_Tech_Ref1 = 53,

        }

        public enum DocumentCategory_AdminUserUploadDoc : int
        {
            Browse_the_Document_to_Upload = 7,
            CallRegistrationDoc = 13,
        }
        public enum DocumentCategory_VendorMADoc : int
        {
            VendorMADoc = 8,
        }

        public enum DocumentCategory_MasterDoc : int
        {
            MasterItemDoc = 10,
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

        public enum COType
        {
            [Description("CM")]
            C,
            [Description("DFO")]
            D,
        }

        public enum COStatus
        {
            [Description("Working")]
            W,
            [Description("Retired")]
            R,
            [Description("Transferred")]
            T,
            [Description("Left/Repatriated")]
            L,
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

        public static IEnumerable<TextValueDropDownDTO> GetEnumDropDownStringValue(Type EnumerationType)
        {
            List<TextValueDropDownDTO> objDropDown = new();
            Dictionary<string, string> RetVal = new();
            var AllItems = Enum.GetValues(EnumerationType);

            foreach (var CurrentItem in AllItems)
            {
                DescriptionAttribute[] DescAttribute = (DescriptionAttribute[])EnumerationType.GetField(CurrentItem.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                string Description = DescAttribute.Length > 0 ? DescAttribute[0].Description : CurrentItem.ToString();
                RetVal.Add(CurrentItem.ToString(), Description);
            }
            foreach (string Key in RetVal.Keys)
            {
                objDropDown.Add(new TextValueDropDownDTO() { Value = Key, Text = RetVal[Key] });
            }

            return objDropDown;
        }

        public static string GetDescriptionByKey(int key)
        {
            Enum e = (Enum)Enum.ToObject(typeof(T), key);
            return e.Description();
        }

        public static string GetDescriptionByKey(string key)
        {
            if(string.IsNullOrEmpty(key)) return string.Empty;
            Enum e = (Enum)Enum.Parse(typeof(T), key);
            return e.Description();
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
