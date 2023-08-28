using IBS.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Text;
using System.Reflection.Metadata;
using IBS.DataAccess;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using IBS.Controllers;
using Humanizer.Localisation;

namespace IBS.Models
{
    public static class Common
    {

        public const string CommonDateFormate = "{0:MM/dd/yyyy}";
        public const string CommonDateFormateForJS = "DD-MM-YYYY";
        public const string CommonDateFormateForDT = "{0:dd/MM/yyyy}";
        public const string CommonDateFormate1 = "dd/MM/yyyy";

        public static string AccessDeniedMessage = "You don't have permission to do this action.";
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
                                           Text = Convert.ToString(a.DefectCd),
                                           Value = Convert.ToString(a.DefectDesc)
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
                                              Text = Convert.ToString(a.JiStatusCd),
                                              Value = Convert.ToString(a.JiStatusDesc)
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

        public static List<SelectListItem> VendorStatus()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "Active";
            single.Value = "A";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Banned/BlackListed";
            single.Value = "B";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Re-Instated";
            single.Value = "R";
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

        public static List<SelectListItem> OnlineCallStatus()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single.Text = "No";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Yes";
            single.Value = "Y";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> ClientType()
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
            single.Value = "PSU";
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
            if (CoCd == "R")
            {
                return (from a in ModelContext.T91Railways
                        where a.RlyCd != "CORE"
                        select new SelectListItem
                        {
                            Text = Convert.ToString(a.RlyCd),
                            Value = Convert.ToString(a.Railway)
                        }).OrderBy(c => c.Text).ToList();
            }
            else if (CoCd != "" && CoCd != null)
            {
                return (from a in ModelContext.T12BillPayingOfficers
                        where a.BpoType == CoCd
                        select new SelectListItem
                        {
                            Text = Convert.ToString(a.BpoRly),
                            Value = Convert.ToString(a.BpoOrgn)
                        }).DistinctBy(x => x.Text).OrderBy(x => x.Value).ToList();
            }
            else
            {
                List<SelectListItem> clients = new List<SelectListItem>();
                return clients;
            }
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

        public static List<SelectListItem> RegionCode()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "NORTHERN REGION";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "EASTERN REGION";
            single.Value = "E";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "WESTERN REGION";
            single.Value = "W";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "SOUTHERN REGION.";
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

        public static List<SelectListItem> StockNonstock()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Stock";
            single.Value = "S";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Non-Stock";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> PoOrLetter()
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

        public static List<SelectListItem> DiscountType()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Percentage";
            single.Value = "P";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Lumpsum";
            single.Value = "L";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Per No.";
            single.Value = "N";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
        }

        public static List<SelectListItem> ExciseType()
        {
            List<SelectListItem> textValueDropDownDTO = new List<SelectListItem>();
            SelectListItem single = new SelectListItem();
            single = new SelectListItem();
            single.Text = "Percentage";
            single.Value = "P";
            textValueDropDownDTO.Add(single);
            single = new SelectListItem();
            single.Text = "Lumpsum";
            single.Value = "L";
            textValueDropDownDTO.Add(single);
            return textValueDropDownDTO.ToList();
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

        public static List<SelectListItem> GetNCCode()
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> NCCODE = ModelContext.T69NcCodes
                                          .Select(a => new SelectListItem
                                          {
                                              Text = a.NcCd + " - " + a.NcDesc,
                                              Value = a.NcCd
                                          })
                                          .ToList();
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
            SelectListItem drop = new SelectListItem();
            drop.Text = "Other";
            drop.Value = "0";
            dropDownDTOs.Add(drop);
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
            else if (RlyNonrly != "")
            {
                List<SelectListItem> dropList = new List<SelectListItem>();
                dropList = (from a in ModelContext.T12BillPayingOfficers
                            where a.BpoType == Convert.ToString(RlyNonrly)
                            select
                       new SelectListItem
                       {
                           Text = Convert.ToString(a.BpoRly),
                           Value = Convert.ToString(a.BpoOrgn)
                       }).OrderBy(x => x.Text).ToList();
                dropDownDTOs.AddRange(dropList);
            }
            return dropDownDTOs.DistinctBy(x => x.Text).ToList();
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

        public static List<SelectListItem> Getfill_consignee_purcher(string RlyNonrlyValue, string RlyNonrlyText, string RlyCd)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropDownDTOs = new List<SelectListItem>();
            SelectListItem drop = new SelectListItem();
            drop.Text = "Other";
            drop.Value = "0";
            dropDownDTOs.Add(drop);
            if (RlyNonrlyText == "Railways")
            {
                List<SelectListItem> dropList = new List<SelectListItem>();
                dropList = (from a in ModelContext.T06Consignees
                            join b in ModelContext.T03Cities on a.ConsigneeCity equals b.CityCd
                            where a.ConsigneeFirm == Convert.ToString(RlyCd)
                            select
                       new SelectListItem
                       {
                           Text = Convert.ToString(a.ConsigneeCd + "-" + a.ConsigneeFirm + "/" + a.ConsigneeDesig + "/" + a.ConsigneeDept + "/" + a.ConsigneeAdd1 + "/" + b.Location + " : " + a.ConsigneeCity),
                           Value = Convert.ToString(a.ConsigneeCd)
                       }).ToList();
                dropDownDTOs.AddRange(dropList);
            }
            else if (RlyNonrlyText != "")
            {
                List<SelectListItem> dropList = new List<SelectListItem>();
                dropList = (from a in ModelContext.T06Consignees
                            join b in ModelContext.T03Cities on a.ConsigneeCity equals b.CityCd
                            where a.ConsigneeType == Convert.ToString(RlyNonrlyValue)
                            select
                       new SelectListItem
                       {
                           Text = Convert.ToString(a.ConsigneeCd + "-" + a.ConsigneeFirm + "/" + a.ConsigneeDesig + "/" + a.ConsigneeDept + "/" + a.ConsigneeAdd1 + "/" + b.Location + " : " + a.ConsigneeCity),
                           Value = Convert.ToString(a.ConsigneeCd)
                       }).ToList();
                dropDownDTOs.AddRange(dropList);
            }
            return dropDownDTOs;
        }

