using IBSAPI.DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System;
using System.Security.Cryptography;

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

        

        public static string CnSHA256(string text, int length)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2")); // convert to hex
                }

                string resultStr = builder.ToString();

                if (length > resultStr.Length)
                {
                    return resultStr;
                }
                else
                {
                    return resultStr.Substring(0, length);
                }
            }
        }

        public static string Encrypt(string plainText, string key, string iv)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(CnSHA256(key, key.Length));
            byte[] ivBytes = Encoding.UTF8.GetBytes(CnSHA256(iv, 16));

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static string Decrypt(string encryptedText, string key, string iv)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(CnSHA256(key, key.Length));
            byte[] ivBytes = Encoding.UTF8.GetBytes(CnSHA256(iv, 16));

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        public static string EncryptString(string plainText, string key, string iv)
        {
            string k = key;
            string i = iv;
            k = CnSHA256(k, 32); // 32 bytes = 256 bits
            i = CnSHA256(i, 16);
            return Encrypt(plainText, k, i);
        }

        public static string DecryptString(string encryptedText, string key, string iv)
        {
            string k = key;
            string i = iv;
            k = CnSHA256(k, 32); // 32 bytes = 256 bits
            i = CnSHA256(i, 16);
            return Decrypt(encryptedText, k, i);
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
