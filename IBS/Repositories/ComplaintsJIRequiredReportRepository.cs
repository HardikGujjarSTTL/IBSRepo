using Humanizer;
using IBS.Controllers;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Dynamic;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories
{
    public class ComplaintsJIRequiredReportRepository : IComplaintsJIRequiredReportRepository
    {
        private readonly ModelContext context;

        public ComplaintsJIRequiredReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public string GetItems(string Clientwise)
        {
            List<SelectListItem> qry = null;
            if (Clientwise == "R")
            {
                 qry = (from a in context.T91Railways
                            select new SelectListItem
                            {
                                Text = Convert.ToString(a.Railway),
                                Value = Convert.ToString(a.RlyCd)
                            }).OrderBy(c => c.Value).ToList();
            }
            else
            {
                 qry = (from bpo in context.T12BillPayingOfficers
                            where bpo.BpoType == Clientwise
                        select new SelectListItem
                            {
                                Text = bpo.BpoOrgn,
                                Value = bpo.BpoRly.Trim().ToUpper()
                            })
                         .Distinct()
                         .OrderBy(item => item.Value)
                         .ToList();

            }
            string json = JsonConvert.SerializeObject(qry, Formatting.Indented);
            return json;
        }

        public DTResult<JIRequiredReport> GetJIRequiredList(DTParameters dtParameters, string Region)
        {
            DTResult<JIRequiredReport> dTResult = new() { draw = 0 };
            IQueryable<JIRequiredReport>? query = null;
            DataTable dt = new DataTable();
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            DataSet ds = null;
            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "IE";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "IE";
                orderAscendingDirection = true;
            }

            string AllCM = "", AllIEs = "", AllVendors = "", AllClient = "", AllConsignee = "", Compact = "", AwaitingJI = "", JIConclusion = "", JIConclusionfollowup = "",
            JIconclusionreport = "", All = "", FinancialYear = "", ParticularClients="", IEWise ="", ParticularCMs="", Clientwiseddl="", VendorWise ="", ParticularVendor="", FinancialYears ="", ConsigneeWise ="", ClientWise ="", CMWise ="", FromDate = null, ToDate = null, ParticularIEs="", JIDecidedDT ="", ddlsupercm = "", ddliename = "", vendor = "", Item = "", consignee = "";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["AllCM"]))
            {
                AllCM = Convert.ToString(dtParameters.AdditionalValues["AllCM"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["AllIEs"]))
            {
                AllIEs = Convert.ToString(dtParameters.AdditionalValues["AllIEs"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["AllVendors"]))
            {
                AllVendors = Convert.ToString(dtParameters.AdditionalValues["AllVendors"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["AllClient"]))
            {
                AllClient = Convert.ToString(dtParameters.AdditionalValues["AllClient"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["AllConsignee"]))
            {
                AllConsignee = Convert.ToString(dtParameters.AdditionalValues["AllConsignee"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Compact"]))
            {
                Compact = Convert.ToString(dtParameters.AdditionalValues["Compact"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["AwaitingJI"]))
            {
                AwaitingJI = Convert.ToString(dtParameters.AdditionalValues["AwaitingJI"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["JIConclusion"]))
            {
                JIConclusion = Convert.ToString(dtParameters.AdditionalValues["JIConclusion"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["JIConclusionfollowup"]))
            {
                JIConclusionfollowup = Convert.ToString(dtParameters.AdditionalValues["JIConclusionfollowup"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["JIconclusionreport"]))
            {
                JIconclusionreport = Convert.ToString(dtParameters.AdditionalValues["JIconclusionreport"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["All"]))
            {
                All = Convert.ToString(dtParameters.AdditionalValues["All"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FinancialYear"]))
            {
                FinancialYear = Convert.ToString(dtParameters.AdditionalValues["FinancialYear"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
            {
                FromDate = Convert.ToString(dtParameters.AdditionalValues["FromDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
            {
                ToDate = Convert.ToString(dtParameters.AdditionalValues["ToDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ddlsupercm"]))
            {
                ddlsupercm = Convert.ToString(dtParameters.AdditionalValues["ddlsupercm"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ddliename"]))
            {
                ddliename = Convert.ToString(dtParameters.AdditionalValues["ddliename"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["vendor"]))
            {
                vendor = Convert.ToString(dtParameters.AdditionalValues["vendor"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Item"]))
            {
                Item = Convert.ToString(dtParameters.AdditionalValues["Item"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["consignee"]))
            {
                consignee = Convert.ToString(dtParameters.AdditionalValues["consignee"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["JIDecidedDT"]))
            {
                JIDecidedDT = Convert.ToString(dtParameters.AdditionalValues["JIDecidedDT"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ParticularIEs"]))
            {
                ParticularIEs = Convert.ToString(dtParameters.AdditionalValues["ParticularIEs"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IEWise"]))
            {
                IEWise = Convert.ToString(dtParameters.AdditionalValues["IEWise"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CMWise"]))
            {
                CMWise = Convert.ToString(dtParameters.AdditionalValues["CMWise"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["VendorWise"]))
            {
                VendorWise = Convert.ToString(dtParameters.AdditionalValues["VendorWise"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ClientWise"]))
            {
                ClientWise = Convert.ToString(dtParameters.AdditionalValues["ClientWise"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ConsigneeWise"]))
            {
                ConsigneeWise = Convert.ToString(dtParameters.AdditionalValues["ConsigneeWise"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FinancialYears"]))
            {
                FinancialYears = Convert.ToString(dtParameters.AdditionalValues["FinancialYears"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ParticularVendor"]))
            {
                ParticularVendor = Convert.ToString(dtParameters.AdditionalValues["ParticularVendor"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ParticularCMs"]))
            {
                ParticularCMs = Convert.ToString(dtParameters.AdditionalValues["ParticularCMs"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Clientwiseddl"]))
            {
                Clientwiseddl = Convert.ToString(dtParameters.AdditionalValues["Clientwiseddl"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ParticularClients"]))
            {
                ParticularClients = Convert.ToString(dtParameters.AdditionalValues["ParticularClients"]);
            }

            if (Convert.ToBoolean(Compact) == true)
            {
                if (Convert.ToBoolean(IEWise) == true)
                {
                    ds = compliants_statement_IEWise(FromDate, ToDate, Region, FinancialYear, JIDecidedDT, ParticularIEs, AllIEs, ddliename, FinancialYears);
                }
                else if (Convert.ToBoolean(VendorWise) == true)
                {
                    ds = compliants_statement_VendorWise(FromDate, ToDate, Region, FinancialYear, JIDecidedDT, AllVendors, ParticularVendor, vendor, FinancialYears);
                }
                else if (Convert.ToBoolean(CMWise) == true)
                {
                    ds = compliants_statement_CMWise(FromDate, ToDate, Region, FinancialYear, JIDecidedDT, AllCM, ParticularCMs, ddlsupercm, FinancialYears);
                }
                else if (Convert.ToBoolean(ClientWise) == true)
                {
                    ds = compliants_statement_ClientWise(FromDate, ToDate, Region, FinancialYear, JIDecidedDT, AllClient, ParticularClients, Clientwiseddl, FinancialYears);
                }
                else if (Convert.ToBoolean(ConsigneeWise) == true)
                {
                    compliants_statement_ConsigneeWise();
                }
            }
            else 
            {
                ji_compliants_statement1();
            }
            dt = ds.Tables[0];
            List<JIRequiredReport> list = dt.AsEnumerable().Select(row => new JIRequiredReport
            {
                IE = row.Field<string>("IE"),
                NO_OF_INSPECTION = Convert.ToInt32(row.Field<decimal>("NO_OF_INSPECTION")),
                MATERIAL_VALUE = row.Field<decimal>("MATERIAL_VALUE"),
                RECD = Convert.ToInt32(row.Field<decimal>("RECD")),
                FINALISED = Convert.ToInt32(row.Field<decimal>("FINALISED")),
                PENDING = Convert.ToInt32(row.Field<decimal>("PENDING")),
                ACCEPTED = Convert.ToInt32(row.Field<decimal>("ACCEPTED")),

                UPHELD = Convert.ToInt32(row.Field<decimal>("UPHELD")),
                SORTING = Convert.ToInt32(row.Field<decimal>("SORTING")),
                RECTIFICATION = Convert.ToInt32(row.Field<decimal>("RECTIFICATION")),
                PRICE_REDUCTION = Convert.ToInt32(row.Field<decimal>("PRICE_REDUCTION")),
                LIFTED_BEFORE_JI = Convert.ToInt32(row.Field<decimal>("LIFTED_BEFORE_JI")),

                TRANSIT_DEMAGE = Convert.ToInt32(row.Field<decimal>("TRANSIT_DEMAGE")),
                UNSTAMPED = Convert.ToInt32(row.Field<decimal>("UNSTAMPED")),
                NOT_ON_RITES_AC = Convert.ToInt32(row.Field<decimal>("NOT_ON_RITES_AC")),

             }).ToList();

            foreach (var item in list)
            {
                item.Total = item.UPHELD + item.SORTING + item.RECTIFICATION + item.PRICE_REDUCTION + item.LIFTED_BEFORE_JI;
            }

            query = list.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.IE).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public DataSet compliants_statement_IEWise(string FromDateFor,string ToDateFor, string Region,string FinancialYear, string JIDecidedDT, string ParticularIEs, string AllIEs, string ddliename,string FinancialYears)
        {
            DataSet ds = null;
            if (Convert.ToBoolean(JIDecidedDT) == true)
            {
                if (Convert.ToBoolean(AllIEs) == true)
                {
                    ds = AllIE(FromDateFor, ToDateFor, Region, ddliename, FinancialYears);
                }
                else if(Convert.ToBoolean(ParticularIEs) == true)
                {
                    ds = AllIE(FromDateFor, ToDateFor, Region, ddliename, FinancialYears);
                }
            }
            else if (Convert.ToBoolean(FinancialYear) == true)
            {
                if (Convert.ToBoolean(AllIEs) == true)
                {
                    ds = AllIE(FromDateFor, ToDateFor, Region, ddliename, FinancialYears);
                }
                else if (Convert.ToBoolean(ParticularIEs) == true)
                {
                    ds = AllIE(FromDateFor, ToDateFor, Region, ddliename, FinancialYears);
                }
            }
            return ds;
        }

        public DataSet compliants_statement_VendorWise(string FromDate,string ToDate,string Region,string FinancialYear,string JIDecidedDT,string AllVendors,string ParticularVendor,string vendor,string FinancialYears)
        {
            DataSet ds = null;
            if (Convert.ToBoolean(JIDecidedDT) == true)
            {
                if (Convert.ToBoolean(AllVendors) == true)
                {
                    ds = AllVendor(FromDate, ToDate, Region, vendor, FinancialYears);
                }
                else if (Convert.ToBoolean(ParticularVendor) == true)
                {
                    ds = AllVendor(FromDate, ToDate, Region, vendor, FinancialYears);
                }
            }
            else if (Convert.ToBoolean(FinancialYear) == true)
            {
                if (Convert.ToBoolean(AllVendors) == true)
                {
                    ds = AllVendor(FromDate, ToDate, Region, vendor, FinancialYears);
                }
                else if (Convert.ToBoolean(ParticularVendor) == true)
                {
                    ds = AllVendor(FromDate, ToDate, Region, vendor, FinancialYears);
                }
            }
            return ds;
        }

        public DataSet compliants_statement_CMWise(string FromDate,string ToDate,string Region,string FinancialYear,string JIDecidedDT,string AllCM,string ParticularCMs,string ddlsupercm, string FinancialYears)
        {
            DataSet ds = null;
            if (Convert.ToBoolean(JIDecidedDT) == true)
            {
                if (Convert.ToBoolean(AllCM) == true)
                {
                    ds = ALLCM(FromDate, ToDate, Region, ddlsupercm, FinancialYears);
                }
                else if (Convert.ToBoolean(ParticularCMs) == true)
                {
                    ds = ALLCM(FromDate, ToDate, Region, ddlsupercm, FinancialYears);
                }
            }
            else if (Convert.ToBoolean(FinancialYear) == true)
            {
                if (Convert.ToBoolean(AllCM) == true)
                {
                    ds = ALLCM(FromDate, ToDate, Region, ddlsupercm, FinancialYears);
                }
                else if (Convert.ToBoolean(ParticularCMs) == true)
                {
                    ds = ALLCM(FromDate, ToDate, Region, ddlsupercm, FinancialYears);
                }
            }
            return ds;
        }

        public DataSet compliants_statement_ClientWise(string FromDate, string ToDate, string Region, string FinancialYear, string JIDecidedDT, string AllClient, string ParticularClients, string Clientwiseddl, string FinancialYears)
        {
            DataSet ds = null;
            if (Convert.ToBoolean(JIDecidedDT) == true)
            {
                if (Convert.ToBoolean(AllClient) == true)
                {
                    ds = ALLClient(FromDate, ToDate, Region, Clientwiseddl, FinancialYears);
                }
                else if (Convert.ToBoolean(ParticularClients) == true)
                {
                    ds = ALLClient(FromDate, ToDate, Region, Clientwiseddl, FinancialYears);
                }
            }
            else if (Convert.ToBoolean(FinancialYear) == true)
            {
                if (Convert.ToBoolean(AllClient) == true)
                {
                    ds = ALLClient(FromDate, ToDate, Region, Clientwiseddl, FinancialYears);
                }
                else if (Convert.ToBoolean(ParticularClients) == true)
                {
                    ds = ALLClient(FromDate, ToDate, Region, Clientwiseddl, FinancialYears);
                }
            }
            return ds;
        }

        public void compliants_statement_ConsigneeWise()
        {

        }
        public void ji_compliants_statement1()
        {

        }

        public DataSet AllIE(string FromDateFor, string ToDateFor, string Region, string ddliename, string FinancialYears)
        {
            DataSet ds = null;
            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, FromDateFor, ParameterDirection.Input);
            par[1] = new OracleParameter("p_todt", OracleDbType.Varchar2, ToDateFor, ParameterDirection.Input);
            par[2] = new OracleParameter("p_ie_cd", OracleDbType.Varchar2, ddliename, ParameterDirection.Input);
            par[3] = new OracleParameter("p_finyear", OracleDbType.Varchar2, FinancialYears, ParameterDirection.Input);
            par[4] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[5] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_statement_AllIE_Date", par, 1);
            return ds;
        }

        public DataSet AllVendor(string FromDate, string ToDate, string Region, string vendor,string FinancialYears)
        {
            DataSet ds = null;
            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_todt", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_vend_cd", OracleDbType.Varchar2, vendor, ParameterDirection.Input);
            par[3] = new OracleParameter("p_finyear", OracleDbType.Varchar2, FinancialYears, ParameterDirection.Input);
            par[4] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[5] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_statement_AllVendor_Report", par, 1);
            return ds;
        }
        
        public DataSet ALLCM(string FromDate,string ToDate,string Region,string ddlsupercm,string FinancialYears)
        {
            DataSet ds = null;
            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_todt", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_CO", OracleDbType.Varchar2, ddlsupercm, ParameterDirection.Input);
            par[3] = new OracleParameter("p_finyear", OracleDbType.Varchar2, FinancialYears, ParameterDirection.Input);
            par[4] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[5] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_statement_AllCM_Report", par, 1);
            return ds;
        }
        
        public DataSet ALLClient(string FromDate,string ToDate,string Region,string Clientwiseddl,string FinancialYears)
        {
            DataSet ds = null;
            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_todt", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_bpo_rly", OracleDbType.Varchar2, Clientwiseddl, ParameterDirection.Input);
            par[3] = new OracleParameter("p_finyear", OracleDbType.Varchar2, FinancialYears, ParameterDirection.Input);
            par[4] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[5] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_statement_AllClient_Report", par, 1);
            return ds;
        }

    }
}
