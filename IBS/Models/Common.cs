using IBS.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Text;
using System.Reflection.Metadata;
using IBS.DataAccess;

namespace IBS.Models
{
    public static class Common
    {

        public const string CommonDateFormate = "{0:MM/dd/yyyy}";

        public static string GetFullAddress(string address1, string address2, string address3, string address4, string address5, string PostCode)
        {
            List<string> strArray = new List<string> { address1, address2, address3, address4, address5, PostCode };

            string fullAddress = string.Join(", ", strArray.Where(m => !string.IsNullOrEmpty(m)).ToList());

            return fullAddress;
        }

        public static string CreateRandomText(int Length)
        {
            string allowedChars = "1234567890";
            char[] chars = new char[Length];
            Random rd = new Random();

            for (int i = 0; i < Length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public static string AESDecrypt(this String strToDecrypt, string strKey)
        {
            try
            {
                return Crypt.AESDecrypt(strToDecrypt, strKey);
            }
            catch (Exception ex)
            {
                throw new Exception("Add more context here", ex);
            }
        }

        public static string Encrypt(this String strToEncrypt)
        {
            try
            {

                return Crypt.EncryptText(strToEncrypt, "TH#&^$HSJB$@#^GGHWF&)!&^@*(#$HJDY");
            }
            catch (Exception ex)
            {
                throw new Exception("Add more context here", ex);
            }
        }

        public static string Decrypt(this String strToDecrypt)
        {
            try
            {
                //strToDecrypt = strToDecrypt.Replace("%2F", "/");
                return Crypt.DecryptText(strToDecrypt, "TH#&^$HSJB$@#^GGHWF&)!&^@*(#$HJDY");
            }
            catch (Exception ex)
            {
                throw new Exception("Add more context here", ex);
            }
        }

        public static string Decrypt(this String strToDecrypt, bool IsQueryStringParameter = false)
        {
            try
            {
                return Crypt.DecryptText(IsQueryStringParameter ? strToDecrypt.Replace(" ", "+") : strToDecrypt, "TH#&^$HSJB$@#^GGHWF&)!&^@*(#$HJDY");
            }
            catch (Exception ex)
            {
                throw new Exception("Add more context here", ex);
            }
        }

        public static string EncryptQueryString(this String strToEncrypt)
        {
            if (!string.IsNullOrEmpty(strToEncrypt))
            {
                byte[] passBytes = Encoding.Unicode.GetBytes(strToEncrypt);
                string encryptPassword = Convert.ToBase64String(passBytes);
                return encryptPassword;
            }
            else
                return string.Empty;
        }

        public static string DecryptQueryString(this String strToDecrypt)
        {
            if (!string.IsNullOrEmpty(strToDecrypt))
            {
                byte[] passByteData = Convert.FromBase64String(strToDecrypt);
                string originalPassword = Encoding.Unicode.GetString(passByteData);
                return originalPassword;
            }
            else
                return string.Empty;
        }
        public static void AddException(string exception, string exceptionmsg, string ControllerName, string ActionName, int CreatedBy, string CreatedIP)
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            Tblexception objexception = new Tblexception();
            objexception.Controllername = ControllerName;
            objexception.Actionname = ActionName;
            objexception.Exceptionmessage = exception;
            objexception.Exception = exceptionmsg;
            objexception.Createdby = CreatedBy;
            objexception.Createip = CreatedIP;
            objexception.Createddate = DateTime.Now;
            context.Tblexceptions.Add(objexception);
            context.SaveChanges();
        }

        //public static IEnumerable<SelectListItem> GetRegionType()
        //{
        //    var obj = EnumUtility<List<SelectListItem>>.GetEnumList(typeof(Enums.RegionType)).ToList();
        //    obj.Insert(0, new SelectListItem { Text = "--Select--", Value = "" });
        //    return obj;
        //}

        public static List<SelectListItem> GetRegionType()
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            var obj = (from c in context.T01Regions
                       select new SelectListItem
                       {
                           Value = c.RegionCode.ToString(),
                           Text = c.Region
                       }).OrderBy(c => c.Text).ToList();
            obj.Insert(0, new SelectListItem { Text = "--Select--", Value = "" });
            return obj;
        }

        public static List<DropDownDTO> GetCity()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<DropDownDTO> city = (from a in context.T03Cities
                                      select
                                 new DropDownDTO
                                 {
                                     Text = a.City,
                                     Value = a.CityCd
                                 }).ToList();
            return city;
        }

        public static List<TextValueDropDownDTO> VendorApproval()
        {
            List<TextValueDropDownDTO> textValueDropDownDTO = new List<TextValueDropDownDTO>();
            TextValueDropDownDTO single = new TextValueDropDownDTO();
            single.Text = "RDSO Approved";
            single.Value = "R";
            textValueDropDownDTO.Add(single);
            single = new TextValueDropDownDTO();
            single.Text = "RE Approved";
            single.Value = "E";
            textValueDropDownDTO.Add(single);
            single = new TextValueDropDownDTO();
            single.Text = "Both RE & RDSO Approved";
            single.Value = "B";
            textValueDropDownDTO.Add(single);
            single = new TextValueDropDownDTO();
            single.Text = "Other";
            single.Value = "O";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static IEnumerable<DropDownDTO> GetYesNoCommon()
        {
            return EnumUtility<List<DropDownDTO>>.GetEnumDropDownIntValue(typeof(Enums.YesNoCommon)).ToList();
        }

        public static List<DropDownDTO> GetRole()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<DropDownDTO> Role = (from a in ModelContext.Roles
                                      select
                                 new DropDownDTO
                                 {
                                     Text = Convert.ToString(a.Rolename),
                                     Value = Convert.ToInt32(a.RoleId)
                                 }).ToList();
            return Role;

        }
    }
    public static class DbContextHelper
    {
        public static DbContextOptions<ModelContext> GetDbContextOptions()
        {
            IConfiguration Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            return new DbContextOptionsBuilder<ModelContext>()
                             .UseOracle(Configuration.GetConnectionString("DefaultConnection"))
                             .Options;
        }

        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string sortColumn, bool descending)
        {
            var parameter = Expression.Parameter(typeof(T), "p");

            string command = "OrderBy";

            if (descending)
            {
                command = "OrderByDescending";
            }

            Expression resultExpression = null;

            var property = typeof(T).GetProperty(sortColumn);
            // this is the part p.SortColumn
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);

            // this is the part p =&gt; p.SortColumn
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // finally, call the "OrderBy" / "OrderByDescending" method with the order by lamba expression
            resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { typeof(T), property.PropertyType },
               query.Expression, Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<T>(resultExpression);
        }
    }
}

