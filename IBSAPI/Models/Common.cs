using IBSAPI.DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace IBSAPI.Models
{
    public static class Common
    {
        public const string CommonDateFormate = "{0:MM/dd/yyyy}";
        public const string CommonDateFormateForJS = "DD-MM-YYYY";
        public const string CommonDateFormateForDT = "{0:dd/MM/yyyy}";
        public const string CommonDateFormate1 = "dd/MM/yyyy";

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
        public static List<ManufacturerModel> GetVendorDigit(int VendCd)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<ManufacturerModel> manufacturerModels = new List<ManufacturerModel>();
            manufacturerModels = (from v in ModelContext.T05Vendors
                        join c in ModelContext.T03Cities on v.VendCityCd equals (c.CityCd)
                        where v.VendCityCd == c.CityCd && v.VendName != null && v.VendCd == VendCd
                        select
                   new ManufacturerModel
                   {
                       ManufacturerEmail=v.VendEmail,
                       ManufacturerName=v.VendName,
                       PhoneNumber = v.VendContactTel1,
                       ContactPersonName = v.VendContactPer1,
                       PlaceofInspection = v.VendAdd1,
                       ManufacturerDropDownName = Convert.ToString(v.VendName) + "/" + Convert.ToString(v.VendAdd1) + "/" + Convert.ToString(c.Location) + "/" + c.City,
                       ManufacturerID = v.VendCd,
                   }).ToList();
            
            return manufacturerModels;
        }

        public static List<ManufacturerModel> GetVendorUsingTextAndValues(string VENDOR)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<ManufacturerModel> manufacturerModels = new List<ManufacturerModel>();
            manufacturerModels = (from v in ModelContext.T05Vendors
                        join c in ModelContext.T03Cities on v.VendCityCd equals (c.CityCd)
                        where v.VendCityCd == c.CityCd && v.VendName != null
                        && v.VendName.Trim().ToUpper().StartsWith(VENDOR.ToUpper())
                        orderby v.VendName
                        select
                   new ManufacturerModel
                   {
                       ManufacturerEmail = v.VendEmail,
                       ManufacturerName = v.VendName,
                       PhoneNumber = v.VendContactTel1,
                       ContactPersonName = v.VendContactPer1,
                       PlaceofInspection = v.VendAdd1,
                       ManufacturerDropDownName = Convert.ToString(v.VendName) + "/" + Convert.ToString(v.VendAdd1) + "/" + Convert.ToString(c.Location) + "/" + c.City,
                       ManufacturerID = v.VendCd,
                   }).ToList();
            return manufacturerModels;
        }

        public static string ConvertDateFormat(this DateTime dt)
        {
            return dt.ToString(Common.CommonDateFormate1);
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

    }
}
