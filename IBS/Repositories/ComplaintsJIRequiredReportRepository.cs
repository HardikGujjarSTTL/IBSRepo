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

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            DataSet ds = null;
            //if (dtParameters.Order != null)
            //{
            //    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

            //    if (orderCriteria == "")
            //    {
            //        orderCriteria = "USER_NAME";
            //    }
            //    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            //}
            //else
            //{
            //    orderCriteria = "USER_NAME";
            //    orderAscendingDirection = true;
            //}

            string AllCM = "", AllIEs = "", AllVendors = "", AllClient = "", AllConsignee = "", Compact = "", AwaitingJI = "", JIConclusion = "", JIConclusionfollowup = "",
            JIconclusionreport = "", All = "", FinancialYear = "", FromDate = "", ToDate = "", ddlsupercm = "", ddliename = "", vendor = "", Item = "", consignee = "";

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

            if (Convert.ToBoolean(Compact) == true)
            {
                if (Convert.ToBoolean(AllIEs) == true)
                {
                    ds = compliants_statement_IEWise(FromDate, ToDate, Region);
                }
                else if (Convert.ToBoolean(vendor) == true)
                {
                    compliants_statement_VendorWise();
                }
                else if (Convert.ToBoolean(AllCM) == true)
                {
                    compliants_statement_CMWise();
                }
                else if (Convert.ToBoolean(AllClient) == true)
                {
                    compliants_statement_ClientWise();
                }
                else if (Convert.ToBoolean(AllConsignee) == true)
                {
                    compliants_statement_ConsigneeWise();
                }
            }
            else 
            {
                ji_compliants_statement1();
            }

            List<JIRequiredReport> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<JIRequiredReport>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
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

        public DataSet compliants_statement_IEWise(string FromDate,string ToDate,string Region)
        {

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_todt", OracleDbType.Date, ToDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_region", OracleDbType.Date, Region, ParameterDirection.Input);
            par[2] = new OracleParameter("RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            return DataAccessDB.GetDataSet("compliants_statement_AllIE_Date", par, 1);

        }
        public void compliants_statement_VendorWise()
        {

        }
        public void compliants_statement_CMWise()
        {

        }
        public void compliants_statement_ClientWise()
        {

        }
        public void compliants_statement_ConsigneeWise()
        {

        }
        public void ji_compliants_statement1()
        {

        }
    }
}
