using Humanizer;
using IBS.Controllers;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Vendor;
using IBS.Models;
using IBS.Models.Reports;
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

        public JIRequiredReport GetJIRequiredList(string FromDate, string ToDate, string AllCM, string AllIEs, string AllVendors, string AllClient, string AllConsignee, string Compact, string AwaitingJI, string JIConclusion, string JIConclusionfollowup,
            string JIconclusionreport, string JIDecidedDT, string All, string ParticularIEs, string IEWise, string CMWise, string VendorWise, string ClientWise, string ConsigneeWise, string FinancialYear, string ParticularCMs, string ParticularClients, string ParticularConsignee,
            string ParticularVendor, string Detailed, string FinancialYears, string ddlsupercm, string ddliename, string Clientwiseddl, string vendor, string Item, string consignee,string Region)
        {
            JIRequiredReport model = new();
            List<JIRequiredList> lstJIRequiredList = new();
            // List<IEPerformanceSummaryListModel> lstPerformanceSummaryList = new();
            DataSet ds = null;
            DataTable dt = new DataTable();

            model.FromDate = FromDate;model.ToDate = ToDate;model.AllCM = AllCM;model.AllIEs = AllIEs;model.AllVendors = AllVendors;model.AllClient = AllClient;model.AllConsignee = AllConsignee;
            model.Compact = Compact;model.AwaitingJI = AwaitingJI;model.JIConclusion = JIConclusion;model.JIConclusionfollowup = JIConclusionfollowup;model.JIconclusionreport = JIconclusionreport;model.JIDecidedDT = JIDecidedDT;
            model.All = All;model.ParticularIEs = ParticularIEs;model.IEWise = IEWise;model.CMWise = CMWise;model.VendorWise = VendorWise;model.ClientWise = ClientWise;model.ConsigneeWise = ConsigneeWise;
            model.FinancialYear = FinancialYear;model.ParticularCMs = ParticularCMs;model.ParticularClients = ParticularClients;model.ParticularConsignee = ParticularConsignee;model.ParticularVendor = ParticularVendor;model.Detailed = Detailed;
            model.FinancialYears = FinancialYears;model.ddlsupercm = ddlsupercm;model.ddliename = ddliename;model.Clientwiseddl = Clientwiseddl;model.vendor = vendor;model.Item = Item;model.consignee = consignee;

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
                    ds = compliants_statement_ConsigneeWise(FromDate, ToDate, Region, FinancialYear, JIDecidedDT, AllConsignee, ParticularConsignee, consignee, FinancialYears);
                }
            }
            else
            {
                ds = ji_compliants_statement(FromDate, ToDate, Region, FinancialYear, JIDecidedDT, Compact, AwaitingJI, JIConclusion, JIConclusionfollowup, All, Detailed, FinancialYears, consignee, ddlsupercm, ddliename, vendor, Clientwiseddl, Item);
            }

            dt = ds.Tables[0];
            List<JIRequiredList> list = dt.AsEnumerable().Select(row => new JIRequiredList
            {
                IE = row.Field<string>("Name"),
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
                Region =Region,
                FromDate = Convert.ToDateTime(FromDate),
                ToDate = Convert.ToDateTime(ToDate),

            }).ToList();

            foreach (var item in list)
            {
                item.Total = item.UPHELD + item.SORTING + item.RECTIFICATION + item.PRICE_REDUCTION + item.LIFTED_BEFORE_JI;
            }
            model.lstJIRequiredList = list;
            return model;
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

        public DataSet compliants_statement_ConsigneeWise(string FromDate,string ToDate,string Region,string FinancialYear,string JIDecidedDT,string AllConsignee,string ParticularConsignee,string consignee,string FinancialYears)
        {
            DataSet ds = null;
            if (Convert.ToBoolean(JIDecidedDT) == true)
            {
                if (Convert.ToBoolean(AllConsignee) == true)
                {
                    ds = ALLConsignee(FromDate, ToDate, Region, consignee, FinancialYears);
                }
                else if (Convert.ToBoolean(ParticularConsignee) == true)
                {
                    ds = ALLConsignee(FromDate, ToDate, Region, consignee, FinancialYears);
                }
            }
            else if (Convert.ToBoolean(FinancialYear) == true)
            {
                if (Convert.ToBoolean(AllConsignee) == true)
                {
                    ds = ALLConsignee(FromDate, ToDate, Region, consignee, FinancialYears);
                }
                else if (Convert.ToBoolean(ParticularConsignee) == true)
                {
                    ds = ALLConsignee(FromDate, ToDate, Region, consignee, FinancialYears);
                }
            }
            return ds;
        }

        public DataSet ji_compliants_statement(string FromDate,string ToDate,string Region,string FinancialYear,string JIDecidedDT,string Compact,string AwaitingJI,string JIConclusion,string JIConclusionfollowup,string All,string Detailed,string FinancialYears,string consignee,string ddlsupercm,string ddliename,string vendor,string Clientwiseddl,string Item)
        {
            DataSet ds = null;
            if (Convert.ToBoolean(JIDecidedDT) == true)
            {
                if (Convert.ToBoolean(Compact) == true)
                {
                    ds = ALLReportTye(FromDate, ToDate, Region, AwaitingJI, JIConclusion, JIConclusionfollowup, All, FinancialYears, consignee, ddlsupercm, ddliename, vendor, Clientwiseddl, Item);
                }
                else if (Convert.ToBoolean(Detailed) == true)
                {
                    ds = ALLReportTye(FromDate, ToDate, Region, AwaitingJI, JIConclusion, JIConclusionfollowup, All, FinancialYears, consignee, ddlsupercm, ddliename, vendor, Clientwiseddl, Item);
                }
            }
            else if (Convert.ToBoolean(FinancialYear) == true)
            {
                if (Convert.ToBoolean(Compact) == true)
                {
                    ds = ALLReportTye(FromDate, ToDate, Region, AwaitingJI, JIConclusion, JIConclusionfollowup, All, FinancialYears, consignee, ddlsupercm, ddliename, vendor, Clientwiseddl, Item);
                }
                else if (Convert.ToBoolean(Detailed) == true)
                {
                    ds = ALLReportTye(FromDate, ToDate, Region, AwaitingJI, JIConclusion, JIConclusionfollowup, All, FinancialYears, consignee, ddlsupercm, ddliename, vendor, Clientwiseddl, Item);
                }
            }
            return ds;
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
        
        public DataSet ALLConsignee(string FromDate,string ToDate,string Region,string consignee,string FinancialYears)
        {
            DataSet ds = null;
            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_todt", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_consignee_cd", OracleDbType.Varchar2, consignee, ParameterDirection.Input);
            par[3] = new OracleParameter("p_finyear", OracleDbType.Varchar2, FinancialYears, ParameterDirection.Input);
            par[4] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[5] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_statement_AllConsignee_Report", par, 1);
            return ds;
        }
        
        public DataSet ALLReportTye(string FromDate,string ToDate,string Region,string AwaitingJI,string JIConclusion,string JIConclusionfollowup,string All,string FinancialYears, string consignee, string ddlsupercm, string ddliename, string vendor, string Clientwiseddl, string Item)
        {
            DataSet ds = null;
            OracleParameter[] par = new OracleParameter[16];
            par[0] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_todt", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_co", OracleDbType.Varchar2, ddlsupercm, ParameterDirection.Input);
            par[3] = new OracleParameter("p_ie", OracleDbType.Varchar2, ddliename, ParameterDirection.Input);
            par[4] = new OracleParameter("p_vend_cd", OracleDbType.Varchar2, vendor, ParameterDirection.Input);
            par[5] = new OracleParameter("p_consignee_cd", OracleDbType.Varchar2, consignee, ParameterDirection.Input);
            par[6] = new OracleParameter("p_today_dt", OracleDbType.Varchar2, DateTime.Now, ParameterDirection.Input);
            par[7] = new OracleParameter("p_finyear", OracleDbType.Varchar2, FinancialYears, ParameterDirection.Input);
            par[8] = new OracleParameter("p_bpo_rly", OracleDbType.Varchar2, Item, ParameterDirection.Input);
            par[9] = new OracleParameter("p_clienttype", OracleDbType.Varchar2, Clientwiseddl, ParameterDirection.Input);
            par[10] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[11] = new OracleParameter("p_awaitingji", OracleDbType.Varchar2, AwaitingJI, ParameterDirection.Input);
            par[12] = new OracleParameter("p_awaitingconclusion", OracleDbType.Varchar2, JIConclusion, ParameterDirection.Input);
            par[13] = new OracleParameter("p_awaitingaction", OracleDbType.Varchar2, JIConclusionfollowup, ParameterDirection.Input);
            par[14] = new OracleParameter("p_awaitingfinalaction", OracleDbType.Varchar2, All, ParameterDirection.Input);
            par[15] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_statement_reporttypes_Report", par, 1);
            return ds;
        }

    }
}
