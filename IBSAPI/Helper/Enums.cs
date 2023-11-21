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
            InternamException = 4
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
