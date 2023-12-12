using Humanizer;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports.Billing;
using IBS.Models;
using IBS.Models.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;
using System.Globalization;
using static IBS.Helper.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.IO;
using Org.BouncyCastle.Asn1.Ocsp;

namespace IBS.Repositories.Reports.Billing
{
    public class BillRaisedRepository : IBillRaisedRepository
    {
        private readonly ModelContext context;

        public BillRaisedRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<BillRaisedModel> GetReportList(BillRaisedModel model)
        {
            model.BillSummary = model.BillSummary == "" ? string.Empty : model.BillSummary;
            model.Region = model.Region == "" ? string.Empty : model.Region;

            string SetFromMn = (Convert.ToInt32(model.FromMn) < 10) ? "0" + model.FromMn : Convert.ToString(model.FromMn);

            //string wHdr_YrMth_FR = SetFromMn + "-" + model.FromYr;
            string wYrMth_FR = Convert.ToString(model.FromYr + SetFromMn);
            //string wHdr_YrMth_TO = "";
            string wYrMth_TO = "";
            //string wRegion = "";

            //if (model.Region == "N")
            //{
            //    wRegion = "Northern Region";
            //}
            //else if (model.Region == "S") { wRegion = "Southern Region"; }
            //else if (model.Region == "E") { wRegion = "Eastern Region"; }
            //else if (model.Region == "W") { wRegion = "Western Region"; }
            //else if (model.Region == "C") { wRegion = "Central Region"; }
            //int ctr = 60;
            //string first_page = "Y";

            if (model.BillSummary == "M")
            {
                //wHdr_YrMth_TO = SetFromMn + "-" + model.FromYr;
                wYrMth_TO = Convert.ToString(model.FromYr + SetFromMn);
            }
            else if (model.BillSummary == "P")
            {
                //wHdr_YrMth_TO = model.ToMn + "-" + model.ToYr;
                wYrMth_TO = Convert.ToString(model.ToYr + model.ToMn);
            }

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("wYrMth_FR", OracleDbType.Varchar2, wYrMth_FR, ParameterDirection.Input);
            par[1] = new OracleParameter("wYrMth_TO", OracleDbType.Varchar2, wYrMth_TO, ParameterDirection.Input);
            par[2] = new OracleParameter("pRegion", OracleDbType.Varchar2, model.Region, ParameterDirection.Input);
            par[3] = new OracleParameter("ref_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_BILLRAISED_SUMMARY", par, 1);
            DataTable dt = ds.Tables[0];

            List<BillRaisedModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<BillRaisedModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            return list;
        }