        public static List<SelectListItem> GetPurchaserCd(string? consignee)
        {
            ModelContext ModelContext = new(DbContextHelper.GetDbContextOptions());
            List<SelectListItem> dropDownDTOs = new List<SelectListItem>();
            List<SelectListItem> dropList = new List<SelectListItem>();
            dropList = (from a in ModelContext.V06Consignees
                        where a.Consignee.StartsWith(consignee)
                        select
                   new SelectListItem
                   {
                       Text = Convert.ToString(a.ConsigneeCd + "-" + a.Consignee),
                       Value = Convert.ToString(a.ConsigneeCd)
                   }).OrderBy(x => x.Text).ToList();
            if (dropList.Count > 0)
            {
                dropDownDTOs.AddRange(dropList);
            }
            SelectListItem drop = new SelectListItem();
            drop.Text = "Other";
            drop.Value = "0";
            dropDownDTOs.Add(drop);
            if (dropDownDTOs != null && dropDownDTOs.Count > 1)
            {
                dropDownDTOs[1].Selected = true;
            }
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
                                     VendStatus = m.VendStatus
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

            var obj = (from of in context.V12BillPayingOfficers
                       where of.Bpo.Contains(SBPO) || of.BpoCd.Contains(SBPO)
                       select of).ToList();


            List<SelectListItem> objdata = (from a in obj
                                            select
                                       new SelectListItem
                                       {
                                           Text = a.BpoCd + "-" + a.Bpo,
                                           Value = a.BpoCd
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
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model = JsonConvert.DeserializeObject<List<PO_MasterDetailsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
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
            ModelContext context = new(DbContextHelper.GetDbContextOptions());

            var obj = (from of in context.V06Consignees
                       where of.ConsigneeCd == ConsigneeSearch
                       select of).ToList();


            List<SelectListItem> objdata = (from a in obj
                                            select
                                       new SelectListItem
                                       {
                                           Text = a.ConsigneeCd + "-" + a.Consignee,
                                           Value = Convert.ToString(a.ConsigneeCd)
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

        public static IEnumerable<SelectListItem> GetFeeType()
        {
            IEnumerable<SelectListItem> item1 = new SelectListItem[] { new SelectListItem { Text = "Man days Basis", Value = "D" } };
            IEnumerable<SelectListItem> item2 = new SelectListItem[] { new SelectListItem { Text = "Hourly Basis", Value = "M" } };
            IEnumerable<SelectListItem> item3 = new SelectListItem[] { new SelectListItem { Text = "Lump sum", Value = "L" } };
            IEnumerable<SelectListItem> item4 = new SelectListItem[] { new SelectListItem { Text = "Percentage Basis", Value = "P" } };

            return item1.Concat(item2).Concat(item3).Concat(item4);
        }

        public static IEnumerable<SelectListItem> GetTaxType()
        {
            IEnumerable<SelectListItem> item1 = new SelectListItem[] { new SelectListItem { Text = "Fee Inclusive Service Tax", Value = "I" } };
            IEnumerable<SelectListItem> item2 = new SelectListItem[] { new SelectListItem { Text = "Service Tax Charged separately", Value = "X" } };
            IEnumerable<SelectListItem> item3 = new SelectListItem[] { new SelectListItem { Text = "No Service Tax(RITES Billing)", Value = "N" } };
            IEnumerable<SelectListItem> item4 = new SelectListItem[] { new SelectListItem { Text = "Fee Inclusive of Service Tax (Don't Print S.Tax in Bill)", Value = "D" } };

            return item1.Concat(item2).Concat(item3).Concat(item4);
        }

        public static IEnumerable<SelectListItem> GetTaxType_GST()
        {
            IEnumerable<SelectListItem> item1 = new SelectListItem[] { new SelectListItem { Text = "IGST @ 18%", Value = "I" } };
            IEnumerable<SelectListItem> item2 = new SelectListItem[] { new SelectListItem { Text = "CGST @ 9% & SGST @ 9%", Value = "C" } };
            IEnumerable<SelectListItem> item3 = new SelectListItem[] { new SelectListItem { Text = "NO GST", Value = "X" } };
            IEnumerable<SelectListItem> item4 = new SelectListItem[] { new SelectListItem { Text = "Fee Inclusive of IGST @ 18%", Value = "Y" } };
            IEnumerable<SelectListItem> item5 = new SelectListItem[] { new SelectListItem { Text = "Fee Inclusive of CGST @ 9% & SGST @ 9%", Value = "Z" } };

            return item1.Concat(item2).Concat(item3).Concat(item4).Concat(item5);
        }

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
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                property = Nullable.GetUnderlyingType(property.PropertyType).GetProperty(sortColumn);
            }

            var propertyAccess = Expression.MakeMemberAccess(parameter, property);

            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // this is the part p.SortColumn
            // var propertyAccess = Expression.MakeMemberAccess(parameter, property);

            // this is the part p =&gt; p.SortColumn
            // var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // finally, call the "OrderBy" / "OrderByDescending" method with the order by lamba expression
            resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { typeof(T), property.PropertyType },
               query.Expression, Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<T>(resultExpression);

        }

    }
}

