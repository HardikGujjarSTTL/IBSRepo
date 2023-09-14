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
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

            string wHdr_YrMth_FR = SetFromMn + "-" + model.FromYr;
            string wYrMth_FR = Convert.ToString(model.FromYr + SetFromMn);
            string wHdr_YrMth_TO = "";
            string wYrMth_TO = "";
            string wRegion = "";

            if (model.Region == "N")
            {
                wRegion = "Northern Region";
            }
            else if (model.Region == "S") { wRegion = "Southern Region"; }
            else if (model.Region == "E") { wRegion = "Eastern Region"; }
            else if (model.Region == "W") { wRegion = "Western Region"; }
            else if (model.Region == "C") { wRegion = "Central Region"; }
            int ctr = 60;
            string first_page = "Y";

            if (model.BillSummary == "M")
            {
                wHdr_YrMth_TO = SetFromMn + "-" + model.FromYr;
                wYrMth_TO = Convert.ToString(model.FromYr + SetFromMn);
            }
            else if (model.BillSummary == "P")
            {
                wHdr_YrMth_TO = model.ToMn + "-" + model.ToYr;
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

                model.BPO_TYPE = Convert.ToString(ds.Tables[0].Rows[0]["BPO_TYPE"]);
                model.INSP_FEE = Convert.ToInt32(ds.Tables[0].Rows[0]["INSP_FEE"]);
                model.TAX = Convert.ToInt32(ds.Tables[0].Rows[0]["TAX"]);
                model.BILL_AMOUNT = Convert.ToInt32(ds.Tables[0].Rows[0]["BILL_AMOUNT"]);
                model.NO_OF_BILLS = Convert.ToInt32(ds.Tables[0].Rows[0]["NO_OF_BILLS"]);

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

    }
}