        public BillRaisedModel GetBillingClient(int FromMn, int FromYr, int ToMn, int ToYr, string ActionType, string rdo, string Region)
        {
            BillRaisedModel model = new();
            List<BillRaisedListModel> lstBill = new();

            model.FromMn = FromMn;
            model.FromYr = FromYr;
            model.ToMn = ToMn;
            model.ToYr = ToYr;
            model.BillSummary = rdo;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            string SetFromMn = (Convert.ToInt32(model.FromMn) < 10) ? "0" + model.FromMn : Convert.ToString(model.FromMn);
            string SetToMn = (Convert.ToInt32(model.ToMn) < 10) ? "0" + model.ToMn : Convert.ToString(model.ToMn);
            string wYrMth_FR = Convert.ToString(model.FromYr + SetFromMn);
            string wYrMth_TO = "";



            if (model.BillSummary == "M")
            {
                DateTime dtDate1 = new DateTime(FromYr, FromMn, 1);
                model.FromMonthName = dtDate1.ToString("MMMM");
                wYrMth_TO = Convert.ToString(model.FromYr + SetFromMn);
            }
            else if (model.BillSummary == "P")
            {
                DateTime dtDate1 = new DateTime(FromYr, FromMn, 1);
                DateTime dtDate2 = new DateTime(ToYr, ToMn, 1);

                model.FromMonthName = dtDate1.ToString("MMMM");
                model.ToMonthName = dtDate2.ToString("MMMM");
                wYrMth_TO = Convert.ToString(model.ToYr + SetToMn);
            }

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("wYrMth_FR", OracleDbType.Varchar2, wYrMth_FR, ParameterDirection.Input);
            par[1] = new OracleParameter("wYrMth_TO", OracleDbType.Varchar2, wYrMth_TO, ParameterDirection.Input);
            par[2] = new OracleParameter("pRegion", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[3] = new OracleParameter("ref_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_BILLRAISED_SUMMARY", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstBill = JsonConvert.DeserializeObject<List<BillRaisedListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                if (ds.Tables.Count > 1)
                {
                    model.BPO_TYPE = Convert.ToString(ds.Tables[0].Rows[0]["BPO_TYPE"]);
                    model.INSP_FEE = Convert.ToInt32(ds.Tables[0].Rows[0]["INSP_FEE"]);
                    model.TAX = Convert.ToInt32(ds.Tables[0].Rows[0]["TAX"]);
                    model.BILL_AMOUNT = Convert.ToInt32(ds.Tables[0].Rows[0]["BILL_AMOUNT"]);
                    model.NO_OF_BILLS = Convert.ToInt32(ds.Tables[0].Rows[0]["NO_OF_BILLS"]);
                }


            }

            model.lstBill = lstBill;
            return model;
        }

        public BillRaisedModel GetBillingSector(int FromMn, int FromYr, int ToMn, int ToYr, string ActionType, string rdo, string Region, string IncRites)
        {
            BillRaisedModel model = new();
            List<BillSectorListModel> lstBill = new();
            List<BillSectorListModel> lstBillSector = new();

            model.FromMn = FromMn;
            model.FromYr = FromYr;
            model.ToMn = ToMn;
            model.ToYr = ToYr;
            model.BillSummary = rdo;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);
            model.IncRites = IncRites;

            string SetFromMn = (Convert.ToInt32(model.FromMn) < 10) ? "0" + model.FromMn : Convert.ToString(model.FromMn);
            string SetToMn = (Convert.ToInt32(model.ToMn) < 10) ? "0" + model.ToMn : Convert.ToString(model.ToMn);
            string wYrMth_FR = Convert.ToString(model.FromYr + SetFromMn);
            string wYrMth_TO = "";

            if (model.BillSummary == "M")
            {
                DateTime dtDate1 = new DateTime(FromYr, FromMn, 1);
                model.FromMonthName = dtDate1.ToString("MMMM");
                wYrMth_TO = Convert.ToString(model.FromYr + SetFromMn);
            }
            else if (model.BillSummary == "P")
            {
                DateTime dtDate1 = new DateTime(FromYr, FromMn, 1);
                DateTime dtDate2 = new DateTime(ToYr, ToMn, 1);

                model.FromMonthName = dtDate1.ToString("MMMM");
                model.ToMonthName = dtDate2.ToString("MMMM");
                wYrMth_TO = Convert.ToString(model.ToYr + SetToMn);
            }

            if (model.IncRites == "Y")
            {
                model.lstBillSector = (from V22 in context.V22aBillingSummaries
                                       where V22.RegionCode == Region
                                          && Convert.ToInt32(V22.BillingYrMth) >= Convert.ToInt32(wYrMth_FR)
                                          && Convert.ToInt32(V22.BillingYrMth) <= Convert.ToInt32(wYrMth_TO)
                                       group V22 by V22.Sector into V22Group
                                       select new BillSectorListModel
                                       {
                                           SECTOR = V22Group.Key == "6RITES" ? "RITES" : V22Group.Key == "2Private" ? "Private" : V22Group.Key == "3State Government" ? "State Government" : V22Group.Key == "5PSU" ? "PSU" : "Railway",
                                           INSP_FEE = V22Group.Sum(item => item.InspFee ?? 0),
                                           SERVICE_TAX = V22Group.Sum(item => item.ServiceTax ?? 0),
                                           EDU_CESS = V22Group.Sum(item => item.EduCess ?? 0),
                                           SHE_CESS = V22Group.Sum(item => item.SheCess ?? 0),
                                           SWACHH_BHARAT_CESS = V22Group.Sum(item => item.SwachhBharatCess ?? 0),
                                           KRISHI_KALYAN_CESS = V22Group.Sum(item => item.KrishiKalyanCess ?? 0),
                                           CGST = V22Group.Sum(item => item.Cgst ?? 0),
                                           SGST = V22Group.Sum(item => item.Sgst ?? 0),
                                           IGST = V22Group.Sum(item => item.Igst ?? 0),
                                           BILL_AMOUNT = V22Group.Sum(item => item.BillAmount ?? 0),
                                           NO_OF_BILLS = V22Group.Sum(item => item.NoOfBillls ?? 0)
                                       }).GroupBy(group => group.SECTOR).Select(x => new BillSectorListModel { SECTOR = x.Key, INSP_FEE = x.Sum(x => x.INSP_FEE), SERVICE_TAX = x.Sum(x => x.SERVICE_TAX), EDU_CESS = x.Sum(x => x.EDU_CESS), SHE_CESS = x.Sum(x => x.SHE_CESS), SWACHH_BHARAT_CESS = x.Sum(x => x.SWACHH_BHARAT_CESS), KRISHI_KALYAN_CESS = x.Sum(x => x.KRISHI_KALYAN_CESS), CGST = x.Sum(x => x.CGST), SGST = x.Sum(x => x.SGST), IGST = x.Sum(x => x.IGST), BILL_AMOUNT = x.Sum(x => x.BILL_AMOUNT), NO_OF_BILLS = x.Sum(x => x.NO_OF_BILLS) }).ToList();
                return model;
            }
            else
            {
                model.lstBillSector = (from V22 in context.V22aBillingSummaries
                                       where V22.RegionCode == Region
                                          && Convert.ToInt32(V22.BillingYrMth) >= Convert.ToInt32(wYrMth_FR)
                                          && Convert.ToInt32(V22.BillingYrMth) <= Convert.ToInt32(wYrMth_TO)
                                          && V22.Sector.Trim() != "6RITES"
                                       group V22 by V22.Sector into V22Group
                                       select new BillSectorListModel
                                       {
                                           SECTOR = V22Group.Key == "6RITES" ? "RITES" : V22Group.Key == "2Private" ? "Private" : V22Group.Key == "3State Government" ? "State Government" : V22Group.Key == "5PSU" ? "PSU" : "Railway",
                                           INSP_FEE = V22Group.Sum(item => item.InspFee ?? 0),
                                           SERVICE_TAX = V22Group.Sum(item => item.ServiceTax ?? 0),
                                           EDU_CESS = V22Group.Sum(item => item.EduCess ?? 0),
                                           SHE_CESS = V22Group.Sum(item => item.SheCess ?? 0),
                                           SWACHH_BHARAT_CESS = V22Group.Sum(item => item.SwachhBharatCess ?? 0),
                                           KRISHI_KALYAN_CESS = V22Group.Sum(item => item.KrishiKalyanCess ?? 0),
                                           CGST = V22Group.Sum(item => item.Cgst ?? 0),
                                           SGST = V22Group.Sum(item => item.Sgst ?? 0),
                                           IGST = V22Group.Sum(item => item.Igst ?? 0),
                                           BILL_AMOUNT = V22Group.Sum(item => item.BillAmount ?? 0),
                                           NO_OF_BILLS = V22Group.Sum(item => item.NoOfBillls ?? 0)
                                       }).GroupBy(group => group.SECTOR).Select(x => new BillSectorListModel { SECTOR = x.Key, INSP_FEE = x.Sum(x => x.INSP_FEE), SERVICE_TAX = x.Sum(x => x.SERVICE_TAX), EDU_CESS = x.Sum(x => x.EDU_CESS), SHE_CESS = x.Sum(x => x.SHE_CESS), SWACHH_BHARAT_CESS = x.Sum(x => x.SWACHH_BHARAT_CESS), KRISHI_KALYAN_CESS = x.Sum(x => x.KRISHI_KALYAN_CESS), CGST = x.Sum(x => x.CGST), SGST = x.Sum(x => x.SGST), IGST = x.Sum(x => x.IGST), BILL_AMOUNT = x.Sum(x => x.BILL_AMOUNT), NO_OF_BILLS = x.Sum(x => x.NO_OF_BILLS) }).ToList();
                return model;
            }
        }

