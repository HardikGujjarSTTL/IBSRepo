using IBS.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;
using IBS.DataAccess;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json;
using System.Globalization;
using IBS.Controllers;
using Humanizer.Localisation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Collections.Generic;
using static IBS.Helper.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Principal;

namespace IBS.Models
{
    public static class Common
    {
        public const string CommonDateFormate = "{0:MM/dd/yyyy}";
        public const string CommonDateFormateForJS = "DD-MM-YYYY";
        public const string CommonDateFormateForDT = "{0:dd/MM/yyyy}";
        public const string CommonDateFormate1 = "dd/MM/yyyy";
        public static string AccessDeniedMessage = "You don't have permission to do this action.";
        public const string RegularExpressionForDT = @"(?:(?:(?:0[1-9]|1\d|2[0-8])\/(?:0[1-9]|1[0-2])|(?:29|30)\/(?:0[13-9]|1[0-2])|31\/(?:0[13578]|1[02]))\/[1-9]\d{3}|29\/02(?:\/[1-9]\d(?:0[48]|[2468][048]|[13579][26])|(?:[2468][048]|[13579][26])00))";
        public const string CommonDateTimeFormat = "dd/MM/yyyy-HH:mm:ss";
        public static int RegenerateOtpButtonShowMinute = 10;

        public static Dictionary<string, int> ConnectedUsers = new Dictionary<string, int>();

        public static string SendOTP(string mobile, string message)
        {
            WebClient client = new WebClient();
            string baseurl = "http://apin.onex-aura.com/api/sms?key=QtPr681q&to=" + mobile + "&from=RITESI&body=" + message + "&entityid=1501628520000011823&templateid=1707168743061977502";
            //string baseurl = $"http://apin.onex-aura.com/api/sms?key=QtPr681q&to={mobile}&from=RITESI&body={message}&entityid=1501628520000011823&templateid=1707161588918541674";
            Stream data = client.OpenRead(baseurl);
            StreamReader smsreader = new StreamReader(data);
            string s = smsreader.ReadToEnd();
            data.Close();
            smsreader.Close();
            return s;
        }

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

        public static string getEncryptedText(string _dencryptedText, string UniqueId)
        {
            string key = "GM2SO0DB2MD0TECV";
            string iv = "GTC2SRE0DAN2MIT0TNECIRNG";
            String UniqueIdKey = UniqueId + key;

            String encUniqueIdKey = CryptLib.getHashSha256(UniqueIdKey, 32);
            String encIv = CryptLib.getHashSha256(iv, 16);

            CryptLib _crypt = new CryptLib();

            return _crypt.encrypt(_dencryptedText, encUniqueIdKey, encIv);
        }

        public static string getDecryptedText(string _encryptedText, string UniqueId)
        {
            string key = "GM2SO0DB2MD0TECV";
            string iv = "GTC2SRE0DAN2MIT0TNECIRNG";
            String UniqueIdKey = UniqueId + key;

            String encUniqueIdKey = CryptLib.getHashSha256(UniqueIdKey, 32);
            String encIv = CryptLib.getHashSha256(iv, 16);

            CryptLib _crypt = new CryptLib();

            return _crypt.decrypt(_encryptedText, encUniqueIdKey, encIv);
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

        public static List<SelectListItem> GetRegionType()
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            var obj = (from c in context.T01Regions
                       select new SelectListItem
                       {
                           Value = c.RegionCode.ToString(),
                           Text = c.Region
                           //}).OrderBy(c => c.Text).ToList();
                       }).ToList();
            obj.Insert(0, new SelectListItem { Text = "--Select--", Value = "" });
            return obj;
        }

        public static List<SelectListItem> GetCity()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> city = (from a in context.T03Cities
                                         select
                                    new SelectListItem
                                    {
                                        Text = a.City,
                                        Value = Convert.ToString(a.CityCd)
                                    }).ToList();
            return city;
        }

