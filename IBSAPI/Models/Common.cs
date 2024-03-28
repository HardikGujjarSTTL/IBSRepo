using IBSAPI.DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System;
using System.Security.Cryptography;
using IBSAPI.Helper;
using static IBSAPI.Helper.Enums;

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

        public static byte[] Encrypt(string plaintext, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes;
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
                        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }
                return encryptedBytes;
            }
        }
        public static string Decrypt(byte[] ciphertext, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] decryptedBytes;
                using (var msDecrypt = new System.IO.MemoryStream(ciphertext))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var msPlain = new System.IO.MemoryStream())
                        {
                            csDecrypt.CopyTo(msPlain);
                            decryptedBytes = msPlain.ToArray();
                        }
                    }
                }
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }


        public static string getEncryptedText(string _dencryptedText, string UniqueId)
        {
            //string key = "GM2SO0DB2MD0TECV";
            string iv = "GTC2SRE0DAN2MIT0TNECIRNG";
            //String UniqueIdKey = UniqueId + key;
            String UniqueIdKey = UniqueId;

            String encUniqueIdKey = CryptLib.getHashSha256(UniqueIdKey, 32);
            //String encIv = CryptLib.getHashSha256(iv, 16);

            CryptLib _crypt = new CryptLib();

            return _crypt.encrypt(_dencryptedText, encUniqueIdKey, iv.Substring(0, 16));
        }

        public static string getDecryptedText(string _encryptedText, string UniqueId)
        {
            //string key = "GM2SO0DB2MD0TECV";
            string iv = "GTC2SRE0DAN2MIT0TNECIRNG";
            //String UniqueIdKey = UniqueId + key;
            String UniqueIdKey = UniqueId;

            String encUniqueIdKey = CryptLib.getHashSha256(UniqueIdKey, 32);
            //String encIv = CryptLib.getHashSha256(iv, 16);

            CryptLib _crypt = new CryptLib();

            return _crypt.decrypt(_encryptedText, encUniqueIdKey, iv.Substring(0, 16));
        }

        public static string getEncryptedText1(string _dencryptedText, string UniqueId)
        {
            string key = "GM2SO0DB2MD0TECV";
            string iv = "GTC2SRE0DAN2MIT0TNECIRNG";
            String UniqueIdKey = UniqueId + key;

            String encUniqueIdKey = CryptLib.getHashSha256(UniqueIdKey, 32);
            String encIv = CryptLib.getHashSha256(iv, 16);

            CryptLib _crypt = new CryptLib();

            return _crypt.encrypt(_dencryptedText, encUniqueIdKey, encIv);
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

        public static DateTime GetFinancialYearStartDate()
        {
            DateTime currentDate = DateTime.Today;
            DateTime financialYearStart;
            if (currentDate.Month >= 4)
            {
                financialYearStart = new DateTime(currentDate.Year, 4, 1);
            }
            else
            {
                financialYearStart = new DateTime(currentDate.Year - 1, 4, 1);
            }            
            return financialYearStart;            
        }

        public static DateTime GetFinancialYearEndDate()
        {
            DateTime financialYearStart = GetFinancialYearStartDate();
            DateTime financialYearEnd = financialYearStart.AddYears(1).AddDays(-1);
            return financialYearEnd;
        }

        public static List<TextValueDropDownDTO> GetUserTypeLogin()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.UserTypeLogin)).ToList();
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