        public BillRaisedModel GetRailwayOnline(string ClientType, string rdoSummary, string BpoRly, string rdoBpo, int FromMn, int FromYr, DateTime? FromDt, DateTime? ToDt, string ActionType, string Region, string chkRegion)
        {
            BillRaisedModel model = new();
            List<RailwayOnlineListModel> lstBillRailway = new();
            model.ClientType = ClientType;
            model.BillSummary = rdoSummary;
            model.BpoRly = BpoRly;
            model.BpoType = rdoBpo;

            model.FromDt = FromDt;
            model.ToDt = ToDt;
            model.ActionType = ActionType;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            string SetFromMn = (Convert.ToInt32(FromMn) < 10) ? "0" + FromMn : Convert.ToString(FromMn);
            string wYrMth = "";
            if (FromMn != 0 && FromYr != 0)
            {
                model.FromMn = FromMn;
                model.FromYr = FromYr;
                DateTime dtDate1 = new DateTime(FromYr, FromMn, 1);
                model.FromMonthName = dtDate1.ToString("MMMM");

                string wHdr_YrMth = model.FromMonthName + ", " + model.FromYr;
                wYrMth = Convert.ToString(model.FromYr + SetFromMn);
            }

            string from = "", to = "";
            to = Convert.ToString(model.ToDt);
            from = Convert.ToString(model.FromDt);
            if (from == "")
            {
                from = "01/04/2008";
            }
            if (to == "")
            {
                to = "31/12/2100";
            }
            from = dateconcate(from);
            to = dateconcate(to);
            int AllRegion = 0;
            if (chkRegion == "A")
            {
                AllRegion = 1;
            }

            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("p_BPO_Type", OracleDbType.Varchar2, model.ClientType, ParameterDirection.Input);
            par[1] = new OracleParameter("p_BPO_RLY", OracleDbType.Varchar2, model.BpoRly, ParameterDirection.Input);
            par[2] = new OracleParameter("p_DateType", OracleDbType.Varchar2, rdoSummary, ParameterDirection.Input);
            par[3] = new OracleParameter("p_WYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[4] = new OracleParameter("p_FromDate", OracleDbType.Varchar2, from, ParameterDirection.Input);
            par[5] = new OracleParameter("p_ToDate", OracleDbType.Varchar2, to, ParameterDirection.Input);
            par[6] = new OracleParameter("p_AllRegions", OracleDbType.Int16, AllRegion, ParameterDirection.Input);
            par[7] = new OracleParameter("p_RegionCode", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[8] = new OracleParameter("ref_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_BILLING_RAILWAY_DETAILS", par, 1);
            //if (rdoBpo == "A")
            //{
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstBillRailway = JsonConvert.DeserializeObject<List<RailwayOnlineListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                if (ds.Tables[0].Rows.Count > 1)
                {
                    model.BPO_RLY = Convert.ToString(ds.Tables[0].Rows[0]["BPO_RLY"]);
                    model.PO_NO = Convert.ToString(ds.Tables[0].Rows[0]["PO_NO"]);
                    model.PO_DT = Convert.ToString(ds.Tables[0].Rows[0]["PO_DT"]);
                    model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
                    model.PO_OR_LETTER = Convert.ToString(ds.Tables[0].Rows[0]["PO_OR_LETTER"]);
                    model.BPO_NAME = Convert.ToString(ds.Tables[0].Rows[0]["BPO_NAME"]);
                    model.BILL_NO = Convert.ToString(ds.Tables[0].Rows[0]["BILL_NO"]);
                    model.BILL_DT = Convert.ToString(ds.Tables[0].Rows[0]["BILL_DT"]);
                    model.INVOICE_NO = Convert.ToString(ds.Tables[0].Rows[0]["INVOICE_NO"]);
                    model.AU_DESC = Convert.ToString(ds.Tables[0].Rows[0]["AU_DESC"]);
                    model.IC_NO = Convert.ToString(ds.Tables[0].Rows[0]["IC_NO"]);
                    model.IC_DT = Convert.ToString(ds.Tables[0].Rows[0]["IC_DT"]);
                    model.BK_NO = Convert.ToString(ds.Tables[0].Rows[0]["BK_NO"]);
                    model.SET_NO = Convert.ToString(ds.Tables[0].Rows[0]["SET_NO"]);
                    model.BILL_AMOUNT = Convert.ToDecimal(ds.Tables[0].Rows[0]["BILL_AMOUNT"]);
                    model.AMOUNT_OUTSTANDING = Convert.ToString(ds.Tables[0].Rows[0]["AMOUNT_OUTSTANDING"]);
                    model.DIG_BILL_GEN_DATE = Convert.ToString(ds.Tables[0].Rows[0]["DIG_BILL_GEN_DATE"]);
                    model.ONLINE_INVOICE = Convert.ToString(ds.Tables[0].Rows[0]["ONLINE_INVOICE"]);
                    model.IC_PHOTO = Convert.ToString(ds.Tables[0].Rows[0]["IC_PHOTO"]);
                    model.PO_SOURCE = Convert.ToString(ds.Tables[0].Rows[0]["PO_SOURCE"]);
                    model.INVOICE_SUPP_DOCS = Convert.ToString(ds.Tables[0].Rows[0]["INVOICE_SUPP_DOCS"]);
                    model.PO_YR = Convert.ToString(ds.Tables[0].Rows[0]["PO_YR"]);
                    model.IMMS_RLY_CD = Convert.ToString(ds.Tables[0].Rows[0]["IMMS_RLY_CD"]);
                }

            }

            model.lstBillRailway = lstBillRailway;

            return model;
        }

        public BillRaisedModel GetBillsNotCris(DateTime FromDate, DateTime ToDate, string chkRegion, string ClientType, string lstAU, string actiontype, string Region, string rdbPRly, string rdbPAU)
        {
            BillRaisedModel model = new();
            List<BillsNotCrisListModel> lstBillCris = new();
            List<BillSubmittedCrisModel> lstBillCrisSubmitted = new();

            model.FromDt = FromDate;
            model.ToDt = ToDate;
            model.ClientType = ClientType;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);
            model.ActionType = actiontype;
            if (actiontype == "NSC")
            {
                OracleParameter[] par = new OracleParameter[7];
                par[0] = new OracleParameter("wFrmDt", OracleDbType.Varchar2, Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy"), ParameterDirection.Input);
                par[1] = new OracleParameter("wToDt", OracleDbType.Varchar2, Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy"), ParameterDirection.Input);
                par[2] = new OracleParameter("pClientType", OracleDbType.Varchar2, model.ClientType, ParameterDirection.Input);
                par[3] = new OracleParameter("pAU", OracleDbType.Varchar2, lstAU, ParameterDirection.Input);
                par[4] = new OracleParameter("pRegionType", OracleDbType.Varchar2, chkRegion, ParameterDirection.Input);
                par[5] = new OracleParameter("pRegion", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                par[6] = new OracleParameter("ref_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("SP_GET_BILLSNOTCRIS", par, 1);
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstBillCris = JsonConvert.DeserializeObject<List<BillsNotCrisListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    if (ds.Tables.Count > 1)
                    {
                        model.BILL_NO = Convert.ToString(ds.Tables[0].Rows[0]["BILL_NO"]);
                        model.BILL_DT = Convert.ToString(ds.Tables[0].Rows[0]["BILL_DT"]);
                        model.BPO_RLY = Convert.ToString(ds.Tables[0].Rows[0]["BPO_RLY"]);
                        model.BILL_AMOUNT = Convert.ToDecimal(ds.Tables[0].Rows[0]["BILL_AMOUNT"]);
                        model.AMT_CLEARED = Convert.ToInt32(ds.Tables[0].Rows[0]["AMT_CLEARED"]);
                        model.AMT_RECEIVED = Convert.ToInt32(ds.Tables[0].Rows[0]["AMT_RECEIVED"]);
                        model.AU = Convert.ToString(ds.Tables[0].Rows[0]["AU"]);
                        model.BILL_GEN_DATE = Convert.ToString(ds.Tables[0].Rows[0]["BILL_GEN_DATE"]);
                        model.INVOICE_NO = Convert.ToString(ds.Tables[0].Rows[0]["INVOICE_NO"]);
                        model.PO_OR_LETTER = Convert.ToString(ds.Tables[0].Rows[0]["PO_OR_LETTER"]);
                        model.IRN_NO = Convert.ToString(ds.Tables[0].Rows[0]["IRN_NO"]);
                    }

                }
                model.lstBillCris = lstBillCris;
            }
            else if (actiontype == "RBNRS")
            {

                OracleParameter[] par = new OracleParameter[7];
                par[0] = new OracleParameter("rdbPRly", OracleDbType.Varchar2, rdbPRly, ParameterDirection.Input);
                par[1] = new OracleParameter("rdbPAU", OracleDbType.Varchar2, rdbPAU, ParameterDirection.Input);
                par[2] = new OracleParameter("lstClientType", OracleDbType.Varchar2, ClientType, ParameterDirection.Input);
                par[3] = new OracleParameter("lstAU", OracleDbType.Varchar2, lstAU, ParameterDirection.Input);
                par[4] = new OracleParameter("chkAllRegions", OracleDbType.Varchar2, chkRegion, ParameterDirection.Input);
                par[5] = new OracleParameter("pRegion", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                par[6] = new OracleParameter("ref_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("SP_GET_BILLSCRISSUBMITTED", par, 1);
                DataTable dt = ds.Tables[0];
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstBillCrisSubmitted = JsonConvert.DeserializeObject<List<BillSubmittedCrisModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    if (dt != null && dt.Rows.Count > 1)
                    {
                        model.BILL_NO = Convert.ToString(ds.Tables[0].Rows[0]["BILL_NO"]);
                        model.BILL_DT = Convert.ToString(ds.Tables[0].Rows[0]["BILL_DT"]);
                        model.BPO_RLY = Convert.ToString(ds.Tables[0].Rows[0]["BPO_RLY"]);
                        model.BK_NO = Convert.ToString(ds.Tables[0].Rows[0]["BK_NO"]);
                        model.BILL_AMOUNT = Convert.ToDecimal(ds.Tables[0].Rows[0]["BILL_AMOUNT"]);
                        model.SET_NO = Convert.ToString(ds.Tables[0].Rows[0]["SET_NO"]);
                        model.BILL_RESENT_COUNT = Convert.ToInt32(ds.Tables[0].Rows[0]["BILL_RESENT_COUNT"]);
                        model.CO6_STATUS = Convert.ToString(ds.Tables[0].Rows[0]["CO6_STATUS"]);
                        model.RETURN_DT = Convert.ToString(ds.Tables[0].Rows[0]["RETURN_DT"]);
                        model.RETURN_REASON = Convert.ToString(ds.Tables[0].Rows[0]["RETURN_REASON"]);
                        model.AU = Convert.ToString(ds.Tables[0].Rows[0]["AU"]);
                    }

                }
                model.lstBillCrisSubmitted = lstBillCrisSubmitted;
            }
            return model;
        }

        string dateconcate(string dt)
        {
            string myYear, myMonth, myDay;

            myYear = dt.Substring(6, 4);
            myMonth = dt.Substring(3, 2);
            myDay = dt.Substring(0, 2);
            string dt1 = myYear + myMonth + myDay;
            return (dt1);
        }

    }
}