        public static List<SelectListItem> GetState()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> state = (from a in context.T92States
                                          select
                                     new SelectListItem
                                     {
                                         Text = a.StateName,
                                         Value = Convert.ToString(a.StateName)
                                     }).ToList();
            return state;
        }

        public static List<SelectListItem> LabInfoReportStatus()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "All";
            single.Value = "A";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Lab Report Uploaded";
            single.Value = "U";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Lab Report Not Uploaded";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetStatusLabRpt()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "All";
            single.Value = "A";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Confirm";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Not Confirm";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "No Comments";
            single.Value = "X";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static IEnumerable<SelectListItem> GetVendorLabRpt()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropList = (from v in ModelContext.T05Vendors
                                             join c in ModelContext.T03Cities on v.VendCityCd equals c.CityCd
                                             where v.VendName != null
                                             orderby (v.VendName.Trim() + "/" + v.VendAdd1.Trim() + "/" +
                     (c.Location != null ? (c.Location.Trim() + " / " + c.City) : c.City))
                                             select new SelectListItem
                                             {
                                                 Text = v.VendName.Trim() + "/" + v.VendAdd1.Trim() + "/" +
                            (c.Location != null ? (c.Location.Trim() + " / " + c.City) : c.City),
                                                 Value = Convert.ToString(v.VendCd)
                                             }).ToList();
            return dropList;
        }

        public static List<SelectListItem> GetCountry()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> country = (from a in context.CountryMasters
                                            select
                                       new SelectListItem
                                       {
                                           Text = a.CountryName,
                                           Value = Convert.ToString(a.CountryName)
                                       }).ToList();
            return country;
        }

        public static List<SelectListItem> GetDesignation()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> country = (from a in context.T08IeControllOfficers
                                            select
                                       new SelectListItem
                                       {
                                           Text = Convert.ToString(a.CoDesig),
                                           Value = Convert.ToString(a.CoDesig)
                                       }).ToList();
            return country;
        }

        public static List<SelectListItem> GetDefectDesc()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> DefectDesc = (from a in context.T38DefectCodes
                                               select
                                          new SelectListItem
                                          {
                                              Value = Convert.ToString(a.DefectCd),
                                              Text = Convert.ToString(a.DefectDesc)
                                          }).ToList();

            return DefectDesc;
        }

        public static List<SelectListItem> GetClassification()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> classification = (from a in context.T39JiStatusCodes
                                                   select
                                          new SelectListItem
                                          {
                                              Value = Convert.ToString(a.JiStatusCd),
                                              Text = Convert.ToString(a.JiStatusDesc)
                                          }).ToList();

            return classification;
        }

        public static List<SelectListItem> GetLabApproval()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "--Select--";
            single.Value = "0";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "NABL";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "RITES";
            single.Value = "R";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "CERTIFER APPROVED";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> StatusOffer()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Ongoing Contract";
            single.Value = "E";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "BID Lost";
            single.Value = "B";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> VendorApproval()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "RDSO Approved";
            single.Value = "R";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "RE Approved";
            single.Value = "E";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Both RE & RDSO Approved";
            single.Value = "B";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Other";
            single.Value = "O";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> InspRegion()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "Northern Region";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Eastern Region";
            single.Value = "E";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Western Region";
            single.Value = "W";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Southern Region";
            single.Value = "S";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<TextValueDropDownDTO> ChargesType()
        {
            //List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            //SelectListItem single = new SelectListItem();
            //single.Text = "Customer's Advance";
            //single.Value = "A";
            //textValueDropDownDTO.Add(single);
            //single = new SelectListItem();
            //single.Text = "Call Cancellation Charge";
            //single.Value = "C";
            //textValueDropDownDTO.Add(single);
            //single = new SelectListItem();
            //single.Text = "Rejection Charges";
            //single.Value = "R";
            //textValueDropDownDTO.Add(single);
            //single = new SelectListItem();
            //single.Text = "Lab Charges";
            //single.Value = "L";
            //textValueDropDownDTO.Add(single);
            //single = new SelectListItem();
            //single.Text = "Revalidation Of IC";
            //single.Value = "V";
            //textValueDropDownDTO.Add(single);
            //single = new SelectListItem();
            //single.Text = "Duplicate IC";
            //single.Value = "D";
            //textValueDropDownDTO.Add(single);
            //single = new SelectListItem();
            //single.Text = "NSIC Call Charges";
            //single.Value = "N";
            //textValueDropDownDTO.Add(single);
            //return textValueDropDownDTO.ToList();

            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.ChargesType)).ToList();
        }

        public static List<SelectListItem> BPORegion()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "Northern Region";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Eastern Region";
            single.Value = "E";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Western Region";
            single.Value = "W";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Southern Region";
            single.Value = "S";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Bhilai Region";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> FeedBackRegion()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "Northern Region";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Eastern Region";
            single.Value = "E";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Western Region";
            single.Value = "W";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Southern Region";
            single.Value = "S";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "CENTRAL REGION";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "CO QA DIVISION";
            single.Value = "Q";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> ParticularAction()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "No Action Required";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Warning Letter";
            single.Value = "W";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Minor Penalty";
            single.Value = "I";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Major Penalty";
            single.Value = "J";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Others";
            single.Value = "O";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> FinancialYear()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "08-09";
            single.Value = "08";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "09-10";
            single.Value = "09";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "10-11";
            single.Value = "10";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "11-12";
            single.Value = "11";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "12-13";
            single.Value = "12";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "13-14";
            single.Value = "13";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "14-15";
            single.Value = "14";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "15-16";
            single.Value = "15";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "16-17";
            single.Value = "16";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "17-18";
            single.Value = "17";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "18-19";
            single.Value = "18";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "19-20";
            single.Value = "19";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "20-21";
            single.Value = "20";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "21-22";
            single.Value = "21";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "22-23";
            single.Value = "22";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> NOJIReasons()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "DP Expired";
            single.Value = "A";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Validity of IC Expired";
            single.Value = "B";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Recieved in Damaged/Broken Condition";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Rejection &lt;5%";
            single.Value = "D";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Rejection issued after 90 Days of reciept of material";
            single.Value = "E";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Guarantee Claim";
            single.Value = "F";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Wrong Dispatch";
            single.Value = "G";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Material not stamped (partial/full)";
            single.Value = "H";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Material received in excess of ordered quantity";
            single.Value = "I";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Material lifted/rectified/replaced (Partially/Fully) before issue of Rejection Advice";
            single.Value = "J";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Reason(s) of rejection, beyond scope of inspection";
            single.Value = "K";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> Inspectionrequired()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "Yes";
            single.Value = "Y";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "No";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Cancelled";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> Checksheet()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "Approved";
            single.Value = "A";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Revision";
            single.Value = "R";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Prepration";
            single.Value = "P";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> DARPurpose()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "No Action Required";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Warning Letter";
            single.Value = "W";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Minor Penalty";
            single.Value = "I";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Major Penalty";
            single.Value = "J";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Counselling";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Others";
            single.Value = "O";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> ItemBlocked()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "No";
            single.Value = "";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Yes";
            single.Value = "Y";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<TextValueDropDownDTO> VendorStatus()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.VendorStatus)).ToList();
        }

        public static List<SelectListItem> GetClaimHead()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "--Select--";
            single.Value = "0";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "308-Conveyance/Fare Charges";
            single.Value = "308";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "309-Traveling Allowance";
            single.Value = "309";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "310-Hotel Charges";
            single.Value = "310";
            single = new SelectListItem();
            single.Text = "608-Telephone/Mobile/Internet Charges";
            single.Value = "608";
            single = new SelectListItem();
            single.Text = "629-Others";
            single.Value = "629";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<TextValueDropDownDTO> OnlineCallStatus()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.OnlineCallStatus)).ToList();
        }

        public static List<TextValueDropDownDTO> ClientType()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.ClientType)).ToList();
        }

        public static List<SelectListItem> GetAccountCode()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> AccountCD = (from a in context.Acccodes
                                              select
                                    new SelectListItem
                                    {
                                        Text = Convert.ToString(a.AccDesc),
                                        Value = Convert.ToString(a.AccCd)
                                    }).ToList();
            return AccountCD;
        }

        public static List<SelectListItem> LabRVGetAccountCode(string rolecd)
        {
            List<SelectListItem> AccountCD = null;
            if (rolecd == "5")
            {
                ModelContext context = new(DbContextHelper.GetDbContextOptions());
                AccountCD = (from a in context.T95AccountCodes
                             where (a.AccCd == 2210 || a.AccCd == 2212)
                             orderby a.AccDesc
                             select
                   new SelectListItem
                   {
                       Text = Convert.ToString(a.AccDesc),
                       Value = Convert.ToString(a.AccCd)
                   }).ToList();
            }
            else
            {
                ModelContext context = new(DbContextHelper.GetDbContextOptions());
                AccountCD = (from a in context.T95AccountCodes
                             where a.AccCd < 3000
                             orderby a.AccDesc
                             select
                   new SelectListItem
                   {
                       Text = Convert.ToString(a.AccDesc),
                       Value = Convert.ToString(a.AccCd)
                   }).ToList();
            }

            return AccountCD;
        }

        public static List<SelectListItem> CallStatus()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> call = (from a in context.T21CallStatusCodes
                                         select
                               new SelectListItem
                               {
                                   Text = Convert.ToString(a.CallStatusDesc),
                                   Value = Convert.ToString(a.CallStatusCd)
                               }).ToList();
            return call;
        }

        public static IEnumerable<SelectListItem> GetClientByClientType(string CoCd)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> List = new List<SelectListItem>();
            if (CoCd == "R")
            {
                List = (from a in ModelContext.T91Railways
                        where a.RlyCd != "CORE"
                        select new SelectListItem
                        {
                            Text = Convert.ToString(a.RlyCd),
                            Value = Convert.ToString(a.Railway)
                        }).OrderBy(c => c.Text).ToList();
            }
            else if (CoCd != "" && CoCd != null)
            {
                var distinctRecords = from bpo in ModelContext.T12BillPayingOfficers
                                      where bpo.BpoType == CoCd
                                      group bpo by new
                                      {
                                          RLY_CD = bpo.BpoRly.Trim().ToUpper(),
                                          ORGN = bpo.BpoOrgn
                                      } into grp
                                      select new
                                      {
                                          RLY_CD = grp.Key.RLY_CD,
                                          ORGN = grp.Key.ORGN
                                      } into distinctRec
                                      orderby distinctRec.ORGN
                                      select distinctRec;

                List = (from a in distinctRecords
                        select new SelectListItem
                        {
                            Text = Convert.ToString(a.RLY_CD),
                            Value = Convert.ToString(a.ORGN)
                        }).ToList();
            }
            return List;
        }

        public static List<SelectListItem> GetMonth()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();

            single = new SelectListItem();
            single.Text = "January";
            single.Value = "01";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "February";
            single.Value = "02";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "March";
            single.Value = "03";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "April";
            single.Value = "04";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "May";
            single.Value = "05";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "June";
            single.Value = "06";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "July";
            single.Value = "07";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "August";
            single.Value = "08";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "September";
            single.Value = "09";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "October";
            single.Value = "10";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "November";
            single.Value = "11";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "December";
            single.Value = "12";
            textValueDropDownDTO.Add(single);

            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetYear()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();

            single = new SelectListItem();
            single.Text = "2008";
            single.Value = "2008";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2009";
            single.Value = "2009";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2010";
            single.Value = "2010";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2011";
            single.Value = "2011";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2012";
            single.Value = "2012";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2013";
            single.Value = "2013";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2014";
            single.Value = "2014";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2015";
            single.Value = "2015";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2016";
            single.Value = "2016";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2017";
            single.Value = "2017";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2018";
            single.Value = "2018";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2019";
            single.Value = "2019";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2020";
            single.Value = "2020";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2021";
            single.Value = "2021";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2022";
            single.Value = "2022";
            textValueDropDownDTO.Add(single);

            single = new SelectListItem();
            single.Text = "2023";
            single.Value = "2023";
            textValueDropDownDTO.Add(single);

            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetRitesOfficerCd(string CO_REGION, string CO_TYPE)
        {
            if (CO_TYPE == "D")
            {
                ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
                return (from a in ModelContext.T08IeControllOfficers
                        where a.CoRegion == CO_REGION && a.CoType == CO_TYPE
                        select new SelectListItem
                        {
                            Text = Convert.ToString(a.CoName),
                            Value = Convert.ToString(a.CoCd)
                        }).OrderBy(c => c.Text).ToList();
            }
            else
            {
                ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
                return (from a in ModelContext.T08IeControllOfficers
                        where a.CoRegion == CO_REGION
                        select new SelectListItem
                        {
                            Text = Convert.ToString(a.CoName),
                            Value = Convert.ToString(a.CoCd)
                        }).OrderBy(c => c.Text).ToList();
            }
        }

        public static List<SelectListItem> GetInspEngCd(int IeCd = 0)
        {
            if (IeCd == 0)
            {
                ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
                return (from a in ModelContext.T09Ies
                        where (a.IeStatus == null)
                        select new SelectListItem
                        {
                            Text = Convert.ToString(a.IeName),
                            Value = Convert.ToString(a.IeCd)
                        }).OrderBy(c => c.Text).ToList();
            }
            else
            {
                ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
                return (from a in ModelContext.T09Ies
                        where (a.IeStatus == null) && (a.IeCd == IeCd)
                        select new SelectListItem
                        {
                            Text = Convert.ToString(a.IeName),
                            Value = Convert.ToString(a.IeCd)
                        }).OrderBy(c => c.Text).ToList();
            }
        }

        public static List<SelectListItem> GetInspEngCdFortech(string IeRegion)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            return (from a in ModelContext.T09Ies
                    where a.IeRegion == IeRegion
                    select new SelectListItem
                    {
                        Text = Convert.ToString(a.IeName),
                        Value = Convert.ToString(a.IeCd)
                    }).OrderBy(c => c.Text).ToList();
        }

        public static List<SelectListItem> GetInspEngCd(string IeRegion)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            return (from a in ModelContext.T09Ies
                    where a.IeStatus == null && a.IeRegion == IeRegion
                    select new SelectListItem
                    {
                        Text = Convert.ToString(a.IeName),
                        Value = Convert.ToString(a.IeCd)
                    }).OrderBy(c => c.Text).ToList();
        }

        public static List<SelectListItem> GetBPOByRegion(string BpoRegion)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            return (from t12 in ModelContext.T12BillPayingOfficers
                    join t03 in ModelContext.T03Cities on t12.BpoCityCd equals t03.CityCd
                    where t12.BpoRegion == BpoRegion
                    select new SelectListItem
                    {
                        Text = t12.BpoName + (t12.BpoAdd != null ? ("/" + t12.BpoAdd) : "") + (t03.Location != null ? ("/" + t03.City + "/" + t03.Location) : ("/" + t03.City)) + "/" + t12.BpoRly,
                        Value = Convert.ToString(t12.BpoCd)
                    }).OrderBy(c => c.Text).ToList();
        }

        public static List<SelectListItem> MAStatus()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Approved";
            single.Value = "A";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Approved With No Changes in IBS";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> MAApproveStatus()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Approved with No Change In IBS";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Return With Remarks";
            single.Value = "R";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Approved";
            single.Value = "A";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> ClientWise()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Railways";
            single.Value = "R";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Private";
            single.Value = "P";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "PSU";
            single.Value = "U";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "State Govt";
            single.Value = "S";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Foreign Railways";
            single.Value = "F";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }
        public static List<SelectListItem> ClientWiseBPO()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Railways";
            single.Value = "R";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Private";
            single.Value = "P";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "PSU";
            single.Value = "U";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "State Govt";
            single.Value = "S";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> ReportStatus()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "All";
            single.Value = "A";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }
        public static List<SelectListItem> TypeOutStandingBills()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "All Bills";
            single.Value = "A";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Full Outstanding Bills";
            single.Value = "F";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Partly Outstanding Bills";
            single.Value = "P";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> SapBillStatus()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Pending";
            single.Value = "P";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Sent to SAP";
            single.Value = "S";
            textValueDropDownDTO.Add(single);
            //single = new SelectListItem();
            //single.Text = "Not-Sent to SAP";
            //single.Value = "Y";
            //textValueDropDownDTO.Add(single);
            //single = new SelectListItem();
            //single.Text = "Sent to SAP";
            //single.Value = "S";
            //textValueDropDownDTO.Add(single);
            //single = new SelectListItem();
            //single.Text = "Received Response from SAP";
            //single.Value = "R";
            //textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Lab Report Uploaded";
            single.Value = "U";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Lab Report Not Uploaded";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> RailwaysTypes()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();

            //single = new SelectListItem();
            //single.Text = "-xx-";
            //single.Value = " ";

            //textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Railways";
            single.Value = "R";

            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Private";
            single.Value = "P";

            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "PSU";
            single.Value = "U";

            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "State Govt.";
            single.Value = "S";

            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Foreign Railways";
            single.Value = "F";

            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> PoOrLetterTypes()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Purchase Order";
            single.Value = "P";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Letter of Offer";
            single.Value = "L";
            textValueDropDownDTO.Add(single);

            return textValueDropDownDTO.ToList();
        }

        public static List<TextValueDropDownDTO> RegionCode()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.RegionCode)).ToList();
        }

        public static List<TextValueDropDownDTO> StockNonstock()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.StockNonstock)).ToList();
        }

        public static List<TextValueDropDownDTO> PoOrLetter()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.PoOrLetter)).ToList();
        }

        public static List<SelectListItem> CallRStatus()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "1st  Re-Mark";
            single.Value = "1";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "2nd  Re-Mark";
            single.Value = "2";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "3rd  Re-Mark";
            single.Value = "3";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "4th  Re-Mark";
            single.Value = "4";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "5th  Re-Mark";
            single.Value = "5";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "6th  Re-Mark";
            single.Value = "6";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "7th  Re-Mark";
            single.Value = "7";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "8th  Re-Mark";
            single.Value = "8";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "9th  Re-Mark";
            single.Value = "9";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> Departmentlist()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Mechanical";
            single.Value = "M";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Electrical";
            single.Value = "E";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Civil";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Textiles";
            single.Value = "T";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "M & P";
            single.Value = "Z";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> IEDepartmentlist()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Mechanical";
            single.Value = "M";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Electrical";
            single.Value = "E";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Civil";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Metallurgy";
            single.Value = "L";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Textiles";
            single.Value = "T";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Power Engineering";
            single.Value = "P";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> CommonYesNo()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Yes";
            single.Value = "Y";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "No";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<TextValueDropDownDTO> DiscountType()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.DiscountType)).ToList();
        }

        public static List<TextValueDropDownDTO> ExciseType()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.ExciseType)).ToList();
        }

        public static bool CallCancelCheck(VenderCallCancellationModel model, int chk)
        {
            if (model.chk1 == chk)
                return true;
            else if (model.chk2 == chk)
                return true;
            else if (model.chk3 == chk)
                return true;
            else if (model.chk4 == chk)
                return true;
            else if (model.chk5 == chk)
                return true;
            else if (model.chk6 == chk)
                return true;
            else if (model.chk7 == chk)
                return true;
            else if (model.chk8 == chk)
                return true;
            else if (model.chk9 == chk)
                return true;
            else if (model.chk10 == chk)
                return true;
            else if (model.chk11 == chk)
                return true;
            else if (model.chk12 == chk)
                return true;

            return false;
        }

        public static IEnumerable<DropDownDTO> GetYesNoCommon()
        {
            return EnumUtility<List<DropDownDTO>>.GetEnumDropDownIntValue(typeof(Enums.YesNoCommon)).ToList();
        }

        public static List<SelectListItem> GetRole()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> Role = (from a in ModelContext.Roles
                                         select
                                    new SelectListItem
                                    {
                                        Text = Convert.ToString(a.Rolename),
                                        Value = Convert.ToString(a.RoleId)
                                    }).ToList();
            return Role;

        }

        public static List<SelectListItem> IEDesignation()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> Desig = (from a in ModelContext.T07RitesDesigs
                                          select
                                     new SelectListItem
                                     {
                                         Text = Convert.ToString(a.RDesignation),
                                         Value = Convert.ToString(a.RDesigCd)
                                     }).ToList();
            return Desig;

        }

        public static List<SelectListItem> BPORailway()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> RlyCd = (from a in ModelContext.T91Railways
                                          select
                                     new SelectListItem
                                     {
                                         Text = Convert.ToString(a.Railway),
                                         Value = Convert.ToString(a.RlyCd)
                                     }).ToList();
            return RlyCd;

        }

        public static List<SelectListItem> GetAUCris()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> RlyCd = (from a in ModelContext.AuCris
                                          select
                                     new SelectListItem
                                     {
                                         Text = a.Au + "-" + a.Audesc + "/" + a.Address,
                                         Value = Convert.ToString(a.Au)
                                     }).ToList();
            return RlyCd;

        }

        public static List<SelectListItem> GetAUCrisByRlyCd(string RlyCd)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> lstRly = (from a in ModelContext.AuCris
                                           where a.RlyCd == RlyCd
                                           select
                                      new SelectListItem
                                      {
                                          Text = a.Au + "-" + a.Audesc + "/" + a.Address,
                                          Value = Convert.ToString(a.Au)
                                      }).ToList();
            return lstRly;

        }

        public static List<SelectListItem> GetComplaint()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> complaint = (from a in ModelContext.T08IeControllOfficers
                                              select
                                         new SelectListItem
                                         {
                                             Text = Convert.ToString(a.CoName),
                                             Value = Convert.ToString(a.CoCd)
                                         }).ToList();
            return complaint;

        }

        public static List<SelectListItem> GetIEName(string RegionCode)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> IE = (from a in ModelContext.T09Ies
                                       orderby a.IeName
                                       where a.IeRegion == RegionCode
                                       select
                                  new SelectListItem
                                  {
                                      Text = Convert.ToString(a.IeName),
                                      Value = Convert.ToString(a.IeCd)
                                  }).ToList();
            return IE;

        }

        public static List<SelectListItem> CourseName(string RegionCode, string TrainingType, string TrainingArea)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> IE = (from a in ModelContext.TrainingCourseMasters
                                       where a.TrainingType == TrainingType && a.TrainingField == TrainingArea && a.Region == RegionCode
                                       orderby a.CourseName
                                       select
                                  new SelectListItem
                                  {
                                      Text = Convert.ToString(a.CourseName + "(" + Convert.ToDateTime(a.CourseDurFr).ToString("dd/MM/yyyy") + Convert.ToDateTime(a.CourseDurTo).ToString("dd/MM/yyyy") + ")"),
                                      Value = Convert.ToString(a.CourseId)
                                  }).Distinct().ToList();
            return IE;

        }
        public static List<SelectListItem> Discipline()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> Disc = (from a in ModelContext.DisciplineMasters

                                         select
                                    new SelectListItem
                                    {
                                        Text = a.DisciplineName,
                                        Value = Convert.ToString(a.DiscId)
                                    }).ToList();
            return Disc;

        }

        public static List<SelectListItem> GetItem(string CaseNo, string CallRecDt, string CallSNO)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> IE = (from a in ModelContext.T18CallDetails
                                       where a.CaseNo == CaseNo && a.CallRecvDt == Convert.ToDateTime(CallRecDt) && a.CallSno == Convert.ToSingle(CallSNO)
                                       select
                                  new SelectListItem
                                  {
                                      Text = Convert.ToString(a.ItemDescPo),
                                      Value = Convert.ToString(a.ItemSrnoPo)
                                  }).ToList();
            return IE;
        }

        public static List<SelectListItem> GetIENameWithoutCode()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> IE = (from a in ModelContext.T09Ies
                                       select
                                  new SelectListItem
                                  {
                                      Text = Convert.ToString(a.IeName),
                                      Value = Convert.ToString(a.IeCd)
                                  }).ToList();
            return IE;

        }

        public static List<SelectListItem> GetTestCat()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> Role = (from a in ModelContext.T64TestCategories
                                         select
                                    new SelectListItem
                                    {
                                        Text = Convert.ToString(a.TestCategoryDesc),
                                        Value = Convert.ToString(a.TestCategoryCd)
                                    }).ToList();
            return Role;

        }

        public static List<SelectListItem> GetClientName(string Type, string SearchValue)
        {
            ModelContext modelContext = new ModelContext(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> distinctUserNames = modelContext.T12BillPayingOfficers
                    .OrderBy(t => t.BpoOrgn)
                    .Where(t => t.BpoType.Trim() == Type && t.BpoOrgn.ToLower().Contains(SearchValue.ToLower()))
                    .Select(t => new SelectListItem { Value = t.BpoRly.Trim() + " = " + t.BpoOrgn.Trim(), Text = t.BpoRly.Trim() + " = " + t.BpoOrgn.Trim() })
                    .Distinct()
                    .ToList();
            return distinctUserNames.OrderBy(x => x.Text).ToList();
        }

        public static List<SelectListItem> GetNCCode(string NCRClass)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> NCCODE = (from a in ModelContext.T69NcCodes
                                           where a.NcClass == NCRClass
                                           select
                                          new SelectListItem
                                          {
                                              Text = a.NcCd + " - " + a.NcDesc,
                                              Value = a.NcCd
                                          }).ToList();
            return NCCODE;
        }

        public static List<SelectListItem> GetLab()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> Role = (from a in ModelContext.T65LaboratoryMasters
                                         join city in ModelContext.T03Cities on a.LabCity equals city.CityCd
                                         orderby a.LabName
                                         select
                                    new SelectListItem
                                    {
                                        Text = a.LabName + "," + city.City,
                                        Value = Convert.ToString(a.LabId)
                                    }).ToList();
            return Role;

        }

        public static List<SelectListItem> GetControllingOfficer(string Region)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> CoName = (from a in ModelContext.T08IeControllOfficers
                                           join c in ModelContext.T07RitesDesigs on a.CoDesig equals c.RDesigCd
                                           where a.CoRegion == Region
                                           orderby a.CoName
                                           select
                                      new SelectListItem
                                      {
                                          Text = c.RDesignation != null ? a.CoName + "/" + c.RDesignation : a.CoName,
                                          Value = Convert.ToString(a.CoCd)
                                      }).ToList();
            return CoName;

        }

        public static List<SelectListItem> TestToBe()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Manual";
            single.Value = "M";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Default";
            single.Value = "D";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }
        public static List<SelectListItem> TypeofGST()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "--Select GST--";
            single.Value = "0";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "IN-STATE";
            single.Value = "IN-STATE";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "OUT-STATE";
            single.Value = "OUT-STATE";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> TestToBeConducted()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Chemical Testing By Spectro";
            single.Value = "CS";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Chemical Testing By Wet";
            single.Value = "CW";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "UTS, YS & % Elongation";
            single.Value = "UT";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Hardness Test";
            single.Value = "HT";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "EPDM/PVC Confirmation";
            single.Value = "EC";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Radiograpgy Test";
            single.Value = "RT";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Others";
            single.Value = "O";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> PaymentStatus()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "";
            single.Value = "";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Rejected With Remarks";
            single.Value = "R";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Approved";
            single.Value = "A";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetStatus()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Confirm";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Not Confirm";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "No Comments";
            single.Value = "X";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }
        public static List<SelectListItem> GetBpoType()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Railways";
            single.Value = "R";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Non-Railways";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }
        public static List<SelectListItem> AccCD()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "All Account Heads (Fees,Advance & Testing Charges)";
            single.Value = "A";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Advance (2709)";
            single.Value = "2709";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Testing Charges (2210,2212)";
            single.Value = "2210";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Other then Advance & Testing Charges";
            single.Value = "O";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> Orgn_Type()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Railway";
            single.Value = "R";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "State Government";
            single.Value = "S";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Other";
            single.Value = "U";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> Non_Orgn_Type()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Private";
            single.Value = "P";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "PSU";
            single.Value = "U";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "State Government";
            single.Value = "S";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Foreing Railways";
            single.Value = "F";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Central Government";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> LabSmapleStatus()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Sample Recieved";
            single.Value = "S";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Lab Report under Compilation";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Lab Report Uploaded";
            single.Value = "U";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Others";
            single.Value = "O";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> Category()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Regular";
            single.Value = "R";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Deputation";
            single.Value = "D";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Other";
            single.Value = "O";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> Qualification()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Degree";
            single.Value = "D";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Diploma";
            single.Value = "P";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Other";
            single.Value = "O";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> TrainingType()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Induction";
            single.Value = "I";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Orientation";
            single.Value = "O";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Refresh";
            single.Value = "R";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Technical";
            single.Value = "T";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> RejectionType()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Local (10,000/- Per Manday)";
            single.Value = "L";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Out Station (15,000/- Per Manday)";
            single.Value = "O";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> TrainingArea()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "Welding",
                Value = "W"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "NDT",
                Value = "N"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "Metrology",
                Value = "M"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "Plastic",
                Value = "P"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "Textile",
                Value = "T"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "Paint",
                Value = "A"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "Transformer",
                Value = "R"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "Cable",
                Value = "C"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "Energy Audit",
                Value = "E"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "Pressure Vessel",
                Value = "V"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "Pipeline",
                Value = "I"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "Rubber",
                Value = "B"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "M&P",
                Value = "X"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "ISO",
                Value = "O"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "DRG Reading",
                Value = "D"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "FR Item",
                Value = "F"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "OHE",
                Value = "H"
            });

            textValueDropDownDTO.Add(new SelectListItem
            {
                Text = "Other",
                Value = "Y"
            });

            return textValueDropDownDTO;
        }

        public static List<SelectListItem> CallCancelStatus()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Call Chargeable";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Call Non-Chargeable";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetRegion(string Region)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> RegionCode = (from a in ModelContext.T01Regions
                                               where a.RegionCode == Region
                                               select
                                  new SelectListItem
                                  {
                                      Text = a.Region,
                                      Value = a.RegionCode
                                  }).ToList();
            return RegionCode;

        }

        public static List<SelectListItem> GetRailPrices()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> lst = (from a in ModelContext.T34RailPrices
                                        orderby a.RailDesc ascending
                                        select
                                  new SelectListItem
                                  {
                                      Text = Convert.ToString(a.RailDesc),
                                      Value = Convert.ToString(a.RailId)
                                  }).ToList();
            return lst;
        }

        public static List<SelectListItem> GetUOM()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> lst = (from a in ModelContext.T04Uoms
                                        select
                                  new SelectListItem
                                  {
                                      Text = Convert.ToString(a.UomLDesc),
                                      Value = Convert.ToString(a.UomCd)
                                  }).ToList();
            return lst;
        }

        public static List<SelectListItem> GetConsignee(string CaseNo)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> lst = (from a in ModelContext.ViewConsigneeDetails
                                        where a.CaseNo == CaseNo
                                        select
                                  new SelectListItem
                                  {
                                      Text = Convert.ToString(a.ConsigneeName),
                                      Value = Convert.ToString(a.ConsigneeCd)
                                  }).ToList();
            return lst;
        }

        public static List<SelectListItem> GetAgencyClient(string RlyNonrly)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropDownDTOs = new List<SelectListItem>();

            if (RlyNonrly == "R")
            {
                List<SelectListItem> dropList = new List<SelectListItem>();
                dropList = (from a in ModelContext.T91Railways
                            where a.RlyCd != "CORE"
                            select
                       new SelectListItem
                       {
                           Text = Convert.ToString(a.Railway),
                           Value = Convert.ToString(a.RlyCd)
                       }).OrderBy(x => x.Value).ToList();
                dropDownDTOs.AddRange(dropList);
                SelectListItem drop = new SelectListItem();
                drop.Text = "Other";
                drop.Value = "0";
                dropDownDTOs.Add(drop);
            }
            else if (RlyNonrly != "" && RlyNonrly != null)
            {
                var query = ModelContext.T12BillPayingOfficers
                        .Where(bpo => bpo.BpoType == RlyNonrly)
                        .GroupBy(bpo => new
                        {
                            RLY_CD = bpo.BpoRly.ToUpper().Trim(),
                            RAILWAY = bpo.BpoOrgn
                        })
                        .Select(group => new
                        {
                            RLY_CD = group.Key.RLY_CD,
                            RAILWAY = group.Key.RAILWAY
                        })
                        .OrderBy(result => result.RLY_CD)
                        .ToList();

                List<SelectListItem> dropList = new List<SelectListItem>();
                dropList = (from a in query
                            where a.RLY_CD != null
                            select
                       new SelectListItem
                       {
                           Text = Convert.ToString(a.RAILWAY),
                           Value = Convert.ToString(a.RLY_CD)
                       }).OrderBy(x => x.Value).ToList();
                dropDownDTOs.AddRange(dropList);
                SelectListItem drop = new SelectListItem();
                drop.Text = "Other";
                drop.Value = "0";
                dropDownDTOs.Add(drop);
            }
            return dropDownDTOs.DistinctBy(x => x.Value).ToList();
            //return dropDownDTOs.ToList();
        }


        public static string GetClient(string RlyNonrly, string rly)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            string ClintName = "";

            if (RlyNonrly == "R")
            {
                ClintName = (from a in ModelContext.T91Railways
                             where a.RlyCd == rly
                             select a.Railway).FirstOrDefault();
            }
            else if (RlyNonrly != "" && RlyNonrly != null)
            {
                var objRailway = ModelContext.T12BillPayingOfficers
                                .Where(bpo => bpo.BpoRly == rly)
                                .GroupBy(bpo => new
                                {
                                    RAILWAY = bpo.BpoOrgn
                                })
                                .Select(group => new
                                {
                                    group.Key.RAILWAY
                                })
                                .FirstOrDefault();
                ClintName = objRailway.RAILWAY;
            }
            return ClintName;
        }

        public static List<SelectListItem> GetAgencyClientForDEOCris()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropList = new List<SelectListItem>();
            dropList = (from a in ModelContext.T91Railways
                        where a.RlyCd != "CORE"
                        select
                   new SelectListItem
                   {
                       Text = Convert.ToString(a.Railway),
                       Value = Convert.ToString(a.RlyCd)
                   }).OrderBy(x => x.Value).ToList();

            return dropList;
        }

        public static List<SelectListItem> GetRlyCd(string RlyNonrly)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropDownDTOs = new List<SelectListItem>();
            if (RlyNonrly == "R")
            {
                List<SelectListItem> dropList = new List<SelectListItem>();
                dropList = (from a in ModelContext.T91Railways
                            where a.RlyCd != "CORE"
                            select
                       new SelectListItem
                       {
                           Text = Convert.ToString(a.Railway),
                           Value = Convert.ToString(a.RlyCd)
                       }).ToList();
                dropDownDTOs.AddRange(dropList);
            }
            else if (RlyNonrly != "" && RlyNonrly != null)
            {
                List<SelectListItem> dropList = new List<SelectListItem>();
                dropList = (from a in ModelContext.T12BillPayingOfficers
                            where a.BpoType == Convert.ToString(RlyNonrly)
                            select
                       new SelectListItem
                       {
                           Text = Convert.ToString(a.BpoOrgn),
                           Value = Convert.ToString(a.BpoRly)
                       }).OrderBy(x => x.Text).ToList();
                dropDownDTOs.AddRange(dropList);
            }
            return dropDownDTOs.DistinctBy(x => x.Text).ToList();
        }

        public static List<SelectListItem> GetBPORlyCd(string ClientType)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropDownDTOs = new List<SelectListItem>();
            if (ClientType == "R")
            {
                List<SelectListItem> dropList = new List<SelectListItem>();
                dropList = (from a in ModelContext.T91Railways
                            select
                       new SelectListItem
                       {
                           Text = Convert.ToString(a.RlyCd),
                           Value = Convert.ToString(a.RlyCd)
                       }).ToList();
                dropDownDTOs.AddRange(dropList);
            }
            else if (ClientType != "" && ClientType != null)
            {
                List<SelectListItem> dropList = new List<SelectListItem>();
                dropList = (from a in ModelContext.T12BillPayingOfficers
                            where a.BpoType == Convert.ToString(ClientType)
                            select
                       new SelectListItem
                       {
                           Text = Convert.ToString(a.BpoRly),
                           Value = Convert.ToString(a.BpoRly)
                       }).OrderBy(x => x.Text).ToList();
                dropDownDTOs.AddRange(dropList);
            }
            return dropDownDTOs.DistinctBy(x => x.Text).ToList();
        }

        public static List<SelectListItem> Getfill_consignee_purcher(string RlyNonrlyValue, string RlyNonrlyText, string RlyCd)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropDownDTOs = new List<SelectListItem>();

            if (RlyNonrlyText == "Railways")
            {
                List<SelectListItem> dropList = new List<SelectListItem>();
                dropList = (from a in ModelContext.T06Consignees
                            join b in ModelContext.T03Cities on a.ConsigneeCity equals b.CityCd
                            where a.ConsigneeFirm.Trim().ToUpper().StartsWith(RlyCd.Trim().ToUpper())
                            orderby
                                (a.ConsigneeDesig.Trim() + "/" ?? "") +
                                (a.ConsigneeDept.Trim() + "/" ?? "") +
                                (a.ConsigneeFirm.Trim() + "/" ?? "") +
                                (a.ConsigneeAdd1.Trim() + "/" ?? "") +
                                (b.Location.Trim() + " : " + b.City.Trim() ?? b.City.Trim())
                            select new SelectListItem
                            {
                                Text = Convert.ToString(a.ConsigneeCd + "-" +
                                        a.ConsigneeFirm +
                                        (a.ConsigneeDesig != null ? "/" + a.ConsigneeDesig : "") +
                                        (a.ConsigneeDept != null ? "/" + a.ConsigneeDept : "") +
                                        (a.ConsigneeAdd1 != null ? "/" + a.ConsigneeAdd1 : "") +
                                        (b.Location != null ? " : " + b.Location + " : " + b.City : b.City)),
                                Value = Convert.ToString(a.ConsigneeCd)
                            }).ToList();

                dropDownDTOs.AddRange(dropList);
                SelectListItem drop = new SelectListItem();
                drop.Text = "Other";
                drop.Value = "0";
                dropDownDTOs.Add(drop);
            }
            else if (RlyNonrlyText != "")
            {
                List<SelectListItem> dropList = new List<SelectListItem>();
                dropList = (from a in ModelContext.T06Consignees
                            join b in ModelContext.T03Cities on a.ConsigneeCity equals b.CityCd
                            where a.ConsigneeType == Convert.ToString(RlyNonrlyValue)
                            orderby
                                (a.ConsigneeDesig.Trim() + "/" ?? "") +
                                (a.ConsigneeDept.Trim() + "/" ?? "") +
                                (a.ConsigneeFirm.Trim() + "/" ?? "") +
                                (a.ConsigneeAdd1.Trim() + "/" ?? "") +
                                (b.Location.Trim() + " : " + b.City.Trim() ?? b.City.Trim())
                            select new SelectListItem
                            {
                                Text = Convert.ToString(a.ConsigneeCd + "-" +
                                        a.ConsigneeFirm +
                                        (a.ConsigneeDesig != null ? "/" + a.ConsigneeDesig : "") +
                                        (a.ConsigneeDept != null ? "/" + a.ConsigneeDept : "") +
                                        (a.ConsigneeAdd1 != null ? "/" + a.ConsigneeAdd1 : "") +
                                        (b.Location != null ? " : " + b.Location + " : " + b.City : b.City)),
                                Value = Convert.ToString(a.ConsigneeCd)
                            }).ToList();
                dropDownDTOs.AddRange(dropList);
                SelectListItem drop = new SelectListItem();
                drop.Text = "Other";
                drop.Value = "0";
                dropDownDTOs.Add(drop);
            }
            return dropDownDTOs;
        }

        public static List<SelectListItem> GetPurchaserCdusingConsigneeCd(int ConsigneeCd)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> dropList = new List<SelectListItem>();
            dropList = (from a in ModelContext.T06Consignees
                        join b in ModelContext.T03Cities on a.ConsigneeCity equals b.CityCd
                        where a.ConsigneeCd == ConsigneeCd
                        orderby a.ConsigneeFirm, a.ConsigneeDesig, a.ConsigneeDept, b.City
                        select
                   new SelectListItem
                   {
                       Text = Convert.ToString(a.ConsigneeCd + "-" + a.ConsigneeFirm + "/" + a.ConsigneeDesig + "/" + a.ConsigneeDept + "/" + b.City),
                       Value = Convert.ToString(a.ConsigneeCd)
                   }).ToList();
            return dropList;
        }

        public static List<SelectListItem> GetPurchaserCd(string? consignee)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropDownDTOs = new List<SelectListItem>();
            List<SelectListItem> dropList = new List<SelectListItem>();
            dropList = (from a in ModelContext.V06Consignees
                        where a.Consignee.Trim().ToUpper().StartsWith(consignee.Trim().ToUpper()) ||
                        a.ConsigneeCd.ToString() == consignee.ToString()
                        orderby a.Consignee
                        select
                   new SelectListItem
                   {
                       Text = Convert.ToString(a.ConsigneeCd + "-" + a.Consignee),
                       Value = Convert.ToString(a.ConsigneeCd)
                   }).ToList();
            if (dropList.Count > 0)
            {
                dropDownDTOs.AddRange(dropList);
            }
            SelectListItem drop = new SelectListItem();
            drop.Text = "Other";
            drop.Value = "0";
            dropDownDTOs.Add(drop);
            //if (dropDownDTOs != null && dropDownDTOs.Count > 1)
            //{
            //    dropDownDTOs[1].Selected = true;
            //}
            return dropDownDTOs;
        }

        public static List<SelectListItem> GetVendor(int VendCd)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropDownDTOs = new List<SelectListItem>();

            List<SelectListItem> dropList = new List<SelectListItem>();
            dropList = (from a in ModelContext.ViewGetvendors
                        where a.VendCd == VendCd && a.VendName != null
                        select
                   new SelectListItem
                   {
                       Text = Convert.ToString(a.VendName),
                       Value = Convert.ToString(a.VendCd),
                   }).ToList();
            if (dropList.Count > 0)
            {
                dropDownDTOs.AddRange(dropList);
            }
            return dropDownDTOs;
        }

        public static List<SelectListItem> GetVendor_City(int VendCd)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropDownDTOs = new List<SelectListItem>();

            List<SelectListItem> dropList = new List<SelectListItem>();
            dropList = (from v in ModelContext.T05Vendors
                        join c in ModelContext.T03Cities on v.VendCityCd equals (c.CityCd)
                        where v.VendCityCd == c.CityCd && v.VendName != null && v.VendCd == VendCd
                        select
                   new SelectListItem
                   {
                       Text = Convert.ToString(v.VendName) + "/" + Convert.ToString(v.VendAdd1) + "/" + Convert.ToString(c.Location) + "/" + c.City,
                       Value = Convert.ToString(v.VendCd),
                   }).ToList();
            if (dropList.Count > 0)
            {
                dropDownDTOs.AddRange(dropList);
            }
            return dropDownDTOs;
        }

        public static List<SelectListItem> GetVendorUsingText(string VENDOR)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropDownDTOs = new List<SelectListItem>();

            List<SelectListItem> dropList = new List<SelectListItem>();
            dropList = (from v in ModelContext.T05Vendors
                        join c in ModelContext.T03Cities on v.VendCityCd equals (c.CityCd)
                        where v.VendCityCd == c.CityCd && v.VendName != null
                        && v.VendName.Trim().ToUpper().StartsWith(VENDOR.ToUpper().Substring(0, 5))
                        orderby v.VendName
                        select
                   new SelectListItem
                   {
                       Text = Convert.ToString(v.VendName) + "/" + Convert.ToString(v.VendAdd1) + "/" + Convert.ToString(c.Location) + "/" + c.City,
                       Value = Convert.ToString(v.VendCd),
                   }).ToList();
            if (dropList.Count > 0)
            {
                dropDownDTOs.AddRange(dropList);
            }
            return dropDownDTOs;
        }

        public static List<SelectListItem> GetVendorUsingTextAndValues(string VENDOR)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropDownDTOs = new List<SelectListItem>();

            List<SelectListItem> dropList = new List<SelectListItem>();
            if (VENDOR != null)
            {
                dropList = (from v in ModelContext.T05Vendors
                            join c in ModelContext.T03Cities on v.VendCityCd equals (c.CityCd)
                            where v.VendCityCd == c.CityCd && v.VendName != null
                            && v.VendName.Trim().ToUpper().StartsWith(VENDOR.ToUpper())
                            orderby v.VendName
                            select
                       new SelectListItem
                       {
                           Text = Convert.ToString(v.VendName) + "/" + Convert.ToString(v.VendAdd1) + "/" + Convert.ToString(c.Location) + "/" + c.City,
                           Value = Convert.ToString(v.VendCd),
                       }).ToList();
            }
            if (dropList.Count > 0)
            {
                dropDownDTOs.AddRange(dropList);
            }
            return dropDownDTOs;
        }

        public static int? GetVEND_CD(string? IMMS_VENDOR_CD)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            int? VendCd = (from v in ModelContext.ImmsRitesPoHdrs
                           where v.ImmsVendorCd == IMMS_VENDOR_CD && v.VendCd != null
                           select v.VendCd).FirstOrDefault();
            return VendCd;
        }

        public static int? GetVEND_CDusingRLY_CD(string RLY_CD, string IMMS_PURCHASER_CD)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            int? PurchaserCd = (from v in ModelContext.ImmsRitesPoHdrs
                                where v.ImmsPurchaserCd == IMMS_PURCHASER_CD && v.ImmsRlyCd == RLY_CD && v.PurchaserCd != null
                                select v.PurchaserCd).FirstOrDefault();
            return PurchaserCd;
        }
        public static string? GetBPO_CDusingRLY_CD(string RLY_CD, string IMMS_BPO_CD)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            string? BpoCd = (from v in ModelContext.ImmsRitesPoHdrs
                             where v.ImmsBpoCd == IMMS_BPO_CD && v.ImmsRlyCd == RLY_CD && v.BpoCd != null
                             select v.BpoCd).FirstOrDefault();
            return BpoCd;
        }

        public static List<SelectListItem> GetIEData(string GetRegionCode)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> contacts = (from v in ModelContext.T09Ies
                                             where v.IeStatus == "" && v.IeRegion == GetRegionCode
                                             orderby v.IeName
                                             select
                                     new SelectListItem
                                     {
                                         Text = v.IeName,
                                         Value = Convert.ToString(v.IeCd)
                                     }).ToList();
            return contacts;
        }

        public static List<SelectListItem> GetCluster(string GetRegionCode)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> contacts = (from t99 in ModelContext.T99ClusterMasters
                                             join t101 in ModelContext.T101IeClusters on t99.ClusterCode equals t101.ClusterCode
                                             join t09 in ModelContext.T09Ies on t101.IeCode equals t09.IeCd
                                             where t99.RegionCode == GetRegionCode && t09.IeStatus == null
                                             orderby t99.ClusterName, t09.IeName

                                             select new SelectListItem
                                             {
                                                 Text = t99.ClusterName + " (" + t09.IeName + ")",
                                                 Value = Convert.ToString(t99.ClusterCode)

                                             }).ToList();

            return contacts;
        }

        public static List<SelectListItem> GetClusterByIE(string GetRegionCode, string Dept)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> contacts = (from t99 in ModelContext.T99ClusterMasters
                                             where t99.RegionCode == GetRegionCode && t99.DepartmentName == Dept
                                             orderby t99.ClusterName

                                             select new SelectListItem
                                             {
                                                 Text = t99.ClusterName + "-" + t99.GeographicalPartition,
                                                 Value = Convert.ToString(t99.ClusterCode)

                                             }).ToList();

            return contacts;
        }

        public static List<SelectListItem> GetItemSuperForm(string CaseNo, string CallDate, string CallSNo)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> Super = (from t18 in context.T18CallDetails
                                          join t17 in context.T17CallRegisters
                                          on new { t18.CaseNo, t18.CallRecvDt, t18.CallSno }
                                          equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                                          where t17.CaseNo == CaseNo &&
                                          t17.CallRecvDt == DateTime.ParseExact(CallDate, "dd/MM/yyyy", null) &&
                                          t17.CallSno == int.Parse(CallSNo)
                                          select
                                     new SelectListItem
                                     {
                                         Text = t18.ItemDescPo,
                                         Value = Convert.ToString(t18.ItemSrnoPo)
                                     }).ToList();
            return Super;
        }

        public static List<SelectListItem> ItemScope()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "(IAF 12) Chemical/Paints";
            single.Value = "A";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "(IAF 14b) Plastics Pipes & Fittings";
            single.Value = "B";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "(IAF 16) Cement Pipes, AC Pressue Pipes & PCC Poles";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "(IAF 17b)  Rails, CI/DI Pipes, Steel Tubes and Fittings, Seamless Blocl/Galvanised, Valves";
            single.Value = "D";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "S(IAF 19a) Conductor, Cables, Power Transformers, CT/PT Fans, Relay, Panel, DG set, Alternator, Energy Meter";
            single.Value = "E";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "(IAF 22) Railway Rolling Stock";
            single.Value = "F";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "(IAF 28) Water Supply";
            single.Value = "G";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "(IAF 28) Construction";
            single.Value = "H";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "(IAF 07) Paper for Printing";
            single.Value = "I";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "(IAF 09) Printed Tickes & Ruled Papers";
            single.Value = "J";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Others";
            single.Value = "O";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetICType()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> IC = (from t93 in ModelContext.T93IcTypes
                                       orderby t93.IcType

                                       select new SelectListItem
                                       {
                                           Text = t93.IcType,
                                           Value = Convert.ToString(t93.IcTypeId)

                                       }).ToList();

            return IC;
        }

        public static List<SelectListItem> GetSealing()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> Sealing = (from t68 in ModelContext.T68SealingPatternCodes
                                            select new SelectListItem
                                            {
                                                Text = t68.SealingPatternDesc,
                                                Value = Convert.ToString(t68.SealingPatternCd)

                                            }).ToList();

            return Sealing;
        }

        public static List<SelectListItem> GetBPOList(string BPOCd)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> Sealing = new();
            if (BPOCd == "")
            {
                Sealing = (from t12 in ModelContext.T12BillPayingOfficers
                           join t03 in ModelContext.T03Cities on t12.BpoCityCd equals t03.CityCd
                           select new SelectListItem
                           {
                               Text = t12.BpoCd + '-' + t12.BpoName + (t12.BpoAdd != null ? ("/" + t12.BpoAdd) : "") + (t03.Location != null ? ("/" + t03.City + "/" + t03.Location) : ("/" + t03.City)) + "/" + t12.BpoRly,
                               Value = Convert.ToString(t12.BpoCd)

                           }).ToList();
            }
            else
            {
                if (BPOCd != null)
                {
                    Sealing = (from t12 in ModelContext.T12BillPayingOfficers
                               join t03 in ModelContext.T03Cities on t12.BpoCityCd equals t03.CityCd
                               where (t12.BpoCd.Trim().ToUpper() == BPOCd.ToUpper() || t12.BpoName.Trim().ToUpper().StartsWith(BPOCd.ToUpper()))
                               select new SelectListItem
                               {
                                   Text = t12.BpoCd + '-' + t12.BpoName + (t12.BpoAdd != null ? ("/" + t12.BpoAdd) : "") + (t03.Location != null ? ("/" + t03.City + "/" + t03.Location) : ("/" + t03.City)) + "/" + t12.BpoRly,
                                   Value = Convert.ToString(t12.BpoCd)

                               }).ToList();
                }
            }


            return Sealing;
        }

        public static List<SelectListItem> GetBPOList()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> BPOList = new();

            BPOList = (from t12 in ModelContext.T12BillPayingOfficers
                       join t03 in ModelContext.T03Cities on t12.BpoCityCd equals t03.CityCd
                       where t12.BpoType == "R" && (t12.BpoRly.Trim().ToUpper() == "IRFC")
                       orderby t12.BpoName
                       select new SelectListItem
                       {
                           Text = t12.BpoCd + '-' + t12.BpoName + (t12.BpoAdd != null ? ("/" + t12.BpoAdd) : "") + (t03.Location != null ? ("/" + t03.City + "/" + t03.Location) : ("/" + t03.City)) + "/" + t12.BpoRly,
                           Value = Convert.ToString(t12.BpoCd)

                       }).ToList();

            return BPOList;
        }

        public static List<SelectListItem> GetBPOListUsingBpoRly(string BpoRly)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> BPOList = new();

            BPOList = (from t12 in ModelContext.T12BillPayingOfficers
                       join t03 in ModelContext.T03Cities on t12.BpoCityCd equals t03.CityCd
                       where t12.BpoType == "R" && (t12.BpoRly.Trim().ToUpper() == BpoRly.Trim().ToUpper())
                       orderby t12.BpoName
                       select new SelectListItem
                       {
                           Text = t12.BpoCd + '-' + t12.BpoName + (t12.BpoAdd != null ? ("/" + t12.BpoAdd) : "") + (t03.Location != null ? ("/" + t03.City + "/" + t03.Location) : ("/" + t03.City)) + "/" + t12.BpoRly,
                           Value = Convert.ToString(t12.BpoCd)

                       }).ToList();

            return BPOList;
        }
        public static List<SelectListItem> GetBPOLabReport(string BpoType)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> BPOList = new();
            var query = from b in ModelContext.T12BillPayingOfficers
                        join c in ModelContext.T03Cities on b.BpoCityCd equals c.CityCd
                        join t92 in ModelContext.T92States on c.StateCd equals t92.StateCd
                        select new
                        {
                            BPO = b,
                            City = c,
                            State = t92
                        };

            var filteredQuery = query.AsEnumerable()
                .Where(ti0 => ti0.BPO.BpoCd.Trim().ToUpper() == BpoType.ToUpper() ||
                              (ti0.BPO.BpoName.Trim().ToUpper().StartsWith(BpoType.ToUpper()) && BpoType != ""))
                .OrderBy(ti0 => (ti0.BPO.BpoName + '/' + (ti0.BPO.BpoAdd != null ? ti0.BPO.BpoAdd + '/' : "") +
                                 (ti0.City.Location != null ? ti0.City.City + '/' + ti0.City.Location : ti0.City.City) + '/' + ti0.BPO.BpoRly))
                .Select(ti0 => new SelectListItem
                {
                    Text = $"{ti0.BPO.BpoCd}-{ti0.BPO.BpoName}/" +
                           $"{(ti0.BPO.BpoAdd != null ? ti0.BPO.BpoAdd + '/' : "")}" +
                           $"{(ti0.City.Location != null ? ti0.City.City + '/' + ti0.City.Location : ti0.City.City)}",
                    Value = Convert.ToString(ti0.BPO.BpoCd)
                });

            return filteredQuery.ToList();

            //var query = from b in ModelContext.T12BillPayingOfficers
            //           join c in ModelContext.T03Cities on b.BpoCityCd equals c.CityCd
            //           join t92 in ModelContext.T92States on c.StateCd equals t92.StateCd
            //           //where b.BpoCd.Trim().ToUpper() == BpoType.ToUpper() || OracleFunctions.Upper(b.BpoName).Trim().StartsWith(BpoType.ToUpper())
            //           where b.BpoCd.Trim().ToUpperInvariant() == BpoType.ToUpperInvariant() || b.BpoName.Trim().ToUpperInvariant().StartsWith(BpoType.ToUpperInvariant())
            //           orderby (b.BpoName + '/' + (b.BpoAdd != null ? b.BpoAdd + '/' : "") +
            //          (c.Location != null ? c.City + '/' + c.Location : c.City) + '/' + b.BpoRly)
            //           select new SelectListItem
            //           {
            //               Text = $"{b.BpoCd}-{b.BpoName}/" +
            //                   $"{(b.BpoAdd != null ? b.BpoAdd + '/' : "")}" +
            //                   $"{(c.Location != null ? c.City + '/' + c.Location : c.City)}",
            //               Value = Convert.ToString(b.BpoCd)

            //           };

            //return query.ToList();
        }

        public static List<SelectListItem> GetlstBPOType(string ClientType, string ClientName)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> Sealing = new();

            Sealing = (from t12 in ModelContext.T12BillPayingOfficers
                       join t03 in ModelContext.T03Cities on t12.BpoCityCd equals t03.CityCd
                       where t12.BpoType == ClientType && t12.BpoRly == ClientName
                       select new SelectListItem
                       {
                           Text = t12.BpoCd + '-' + t12.BpoName + (t12.BpoAdd != null ? ("/" + t12.BpoAdd) : "") + (t03.Location != null ? ("/" + t03.City + "/" + t03.Location) : ("/" + t03.City)) + "/" + t12.BpoRly,
                           Value = Convert.ToString(t12.BpoCd)

                       }).ToList();



            return Sealing;
        }

        public static List<SelectListItem> GetBPORLY(string BpoType)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> BpoRly = new();


            BpoRly = (from t12 in ModelContext.T12BillPayingOfficers
                      where t12.BpoType == BpoType
                      //orderby t12.BpoRly ascending
                      select new SelectListItem
                      {
                          Text = Convert.ToString(t12.BpoRly).Trim(),
                          Value = Convert.ToString(t12.BpoRly).Trim()
                      }).Distinct().OrderBy(x => x.Text).ToList();
            return BpoRly;
        }
        public static List<SelectListItem> GetBPO(string BpoType)
        {
            string searchText = BpoType.Trim().ToUpper();
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> BpoRly = new();

            BpoRly = (from bpo in ModelContext.T12BillPayingOfficers
                      where bpo.BpoCd.Trim().ToUpper() == searchText ||
                  bpo.BpoName.Trim().ToUpper().StartsWith(searchText)
                      orderby bpo.BpoName ascending
                      select new SelectListItem
                      {
                          Value = bpo.BpoCd,
                          //Text = bpo.BpoName + "/" + bpo.BpoAdd + "/" + bpo.BpoRly
                          Text = bpo.BpoCd + '-' + bpo.BpoName + (bpo.BpoAdd != null ? ("/" + bpo.BpoAdd) : "") + "/" + bpo.BpoRly
                      }).Distinct().ToList();
            return BpoRly;
        }

        public static List<SelectListItem> GetCOData(string GetRegionCode)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> contacts = (from v in ModelContext.T08IeControllOfficers
                                             where v.CoStatus == "" && v.CoRegion == GetRegionCode
                                             orderby v.CoName
                                             select
                                     new SelectListItem
                                     {
                                         Text = v.CoName,
                                         Value = Convert.ToString(v.CoCd)
                                     }).ToList();
            return contacts;
        }

        public static List<SelectListItem> GetCODataSuperForm(string GetRegionCode)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> contacts = (from v in ModelContext.T08IeControllOfficers
                                             where v.CoRegion == GetRegionCode
                                             orderby v.CoName
                                             select
                                     new SelectListItem
                                     {
                                         Text = v.CoName,
                                         Value = Convert.ToString(v.CoCd)
                                     }).ToList();
            return contacts;
        }

        public static VendorModel Getvendor_status(int VendCd)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            VendorModel model = (from m in ModelContext.T05Vendors
                                 where m.VendCd == VendCd
                                 select new VendorModel
                                 {
                                     VendCd = m.VendCd,
                                     VendName = m.VendName,
                                     VendAdd1 = m.VendAdd1,
                                     VendAdd2 = m.VendAdd2,
                                     VendCityCd = m.VendCityCd,
                                     VendApproval = m.VendApproval,
                                     VendApprovalFr = m.VendApprovalFr,
                                     VendApprovalTo = m.VendApprovalTo,
                                     VendContactPer1 = m.VendContactPer1,
                                     VendContactTel1 = m.VendContactTel1,
                                     VendContactPer2 = m.VendContactPer2,
                                     VendContactTel2 = m.VendContactTel2,
                                     VendEmail = m.VendEmail,
                                     VendRemarks = m.VendRemarks,
                                     VendStatus = m.VendStatus,
                                     VendStatusDtFr = m.VendStatusDtFr,
                                     VendStatusDtTo = m.VendStatusDtTo
                                 }).FirstOrDefault();

            return model;

        }

        public static VendorModel GetManufVEND(int VendCd)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            VendorModel model = (from m in ModelContext.ViewGetmanufvends
                                 where m.VendCd == VendCd
                                 select new VendorModel
                                 {
                                     VendCd = m.VendCd,
                                     VendName = m.VendName,
                                     VendAdd1 = m.VendAdd1,
                                     VendContactPer1 = m.VendContactPer1,
                                     VendContactTel1 = m.VendContactTel1,
                                     VendStatus = m.VendStatus,
                                     VendStatusDtFrST = m.VendStatusFr,
                                     VendStatusDtToST = m.VendStatusTo,
                                     VendEmail = m.VendEmail
                                 }).FirstOrDefault();

            return model;
        }

        public static List<SelectListItem> GetDocType()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> DocType = (from a in ModelContext.T74DocumentTypes
                                            select
                                       new SelectListItem
                                       {
                                           Text = Convert.ToString(a.DocTypeDesc),
                                           Value = Convert.ToString(a.DocType)
                                       }).ToList();
            return DocType;
        }

        public static List<SelectListItem> GetDocSubType(string DocType)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> DocSubType = (from a in ModelContext.T75DocSubTypes
                                               where a.DocType == DocType
                                               select
                                          new SelectListItem
                                          {
                                              Text = a.DocSubTypeDesc,
                                              Value = a.DocSubType
                                          }).ToList();
            return DocSubType;
        }

        public static List<SelectListItem> labRVGetBank()
        {

            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> Bank = (from a in context.T94Banks
                                         where a.BankCd < 990
                                         orderby a.BankName
                                         select
                                    new SelectListItem
                                    {
                                        Text = a.BankName,
                                        Value = Convert.ToString(a.BankCd)
                                        //Selected = (a.BankCd == Convert.ToInt32(regin))
                                    }).ToList();
            return Bank;
        }

        public static List<SelectListItem> GetBank()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> Bank = (from a in context.T94Banks
                                         orderby a.BankName
                                         select
                                    new SelectListItem
                                    {
                                        Text = a.BankName,
                                        Value = Convert.ToString(a.BankCd)
                                    }).ToList();
            return Bank;
        }

        public static List<SelectListItem> GetBankPayment()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> Bank = (from a in context.T94Banks
                                         where a.FmisBankCd != null
                                         orderby a.BankName
                                         select
                                    new SelectListItem
                                    {
                                        Text = a.BankName,
                                        Value = Convert.ToString(a.BankCd)
                                    }).ToList();
            return Bank;
        }

        public static List<SelectListItem> GetBankCode()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> BankCD = (from a in context.Bankcodedrpdowns
                                           select
                                 new SelectListItem
                                 {
                                     Text = Convert.ToString(a.BankName),
                                     Value = Convert.ToString(a.BankCd)
                                 }).ToList();
            return BankCD;
        }

        public static List<SelectListItem> GetControllingOfficer()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> ControllingOfficers = (from a in context.T08IeControllOfficers
                                                        where a.CoStatus == null && a.CoRegion == "N"
                                                        orderby a.CoName
                                                        select
                                 new SelectListItem
                                 {
                                     Text = Convert.ToString(a.CoName),
                                     Value = Convert.ToString(a.CoCd)
                                 }).ToList();
            return ControllingOfficers;
        }

        public static List<SelectListItem> GetIEname()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> IE_NAME = new List<SelectListItem>();
            IE_NAME.Add(new SelectListItem { Value = "-1", Text = "ALL" });
            IE_NAME = (from a in context.T09Ies
                       where a.IeStatus == null && a.IeRegion == "N"
                       orderby a.IeName
                       select
             new SelectListItem
             {
                 Text = Convert.ToString(a.IeName),
                 Value = Convert.ToString(a.IeCd)
             }).ToList();
            return IE_NAME;
        }

        public static List<SelectListItem> GetListBPO()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> GetBPO = (from a in context.T12BillPayingOfficers
                                           select
                                 new SelectListItem
                                 {
                                     Text = Convert.ToString(a.BpoName),
                                     Value = Convert.ToString(a.BpoCd)
                                 }).ToList();
            return GetBPO;
        }

        public static List<SelectListItem> GetBankLst()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> GetBankLst = (from a in context.T94Banks
                                               where a.BankCd > 990
                                               orderby a.BankName
                                               select
                                     new SelectListItem
                                     {
                                         Text = Convert.ToString(a.BankName),
                                         Value = Convert.ToString(a.BankCd)
                                     }).ToList();
            return GetBankLst;
        }

        public static List<SelectListItem> GetAccountLst()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> AccountLst = (from a in context.T95AccountCodes
                                               where a.AccCd > 3000
                                               orderby a.AccDesc
                                               select
                                    new SelectListItem
                                    {
                                        Text = Convert.ToString(a.AccDesc),
                                        Value = Convert.ToString(a.AccCd)
                                    }).ToList();
            return AccountLst;
        }

        public static List<SelectListItem> GetAccountCodeLst()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> AccountLst = (from a in context.T95AccountCodes
                                               where a.AccCd < 3000
                                               orderby a.AccDesc
                                               select
                                    new SelectListItem
                                    {
                                        Text = Convert.ToString(a.AccDesc) + ":" + Convert.ToString(a.AccCd),
                                        Value = Convert.ToString(a.AccCd)
                                    }).ToList();
            return AccountLst;
        }

        public static List<SelectListItem> GetBillPayingOfficer(string RlyCd, string RlyNonrly)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_BPO_TYPE", OracleDbType.Varchar2, RlyNonrly, ParameterDirection.Input);
            par[1] = new OracleParameter("p_client", OracleDbType.Varchar2, RlyCd, ParameterDirection.Input);
            par[2] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("GET_BPOS_BY_TYPE_RLY", par, 1);

            List<PO_MasterDetailsModel> model = new List<PO_MasterDetailsModel>();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model = JsonConvert.DeserializeObject<List<PO_MasterDetailsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }

            List<SelectListItem> obj = (from a in model.ToList()
                                        select
                                   new SelectListItem
                                   {
                                       Text = a.BPO_NAME,
                                       Value = a.BPO_CD
                                   }).ToList();
            return obj;
        }

        public static List<SelectListItem> GetBillPayingOfficerUsingSBPO(string SBPO)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> objdata = new List<SelectListItem>();
            if (SBPO != null && SBPO != "")
            {
                List<SelectListItem> model = new();
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_searchTerm", OracleDbType.Varchar2, SBPO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("GetBPOData", par, 1);
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model = JsonConvert.DeserializeObject<List<SelectListItem>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }

                //var obj = (from of in context.V12BillPayingOfficers
                //             where SqlMethods.Like(of.Bpo.ToUpper().Trim(), SBPO + "%") ||
                //                    of.BpoCd == SBPO.Trim()
                //             select of).ToList();
                objdata = (from a in model
                           select
                      new SelectListItem
                      {
                          Text = a.Text,
                          Value = a.Value
                      }).ToList();


                //var obj = (from of in context.V12BillPayingOfficers
                //           where of.Bpo.Trim().ToUpper().StartsWith(SBPO.Trim().ToUpper()) || of.BpoCd.Trim().ToUpper().StartsWith(SBPO.Trim().ToUpper())
                //           select of).ToList();
                //objdata = (from a in obj
                //           select
                //      new SelectListItem
                //      {
                //          Text = a.BpoCd + "-" + a.Bpo,
                //          Value = a.BpoCd
                //      }).ToList();
            }
            return objdata;
        }
        public static List<SelectListItem> GetBillPayingOfficer()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> objdata = new List<SelectListItem>();

            var obj = (from of in context.V12BillPayingOfficers
                           //where of.Bpo.Trim().ToUpper().StartsWith(SBPO.Trim().ToUpper()) || of.BpoCd.Trim().ToUpper().StartsWith(SBPO.Trim().ToUpper())
                       select of).ToList();
            objdata = (from a in obj
                       select
                  new SelectListItem
                  {
                      Text = a.BpoCd + "-" + a.Bpo,
                      Value = a.BpoCd
                  }).ToList();

            return objdata;
        }

        public static List<SelectListItem> GetEditBillPayingOfficer(string BpoCd)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            var query = (from bpo in context.T12BillPayingOfficers
                         join city in context.T03Cities on bpo.BpoCityCd equals city.CityCd
                         where bpo.BpoCd == BpoCd
                         orderby bpo.BpoName
                         select new
                         {
                             BPO_CD = bpo.BpoCd,
                             BPO_NAME = bpo.BpoCd + "-" + bpo.BpoName + "/" + bpo.BpoRly + "/" +
                                  (bpo.BpoAdd != null ? bpo.BpoAdd + "/" : "") +
                                  (city.Location != null ? city.City + "/" + city.Location : city.City)
                         }).ToList();

            List<SelectListItem> objdata = (from a in query
                                            select
                                       new SelectListItem
                                       {
                                           Text = a.BPO_NAME,
                                           Value = a.BPO_CD
                                       }).ToList();
            return objdata;
        }

        public static List<SelectListItem> GetConsigneeUsingConsigneeBeforEdit(string IMMS_RLY_CD)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            var RLY_CD = (from m in context.T91Railways where m.ImmsRlyCd == IMMS_RLY_CD select m.RlyCd).FirstOrDefault();
            List<SelectListItem> objdata = new List<SelectListItem>();
            if (RLY_CD != null)
            {
                var query = from consignee in context.T06Consignees
                            join city in context.T03Cities on consignee.ConsigneeCity equals city.CityCd
                            where consignee.ConsigneeType == "R" && consignee.ConsigneeFirm.Trim().ToUpper() == RLY_CD.Trim().ToUpper()
                            orderby
                                (consignee.ConsigneeDesig ?? "") + "/" +
                                (consignee.ConsigneeDept ?? "") + "/" +
                                (consignee.ConsigneeFirm ?? "") + "/" +
                                (consignee.ConsigneeAdd1 ?? "") + "/" +
                                (city.Location ?? city.City)
                            select new
                            {
                                CONSIGNEE_CD = consignee.ConsigneeCd,
                                CONSIGNEE_NAME =
                                    consignee.ConsigneeCd + "-" +
                                    (consignee.ConsigneeDesig ?? "") + "/" +
                                    (consignee.ConsigneeDept ?? "") + "/" +
                                    (consignee.ConsigneeFirm ?? "") + "/" +
                                    (consignee.ConsigneeAdd1 ?? "") + "/" +
                                    (city.Location != null ? city.Location + " : " + city.City : city.City)
                            };
                objdata = (from a in query
                           select
                      new SelectListItem
                      {
                          Text = a.CONSIGNEE_NAME,
                          Value = Convert.ToString(a.CONSIGNEE_CD)
                      }).ToList();
            }
            return objdata;
        }

        public static List<SelectListItem> GetBillPayingOfficerBeforEdit(string IMMS_RLY_CD)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            var RLY_CD = (from m in context.T91Railways where m.ImmsRlyCd == IMMS_RLY_CD select m.RlyCd).FirstOrDefault();
            var query = (from bpo in context.T12BillPayingOfficers
                         join city in context.T03Cities on bpo.BpoCityCd equals city.CityCd
                         where bpo.BpoType == "R" && bpo.BpoRly.Trim().ToUpper() == RLY_CD
                         orderby bpo.BpoName
                         select new
                         {
                             BPO_CD = bpo.BpoCd,
                             BPO_NAME = bpo.BpoCd + "-" + bpo.BpoName + "/" + bpo.BpoRly + "/" +
                                 (bpo.BpoAdd != null ? bpo.BpoAdd + "/" : "") +
                                 (city.Location != null ? city.City + "/" + city.Location : city.City)
                         }).ToList();

            List<SelectListItem> objdata = (from a in query
                                            select
                                       new SelectListItem
                                       {
                                           Text = a.BPO_NAME,
                                           Value = a.BPO_CD
                                       }).ToList();
            return objdata;
        }

        public static List<SelectListItem> GetIeCity(int IeCityId)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<T03City> obj = null;
            if (IeCityId > 0)
            {
                obj = (from t03 in context.T03Cities
                       where t03.CityCd == IeCityId
                       select t03).ToList();
            }
            else
            {
                obj = (from t03 in context.T03Cities
                           //where t03.CityCd == IeCityId
                       select t03).ToList();
            }


            List<SelectListItem> objdata = (from a in obj
                                            select
                                       new SelectListItem
                                       {
                                           Text = a.Location != null ? a.Location + " : " + a.City : a.City,
                                           Value = Convert.ToString(a.CityCd),
                                       }).ToList();
            return objdata;
        }

        public static List<SelectListItem> GetConsignee(string RlyCd, string RlyNonrly)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_RLY_NONRLY", OracleDbType.Varchar2, RlyNonrly, ParameterDirection.Input);
            par[1] = new OracleParameter("p_client", OracleDbType.Varchar2, RlyCd, ParameterDirection.Input);
            par[2] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GetConsignee", par, 1);

            List<PO_MasterDetailsModel> model = new List<PO_MasterDetailsModel>();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 1 && Convert.ToInt32(ds.Tables[0].Rows[0]["CONSIGNEE_CD"]) == 0)
                {
                    model = new List<PO_MasterDetailsModel>();
                }
                else
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model = JsonConvert.DeserializeObject<List<PO_MasterDetailsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
                }
            }

            List<SelectListItem> obj = (from a in model.ToList()
                                        select
                                   new SelectListItem
                                   {
                                       Text = a.CONSIGNEE_NAME,
                                       Value = a.CONSIGNEE_CD
                                   }).ToList();
            return obj;
        }

        public static List<SelectListItem> GetConsigneeUsingConsignee(int ConsigneeSearch)
        {
            List<SelectListItem> objdata = new List<SelectListItem>();
            if (ConsigneeSearch != 0)
            {
                ModelContext context = new(DbContextHelper.GetDbContextOptions());
                var obj = (from of in context.V06Consignees
                           where of.ConsigneeCd == ConsigneeSearch
                           select of).ToList();

                objdata = (from a in obj
                           select
                      new SelectListItem
                      {
                          Text = a.ConsigneeCd + "-" + a.Consignee,
                          Value = Convert.ToString(a.ConsigneeCd)
                      }).ToList();
            }
            return objdata;
        }

        public static List<SelectListItem> GetConsigneeUsingConsigneeLike(string ConsigneeSearch)
        {
            List<SelectListItem> objdata = new List<SelectListItem>();
            if (ConsigneeSearch != null && ConsigneeSearch != "")
            {
                //ModelContext context = new(DbContextHelper.GetDbContextOptions());
                //var obj = (from of in context.V06Consignees
                //           where of.Consignee.Trim().ToUpper().StartsWith(ConsigneeSearch.ToUpper())
                //           select of).ToList();

                List<SelectListItem> model = new();
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_searchTerm", OracleDbType.Varchar2, ConsigneeSearch, ParameterDirection.Input);
                par[1] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("GetConsigneeData", par, 1);
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model = JsonConvert.DeserializeObject<List<SelectListItem>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }

                objdata = (from a in model
                           select
                      new SelectListItem
                      {
                          Text = a.Text,
                          Value = a.Value
                      }).ToList();
            }
            return objdata;
        }

        public static List<SelectListItem> GetEditConsigneeUsingConsignee(string ConsigneeCd)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> objdata = new List<SelectListItem>();
            if (ConsigneeCd != null)
            {
                var query = (from consignee in context.T06Consignees
                             join city in context.T03Cities on consignee.ConsigneeCity equals city.CityCd
                             where consignee.ConsigneeCd == Convert.ToInt32(ConsigneeCd)
                             orderby (
                                 (consignee.ConsigneeDesig ?? "") + "/" +
                                 (consignee.ConsigneeDept ?? "") + "/" +
                                 (consignee.ConsigneeFirm ?? "") + "/" +
                                 (consignee.ConsigneeAdd1 ?? "") + "/" +
                                 (city.Location ?? city.City)
                             )
                             select new
                             {
                                 CONSIGNEE_CD = consignee.ConsigneeCd,
                                 CONSIGNEE_NAME =
                                     consignee.ConsigneeCd + "-" +
                                     (consignee.ConsigneeDesig ?? "") + "/" +
                                     (consignee.ConsigneeDept ?? "") + "/" +
                                     (consignee.ConsigneeFirm ?? "") + "/" +
                                     (consignee.ConsigneeAdd1 ?? "") + "/" +
                                     (city.Location != null ? city.Location + " : " + city.City : city.City)
                             });

                objdata = (from a in query
                           select
                      new SelectListItem
                      {
                          Text = a.CONSIGNEE_NAME,
                          Value = Convert.ToString(a.CONSIGNEE_CD)
                      }).ToList();
            }
            return objdata;
        }

        public static int? getConsigneeCd(string ConsigneeCd, string IMMS_RLY_CD)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            int? ConsigneeCd1 = (from detail in context.ImmsRitesPoDetails
                                 where detail.ImmsConsigneeCd == ConsigneeCd
                                && detail.ImmsRlyCd == IMMS_RLY_CD
                                && detail.ConsigneeCd != null
                                 select detail.ConsigneeCd).Distinct().FirstOrDefault();

            return ConsigneeCd1;
        }

        public static List<SelectListItem> GetSummaryConsignee()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> objdata = (from a in context.V06Consignees
                                            orderby a.Consignee
                                            select
                                       new SelectListItem
                                       {
                                           Text = a.ConsigneeCd + "-" + a.Consignee,
                                           Value = Convert.ToString(a.ConsigneeCd)
                                       }).ToList();
            return objdata;
        }

        public static List<SelectListItem> GetConsigneeList(string CaseNo)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> objdata = (from P in context.T14PoBpos
                                            join C in context.T06Consignees on P.ConsigneeCd equals C.ConsigneeCd
                                            join D in context.T03Cities on C.ConsigneeCity equals D.CityCd
                                            where P.CaseNo == CaseNo
                                            select
                                       new SelectListItem
                                       {
                                           Text = (C.ConsigneeCd + "-" + (string.IsNullOrEmpty(C.ConsigneeDesig) ? "" : C.ConsigneeDesig + "/") + (string.IsNullOrEmpty(C.ConsigneeDept) ? "" : C.ConsigneeDept + "/") + (string.IsNullOrEmpty(C.ConsigneeFirm) ? "" : C.ConsigneeFirm + "/") + (string.IsNullOrEmpty(C.ConsigneeAdd1) ? "" : C.ConsigneeAdd1 + "/") + (string.IsNullOrEmpty(D.Location) ? "" : D.Location + " : " + D.City)),
                                           Value = Convert.ToString(P.ConsigneeCd)
                                       }).ToList();
            return objdata;
        }

        public static List<SelectListItem> GetUnitOfMeasurment()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> objdata = (from a in context.T04Uoms
                                            orderby a.UomLDesc
                                            select
                                       new SelectListItem
                                       {
                                           Text = a.UomLDesc,
                                           Value = Convert.ToString(a.UomCd)
                                       }).ToList();
            return objdata;
        }

        public static List<SelectListItem> DocType()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Inernal Records";
            single.Value = "I";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Firm Certificate(Like RDSO, Approval, Type test etc.)";
            single.Value = "F";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Raw Material/Invoice";
            single.Value = "R";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Calibration Records";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> RlyBPOFee()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "2610/- (For PO Value less than 5 lakhs)";
            single.Value = "2610";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "5450/- (Man day for RINL)";
            single.Value = "5450";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "0.522% (For PO Value between 5 Lakhs to 1 Crore)";
            single.Value = "0.522";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "0.116% (For PO Value between 1 Crore to 25 Crores)";
            single.Value = "0.116";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "0.053% (For PO Value between 25 Crores to 100 Crores)";
            single.Value = "0.053";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "0.035% (For PO Value between more then 100 Crores)";
            single.Value = "0.035";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "0.9% (For PO Date on or before 26-Nov-2022)";
            single.Value = "0.9";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "0.55% (For CR Billing)";
            single.Value = "0.55";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetClient()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "RSM";
            single.Value = "1";
            single.Selected = true;
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "URM";
            single.Value = "2";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "JINDAL RAILWAY";
            single.Value = "3";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "JINDAL NON RAILWAY.";
            single.Value = "4";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "PLATE(BSP)";
            single.Value = "5";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetClientForII()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "RSM";
            single.Value = "1";
            single.Selected = true;
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "URM";
            single.Value = "2";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "JINDAL";
            single.Value = "3";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "PLATE(BSP)";
            single.Value = "4";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetWeight()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "13M";
            single.Value = "1";
            single.Selected = true;
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "26M";
            single.Value = "2";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "52M";
            single.Value = "3";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "65M";
            single.Value = "4";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "86.67M";
            single.Value = "5";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "87M";
            single.Value = "6";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "117M";
            single.Value = "7";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "130M";
            single.Value = "8";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetLength()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "52kg";
            single.Value = "1";
            single.Selected = true;
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "60kg";
            single.Value = "2";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> ICType()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Finalization";
            single.Value = "F";
            single.Selected = true;
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Stage";
            single.Value = "S";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetUsers()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> city = (from a in context.T02Users
                                         select
                                    new SelectListItem
                                    {
                                        Text = a.UserName,
                                        Value = Convert.ToString(a.UserId)
                                    }).ToList();
            return city;
        }

        //public static List<SelectListItem> GetUsersgetbyName(string name)
        //{
        //    ModelContext context = new(DbContextHelper.GetDbContextOptions());
        //    List<SelectListItem> city = (from a in context.T02Users
        //                                 where a.UserName.Trim().ToUpper().StartsWith(name.Trim().ToUpper())
        //                                 select
        //                            new SelectListItem
        //                            {
        //                                Text = a.UserName,
        //                                Value = Convert.ToString(a.UserId)
        //                            }).ToList();
        //    return city;
        //}

        public static List<SelectListItem> GetUsersgetbyID(string User_ID)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> city = (from a in context.UserMasters
                                         where a.Id == Convert.ToInt64(User_ID)
                                         select
                                    new SelectListItem
                                    {
                                        Text = a.Name,
                                        Value = Convert.ToString(a.Id)
                                    }).ToList();
            return city;
        }
        public static List<SelectListItem> GetUsersgetbyName(string name, string userType)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> city = (from a in context.UserMasters
                                         where a.Name.Trim().ToUpper().StartsWith(name.Trim().ToUpper())
                                         && (userType == null || a.UserType == userType)
                                         select
                                    new SelectListItem
                                    {
                                        Text = a.Name,
                                        Value = Convert.ToString(a.Id)
                                    }).ToList();
            return city;
        }

        public static List<SelectListItem> GetRoles()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> city = (from a in context.Roles
                                         select
                                    new SelectListItem
                                    {
                                        Text = a.Rolename,
                                        Value = Convert.ToString(a.RoleId)
                                    }).ToList();
            return city;
        }

        public static List<SelectListItem> GetRailwayCode(string type = "")
        {

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            if (type != "")
            {
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_bpo_type", OracleDbType.Varchar2, type.ToString() == "" ? DBNull.Value : type.ToString(), ParameterDirection.Input);
                par[1] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("SP_GET_RAILWAY_CODES", par, 1);
                DataTable dt = ds.Tables[0];

                List<RailwayCodeModel> list = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    list = JsonConvert.DeserializeObject<List<RailwayCodeModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }

                ModelContext context = new(DbContextHelper.GetDbContextOptions());
                selectListItems = (from a in list
                                   select
                              new SelectListItem
                              {
                                  Text = a.RLY_CD,
                                  Value = a.RLY_CD,
                              }).ToList();
            }
            return selectListItems;
        }

        public static string GetRailway(string type = "")
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            string types = context.T91Railways.Where(x => x.RlyCd == type.ToString()).Select(x => x.Railway).FirstOrDefault();
            return types;
        }

        public static List<SelectListItem> GetIENameIsStatusNull(string RegionCode)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> IE = (from a in ModelContext.T09Ies
                                       orderby a.IeName
                                       where a.IeRegion == RegionCode && a.IeStatus == null
                                       select
                                  new SelectListItem
                                  {
                                      Text = Convert.ToString(a.IeName),
                                      Value = Convert.ToString(a.IeCd)
                                  }).ToList();
            return IE;
        }

        public static List<SelectListItem> GetOfficerIsCoStatusIsNull(string REGION)
        {

            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            return (from a in ModelContext.T08IeControllOfficers
                    where a.CoRegion == REGION && a.CoStatus == null
                    select new SelectListItem
                    {
                        Text = Convert.ToString(a.CoName),
                        Value = Convert.ToString(a.CoCd)
                    }).OrderBy(c => c.Text).ToList();
        }

        public static string DateConcate(string date) // Date formate is dd/MM/yyyy to Convert yyyyMMdd
        {
            string myYear = date.Substring(6, 4);
            string myMonth = date.Substring(3, 2);
            string myDay = date.Substring(0, 2);
            string date_out = myYear + myMonth + myDay;
            return date_out;
        }

        public static string ConvertDateFormat(this DateTime dt)
        {
            return dt.ToString(Common.CommonDateFormate1);
        }

        public static IEnumerable<SelectListItem> GetIENameForUnregCall(string RegionCode, bool IsReadOnly)
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            var obj = (from c in context.T09Ies
                       where (!IsReadOnly ? c.IeStatus == null : c.IeStatus == c.IeStatus)
                       && c.IeRegion == RegionCode
                       select new SelectListItem
                       {
                           Value = c.IeCd.ToString(),
                           Text = c.IeName
                       }).OrderBy(c => c.Text).ToList();
            return obj;
        }

        public static List<SelectListItem> GetYears()
        {
            List<SelectListItem> list = new();
            var StartYear = 2008;

            var years = Enumerable.Range(StartYear, DateTime.Now.Year - StartYear + 1).OrderByDescending(x => x);

            foreach (var year in years)
            {
                list.Add(new SelectListItem() { Value = year.ToString(), Text = year.ToString() });
            }
            return list;
        }

        public static List<SelectListItem> GetMonths()
        {
            List<SelectListItem> list = new();

            list.AddRange(DateTimeFormatInfo
                  .InvariantInfo
                  .MonthNames
                  .Where(m => !String.IsNullOrEmpty(m))
                  .Select((monthName, index) => new SelectListItem
                  {
                      Value = (index + 1).ToString(),
                      Text = monthName
                  }).ToList());

            return list;
        }

        //public static IEnumerable<SelectListItem> GetFeeType()
        //{
        //    IEnumerable<SelectListItem> item1 = new SelectListItem[] { new SelectListItem { Text = "Man days Basis", Value = "D" } };
        //    IEnumerable<SelectListItem> item2 = new SelectListItem[] { new SelectListItem { Text = "Hourly Basis", Value = "M" } };
        //    IEnumerable<SelectListItem> item3 = new SelectListItem[] { new SelectListItem { Text = "Lump sum", Value = "L" } };
        //    IEnumerable<SelectListItem> item4 = new SelectListItem[] { new SelectListItem { Text = "Percentage Basis", Value = "P" } };

        //    return item1.Concat(item2).Concat(item3).Concat(item4);
        //}

        //public static IEnumerable<SelectListItem> GetTaxType()
        //{
        //    IEnumerable<SelectListItem> item1 = new SelectListItem[] { new SelectListItem { Text = "Fee Inclusive Service Tax", Value = "I" } };
        //    IEnumerable<SelectListItem> item2 = new SelectListItem[] { new SelectListItem { Text = "Service Tax Charged separately", Value = "X" } };
        //    IEnumerable<SelectListItem> item3 = new SelectListItem[] { new SelectListItem { Text = "No Service Tax(RITES Billing)", Value = "N" } };
        //    IEnumerable<SelectListItem> item4 = new SelectListItem[] { new SelectListItem { Text = "Fee Inclusive of Service Tax (Don't Print S.Tax in Bill)", Value = "D" } };

        //    return item1.Concat(item2).Concat(item3).Concat(item4);
        //}

        //public static IEnumerable<SelectListItem> GetTaxType_GST()
        //{
        //    IEnumerable<SelectListItem> item1 = new SelectListItem[] { new SelectListItem { Text = "IGST @ 18%", Value = "I" } };
        //    IEnumerable<SelectListItem> item2 = new SelectListItem[] { new SelectListItem { Text = "CGST @ 9% & SGST @ 9%", Value = "C" } };
        //    IEnumerable<SelectListItem> item3 = new SelectListItem[] { new SelectListItem { Text = "NO GST", Value = "X" } };
        //    IEnumerable<SelectListItem> item4 = new SelectListItem[] { new SelectListItem { Text = "Fee Inclusive of IGST @ 18%", Value = "Y" } };
        //    IEnumerable<SelectListItem> item5 = new SelectListItem[] { new SelectListItem { Text = "Fee Inclusive of CGST @ 9% & SGST @ 9%", Value = "Z" } };

        //    return item1.Concat(item2).Concat(item3).Concat(item4).Concat(item5);
        //}

        public static IEnumerable<SelectListItem> GetStates()
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from c in context.T92States
                    select new SelectListItem
                    {
                        Value = c.StateCd.ToString(),
                        Text = c.StateName
                    }).OrderBy(c => c.Text).ToList();
        }

        public static IEnumerable<SelectListItem> GetPayingWindow()
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from c in context.T73PayingWindows
                    select new SelectListItem
                    {
                        Value = c.PayWindowId,
                        Text = c.PayWindowDesc
                    }).OrderBy(c => c.Text).ToList();
        }

        public static IEnumerable<SelectListItem> GetCountries()
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from c in context.CountryMasters
                    select new SelectListItem
                    {
                        Value = c.CountryCode.ToString(),
                        Text = c.CountryName
                    }).OrderBy(c => c.Text).ToList();
        }

        public static IEnumerable<SelectListItem> GetDesignations()
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from c in context.T07RitesDesigs
                    where c.Isdeleted != 1
                    select new SelectListItem
                    {
                        Value = c.RDesigCd.ToString(),
                        Text = c.RDesignation
                    }).OrderBy(c => c.Text).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetCOStatus()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.COStatus)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetCMType()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.COType)).ToList();
        }

        public static List<SelectListItem> GetVendCd(string vend_cd)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            if (vend_cd != "0")
            {
                ModelContext context = new(DbContextHelper.GetDbContextOptions());
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_vend_cd", OracleDbType.Varchar2, vend_cd != "" ? vend_cd : DBNull.Value, ParameterDirection.Input);
                par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("GET_VENDOR_DETAILSForDropDown", par, 1);
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model = JsonConvert.DeserializeObject<List<SelectListItem>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
                }
            }
            return model;
        }

        public static List<SelectListItem> getInspectingAgency()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "RITES";
            single.Value = "R";
            single.Selected = true;
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Consignee";
            single.Value = "C";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "PO Cancelled";
            single.Value = "X";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "PO Suspended For Inspection";
            single.Value = "S";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<TextValueDropDownDTO> getServTax()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.ServTax)).ToList();
        }

        public static IEnumerable<SelectListItem> GetRailway()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropList = (from a in ModelContext.T91Railways
                                             orderby a.Railway
                                             select new SelectListItem
                                             {
                                                 Text = Convert.ToString(a.Railway),
                                                 Value = Convert.ToString(a.RlyCd)
                                             }).ToList();
            return dropList;
        }

        public static IEnumerable<SelectListItem> GetConsigneeDesignation()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropList = (from a in ModelContext.T90RlyDesignations
                                             orderby a.RlyDesigCd
                                             select new SelectListItem
                                             {
                                                 Text = Convert.ToString(a.RlyDesigCd),
                                                 Value = Convert.ToString(a.RlyDesigCd)
                                             }).ToList();
            return dropList;
        }

        public static IEnumerable<SelectListItem> GetConsigneeCity()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropList = new List<SelectListItem>();
            dropList = ModelContext.T03Cities
                            .OrderBy(city => city.City)
                            .Select(city => new SelectListItem
                            {
                                Value = Convert.ToString(city.CityCd),
                                Text = city.Location != null ? city.Location + " : " + city.City : city.City
                            }).ToList();
            return dropList;
        }

        public static IEnumerable<SelectListItem> GetItems()
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from c in context.T61ItemMasters
                    where c.Isdeleted != 1
                    select new SelectListItem
                    {
                        Value = c.ItemCd.ToString(),
                        Text = c.ItemDesc
                    }).OrderBy(c => c.Text).ToList();
        }

        public static List<SelectListItem> GetIEIEToWhomIssued(string RegionCode)
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from c in context.T09Ies
                    where c.IeStatus == null && c.IeRegion == RegionCode
                    select new SelectListItem
                    {
                        Value = c.IeCd.ToString(),
                        Text = c.IeName
                    }).OrderBy(c => c.Text).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetBookSubmitted()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.BookSubmitted)).ToList();
        }

        public static IEnumerable<SelectListItem> GetClustersName(string RegionCode, string DepartmentName)
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from c in context.T99ClusterMasters
                    where c.Isdeleted != 1 && c.RegionCode == RegionCode && c.DepartmentName != null && c.DepartmentName == DepartmentName
                    select new SelectListItem
                    {
                        Value = c.ClusterCode.ToString(),
                        Text = c.ClusterName
                    }).OrderBy(c => c.Text).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetDepartment()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.Department)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetRegion()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.Region)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetActiveInActive()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.ActiveInActive)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetUserType()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.UserType)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetIEPosting()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.IEPosting)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetIEStatus()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.IEStatus)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetIEJobType()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.IEJobType)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetBPOFeeType()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.BPOFeeType)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetBPOTaxType()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.BPOTaxType)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetBPOFlag()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.BPOFlag)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetBPOAdvFlag()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.BPOAdvFlag)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetAdvanceBill()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.AdvanceBill)).ToList();
        }

        public static IEnumerable<SelectListItem> GetIEToWhomIssued(string Region)
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from c in context.T09Ies
                    where c.IeRegion == Region
                    select new SelectListItem
                    {
                        Value = c.IeCd.ToString(),
                        Text = c.IeName
                    }).OrderBy(c => c.Text).ToList();
        }

        public static IEnumerable<SelectListItem> GetIENameByIECD(int IeCd)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            List<SelectListItem> contacts = (from v in ModelContext.T09Ies
                                             where v.IeCd == IeCd
                                             select
                                     new SelectListItem
                                     {
                                         Text = v.IeName,
                                         Value = Convert.ToString(v.IeCd)
                                     }).ToList();
            return contacts;
        }

        public static List<TextValueDropDownDTO> GetIcStatus()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.IcStatus)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetSector()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.Sector)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetFPart()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.FPart)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetTAXType()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.TAXType)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetFeeType()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.BPOFeeType)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetTaxType_GST()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.TaxType_GST)).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetTaxType_GST_07()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.TaxType_GST_07)).ToList();
        }
        public static IEnumerable<TextValueDropDownDTO> GetTaxType_GST_O()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.TaxType_GST_O)).ToList();
        }


        public static string[] GetVenderDetails(int VendCd)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());

            var objData = (from t05 in ModelContext.T05Vendors
                           join t03 in ModelContext.T03Cities on t05.VendCityCd equals t03.CityCd
                           where t05.VendCd == VendCd
                           select new
                           {
                               VENDOR = t05.VendName + "," +
                                        (t05.VendAdd2 != null ? (t05.VendAdd1 + "/" + t05.VendAdd2) : t05.VendAdd1) + "/" + t03.City,
                               t05.VendEmail
                           }).FirstOrDefault();
            string[] strings = new string[2];
            if (objData != null)
            {
                strings[0] = objData.VENDOR;
                strings[1] = objData.VendEmail;
            }
            return strings;
        }

        public static List<SelectListItem> GetControllingOfficers(string Region)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from a in context.T08IeControllOfficers
                    where a.CoStatus == null && a.CoRegion == Region
                    select new SelectListItem
                    {
                        Text = Convert.ToString(a.CoName),
                        Value = Convert.ToString(a.CoCd)
                    }).OrderBy(c => c.Text).ToList();
        }

        public static IEnumerable<SelectListItem> GetIEByCo(string Region, int COCd)
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from c in context.T09Ies
                    where c.IeStatus == null && c.IeRegion == Region && c.IeCoCd == COCd
                    select new SelectListItem
                    {
                        Value = c.IeCd.ToString(),
                        Text = c.IeName
                    }).OrderBy(c => c.Text).ToList();
        }

        public static List<SelectListItem> GetRemarkingToIE(string Region)
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from c in context.T09Ies
                    where c.IeStatus == null && c.IeRegion == Region
                    select new SelectListItem
                    {
                        Value = c.IeCd.ToString(),
                        Text = c.IeName
                    }).OrderBy(c => c.Text).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetActionProposed()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.ActionProposed)).ToList();
        }

        public static List<SelectListItem> GetIeCity()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> objdata = (from a in context.T03Cities
                                            where a.City != null && a.Location != null
                                            select new SelectListItem
                                            {
                                                Value = a.CityCd.ToString(),
                                                Text = a.Location != null ? a.Location + " : " + a.City : a.City,
                                            }).ToList();
            return objdata;
        }

        public static List<TextValueDropDownDTO> GetConsigneeType()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.ConsigneeType)).ToList();
        }

        public static List<TextValueDropDownDTO> GetRailType()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.RailTypes)).ToList();
        }

        public static string ConvertToUpper(string str)
        {
            return str != null ? str.ToUpper() : "";
        }

        public static IEnumerable<TextValueDropDownDTO> GetCriteria()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.Criteria)).ToList();
        }

        public static int DiffDays(DateTime StartDate, DateTime EndDate)
        {
            TimeSpan difference = EndDate - StartDate;
            int daysDifference = (int)difference.TotalDays;
            return daysDifference;
        }

        public static List<TextValueDropDownDTO> GetScopeOfsector()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.ScopeOfsector)).ToList();
        }

        public static List<SelectListItem> GetCRISRLYStatus()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>() {
                new SelectListItem() { Text = "Passed", Value = "P" },
                new SelectListItem() { Text = "Returned", Value = "R" },
                new SelectListItem() { Text = "Pending", Value = "X" },
                new SelectListItem() { Text = "Returned Bills Resent", Value = "S" },
                new SelectListItem() { Text = "ALL", Value = "A" }
            };
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetCRISRLYStatusDate()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>() {
                new SelectListItem() { Text = "Invoice Date", Value = "A" },
                new SelectListItem() { Text = "Payment Date", Value = "P" }
            };
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetControllingSelectedIE(string Region, string CO)
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from c in context.T09Ies
                    where c.IeStatus == null && c.IeRegion == Region && (CO == "" || (c.IeCoCd == Convert.ToInt16(CO) && CO != ""))
                    select new SelectListItem
                    {
                        Value = c.IeCd.ToString(),
                        Text = c.IeName
                    }).OrderBy(c => c.Text).ToList();
        }

        public static IEnumerable<TextValueDropDownDTO> GetOnlineStatus()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.Status)).ToList();
        }

        public static IEnumerable<SelectListItem> GetBPORailway(string ClientType)
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());

            if (ClientType == "R")
            {
                return (from c in context.T91Railways
                        select new SelectListItem
                        {
                            Value = c.RlyCd.ToString().Trim().ToUpper(),
                            Text = c.Railway
                        }).OrderBy(c => c.Text).ToList();
            }
            else
            {
                return (from c in context.T12BillPayingOfficers
                        where c.BpoType == ClientType
                        select new SelectListItem
                        {
                            Value = c.BpoRly.ToString().Trim().ToUpper(),
                            Text = c.BpoOrgn
                        }).Distinct().OrderBy(c => c.Text).ToList();

            }
        }

        public static IEnumerable<TextValueDropDownDTO> GetCallsStatus()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.CallsStatus)).ToList();
        }

        public static string ConvertDateTimeFormat(this DateTime dt)
        {
            return dt.ToString(Common.CommonDateTimeFormat);
        }

        public static List<SelectListItem> GetContract()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> city = (from a in context.T100Contracts
                                         where a.Isdeleted != 1
                                         select
                                    new SelectListItem
                                    {
                                        Text = a.Clientname,
                                        Value = Convert.ToString(a.Id)
                                    }).ToList();
            return city;
        }

        public static List<SelectListItem> GetIterUnitRegionList()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>() {
                new SelectListItem() { Text = "Northern Region", Value = "3007" },
                new SelectListItem() { Text = "Eastern Region", Value = "3008" },
                new SelectListItem() { Text = "Southern Region", Value = "3009" },
                new SelectListItem() { Text = "Western Region", Value = "3006" },
                new SelectListItem() { Text = "Central Region", Value = "3066" },
                new SelectListItem() { Text = "Bill Adjustment of Old System", Value = "9999" },
                new SelectListItem() { Text = "Miscelleanous Adjustments", Value = "9998" }
            };
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> GetBankNameWithFMIS()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from bank in context.T94Banks
                    where bank.FmisBankCd != null
                    orderby bank.BankName
                    select new SelectListItem
                    {
                        Value = bank.BankCd.ToString(),
                        Text = bank.FmisBankCd.ToString().PadLeft(4, '0') + "-" + bank.BankName,
                    }).ToList();
        }

        public static List<SelectListItem> GetBankNames()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from a in context.T94Banks
                    where a.BankCd < 990
                    orderby a.BankName
                    select new SelectListItem
                    {
                        Text = a.BankName,
                        Value = a.BankCd.ToString()
                    }).ToList();
        }

        public static List<SelectListItem> GetAccountCode(string Role_Cd)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            if (Role_Cd == "5")
            {
                return (from a in context.T95AccountCodes
                        where (a.AccCd == 2210 || a.AccCd == 2212)
                        orderby a.AccDesc
                        select new SelectListItem
                        {
                            Text = a.AccDesc.ToString() + ":" + a.AccCd.ToString(),
                            Value = a.AccCd.ToString()
                        }).ToList();
            }
            else
            {
                return (from a in context.T95AccountCodes
                        where a.AccCd < 3000
                        orderby a.AccDesc
                        select new SelectListItem
                        {
                            Text = a.AccDesc.ToString() + ":" + a.AccCd.ToString(),
                            Value = a.AccCd.ToString()
                        }).ToList();
            }
        }

        //    public static List<TextValueDropDownDTO> NIWorkType()
        //    {
        //        return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.NIWorkType)).ToList();
        //        public enum NIWorkType
        //    {
        //        [Description("Training")]
        //        T,
        //        [Description("Leave")]
        //        L,
        //        [Description("Office")]
        //        O,
        //        [Description("Joint Inspection")]
        //        J,
        //        [Description("Firm Visit")]
        //        F,
        //        [Description("Others")]
        //        X,
        //    }
        //}

        public static List<SelectListItem> NIWorkType()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>() {
                new SelectListItem() { Text = "Training", Value = "T" },
                new SelectListItem() { Text = "Leave", Value = "L" },
                new SelectListItem() { Text = "Office", Value = "O" },
                new SelectListItem() { Text = "Joint Inspection", Value = "J" },
                new SelectListItem() { Text = "Firm Visit", Value = "F" },
                new SelectListItem() { Text = "Others", Value = "X" }
            };
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> BindIEStamps()
        {
            DataSet ds = new DataSet();
            ModelContext context = new ModelContext(DbContextHelper.GetDbContextOptions());
            try
            {
                using (var conn = (OracleConnection)context.Database.GetDbConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT 0 AS IE_STAMP_CD,'SELECT STAMP' AS IE_STAMPS_DETAIL FROM DUAL UNION SELECT IE_STAMP_CD, IE_STAMPS_DETAIL FROM IE_STAMPS ORDER BY IE_STAMP_CD"; ;

                        using (var adapter = new OracleDataAdapter(cmd))
                        {
                            adapter.Fill(ds);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Handle exceptions if needed
            }
            DataTable dt = ds.Tables[0];
            List<SelectListItem> lst = dt.AsEnumerable().Select(row => new SelectListItem
            {
                Text = row["IE_STAMPS_DETAIL"].ToString(),
                Value = row["IE_STAMP_CD"].ToString()
            }).ToList();
            return lst;
        }

        public static List<SelectListItem> BindConsignee(string CASE_NO, string CALL_RECV_DT, string CALL_SNO)
        {
            DataSet ds = new DataSet();
            ModelContext context = new ModelContext(DbContextHelper.GetDbContextOptions());
            try
            {
                using (var conn = (OracleConnection)context.Database.GetDbConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        var sql = "";
                        sql = "select 0 as consignee_cd,'Select Consignee' as consignee_firm from dual union select distinct csn.consignee_cd,CSN.consignee_cd ||'-'|| csn.consignee consignee_firm   from t18_call_details CDT inner join V06_CONSIGNEE CSN ";
                        sql += " on cdt.consignee_cd = csn.consignee_cd where case_no = '" + CASE_NO + "' and call_recv_dt = TO_date('" + Convert.ToDateTime(CALL_RECV_DT).ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy') and call_sno = '" + CALL_SNO + "' ";
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        using (var adapter = new OracleDataAdapter(cmd))
                        {
                            adapter.Fill(ds);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Handle exceptions if needed
            }
            DataTable dt = ds.Tables[0];
            List<SelectListItem> lst = dt.AsEnumerable().Select(row => new SelectListItem
            {
                Text = row["consignee_firm"].ToString(),
                Value = row["consignee_cd"].ToString()
            }).ToList();
            return lst;
        }

        public static List<SelectListItem> GetConsignneManufacturingType()
        {
            List<SelectListItem> lstManuf = new List<SelectListItem>() {
                new SelectListItem() { Text = "BHILAI STEEL PLANT", Value = "B" },
                new SelectListItem() { Text = "JINDAL STEEL & POWER LTD", Value = "J" },
                new SelectListItem() { Text = "Others", Value = "O" }
            };
            return lstManuf;
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

        public static List<SelectListItem> GetAllIeControllingOfficers()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            return (from a in context.T08IeControllOfficers
                    where a.CoStatus == null
                    orderby a.CoName
                    select new SelectListItem
                    {
                        Text = Convert.ToString(a.CoName),
                        Value = Convert.ToString(a.CoCd)
                    }).OrderBy(c => c.Text).ToList();
        }

        //public static List<SelectListItem> GetNewUserType()
        //{
        //    ModelContext context = new(DbContextHelper.GetDbContextOptions());
        //    List<SelectListItem> city = (from a in context.UserMasters
        //                                 select new SelectListItem
        //                                 {
        //                                     Text = a.UserType,
        //                                     Value = a.UserType
        //                                 })
        //                    .Distinct()
        //                    .ToList();
        //    return city;
        //}

        public static List<TextValueDropDownDTO> GetUserTypeLogin()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.UserTypeLogin)).ToList();
        }

        public static List<SelectListItem> GetNewUserType()
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> city = (from a in context.UserMasters
                                         select new SelectListItem
                                         {
                                             Text = a.UserType,
                                             Value = a.UserType
                                         })
                            .Distinct()
                            .ToList();
            return city;
        }

        public static List<SelectListItem> SetClientName(string ClientType, string ClientName)
        {
            var Client = ClientName.Split("=");
            var ClientCode = Client.Count() > 0 ? Client[0].Trim() : "";
            var ClientOrg = Client.Count() > 0 ? Client[1].Trim() : "";
            List<SelectListItem> lstClient = new List<SelectListItem>();
            if (ClientType == "R")
            {
                ModelContext modelContext = new ModelContext(DbContextHelper.GetDbContextOptions());
                lstClient = modelContext.T91Railways
                        .Where(x => x.RlyCd.Trim() == ClientCode.Trim() && x.Railway.Trim() == ClientOrg.Trim())
                        .Select(x => new SelectListItem { Value = x.RlyCd.Trim() + " = " + x.Railway.Trim(), Text = x.RlyCd.Trim() + " = " + x.Railway.Trim() })
                        .ToList();
            }
            else
            {
                ModelContext modelContext = new ModelContext(DbContextHelper.GetDbContextOptions());
                lstClient = modelContext.T12BillPayingOfficers
                        .Where(t => t.BpoType.Trim() == ClientType && t.BpoOrgn.Trim() == ClientOrg.Trim())
                        .Select(t => new SelectListItem { Value = t.BpoRly.Trim() + " = " + t.BpoOrgn.Trim(), Text = t.BpoRly.Trim() + " = " + t.BpoOrgn.Trim() })
                        .Distinct()
                        .ToList();

            }
            return lstClient;
        }

        public static List<SelectListItem> GetRailwayWithCode(string searchTerm)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropList = (from a in ModelContext.T91Railways
                                             where a.Railway.ToLower().Contains(searchTerm.ToLower())
                                             orderby a.Railway
                                             select new SelectListItem
                                             {
                                                 Text = Convert.ToString(a.RlyCd) + " = " + Convert.ToString(a.Railway),
                                                 Value = Convert.ToString(a.RlyCd) + " = " + Convert.ToString(a.Railway)
                                             }).ToList();
            return dropList.OrderBy(x => x.Text).ToList();
        }

        public static List<SelectListItem> GetHolidayMaster()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> IE = (from a in ModelContext.T111HolidayMasters
                                       orderby a.Id
                                       select new SelectListItem
                                       {
                                           Text = Convert.ToString(a.FinancialYear),
                                           Value = Convert.ToString(a.Id)
                                       }).ToList();
            return IE;
        }
        public static List<TextValueDropDownDTO> GetSAPType()
        {
            return EnumUtility<List<TextValueDropDownDTO>>.GetEnumDropDownStringValue(typeof(Enums.SAPType)).ToList();
        }

        public static List<SelectListItem> GetUserMaster()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> UM = (from a in ModelContext.UserMasters
                                       where a.UserType == "USERS" || a.UserType == "IE"
                                       orderby a.UserType ascending, a.Name ascending
                                       select new SelectListItem
                                       {
                                           Text = Convert.ToString(a.Name),
                                           Value = Convert.ToString(a.Id)
                                       }).ToList();
            return UM;
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

        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string sortColumn, bool ascending)
        {
            var parameter = Expression.Parameter(typeof(T), "p");

            string command = "OrderBy";

            if (!ascending)
            {
                command = "OrderByDescending";
            }

            Expression resultExpression = null;

            var property = typeof(T).GetProperty(sortColumn);

            // Handle nullable properties
            //if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            //{
            //    property = Nullable.GetUnderlyingType(property.PropertyType).GetProperty(sortColumn);
            //}

            var propertyAccess = Expression.MakeMemberAccess(parameter, property);

            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // finally, call the "OrderBy" / "OrderByDescending" method with the order by lamba expression
            resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { typeof(T), property.PropertyType },
               query.Expression, Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<T>(resultExpression);

        }

    }
}

