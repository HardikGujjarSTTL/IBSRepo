using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Reflection;

namespace IBSAPI.Helper
{
    public class Enums
    {
        public enum ResultFlag
        {
            [Description("Success message")]
            SucessMessage = 1,
            [Description("error")]
            ErrorMessage = 0,
            [Description("Invalid token")]
            TokenMessage = 2,
            [Description("Internal Exception")]
            InternamException = 4,
            [Description("Validation message")]
            ValidationMessage = 5
        }
        public enum Region
        {
            [Description("Northern Region")]
            N,
            [Description("Southern Region")]
            S,
            [Description("Eastern Region")]
            E,
            [Description("Westrern Region")]
            W,
            [Description("Central Region")]
            C,
            [Description("QA Corporate")]
            Q,
        }
        public enum DocumentCategory : int
        {
            ICPHOTOS = 22,
        }
        public enum DocumentCategory_CANRegisrtation : int
        {
            IC_Photos_Upload1 = 22,
            IC_Photos_Upload2 = 24,
            IC_Photos_Upload3 = 25,
            IC_Photos_Upload4 = 26,
            IC_Photos_Upload5 = 27,
            IC_Photos_Upload6 = 28,
            IC_Photos_Upload7 = 29,
            IC_Photos_Upload8 = 30,
            IC_Photos_Upload9 = 31,
            IC_Photos_Upload10 = 32,
            ICPhoto_Dig_Sign = 33,
        }

        public enum FolderPath
        {
            [Description("/ReadWriteData/Files/TempUploadedFiles")]
            TempFilePath = 1,
            [Description("/ReadWriteData/IC_PHOTOS")]
            ICPHOTOS = 30
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

        public static string GetDescriptionByKey(int key)
        {
            Enum e = (Enum)Enum.ToObject(typeof(T), key);
            return e.Description();
        }

        public static string GetDescriptionByKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
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
}
