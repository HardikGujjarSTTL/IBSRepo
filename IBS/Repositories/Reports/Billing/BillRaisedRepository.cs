using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports.Billing;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.Reports.Billing
{
    public class BillRaisedRepository : IBillRaisedRepository
    {
        private readonly ModelContext context;

        public BillRaisedRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<BillRaisedModel> GetBillingData(BillRaisedModel model)
        {
            List<BillRaisedModel> dTResult = new();

            string wHdr_YrMth_FR = model.FromMn + "-" + model.FromYr;
            string wYrMth_FR = Convert.ToString(model.FromYr + model.FromMn);
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
                wHdr_YrMth_TO = model.FromMn + "-" + model.FromYr;
                wYrMth_TO = Convert.ToString(model.FromYr + model.FromMn);
            }
            else if (model.BillSummary == "P")
            {
                wHdr_YrMth_TO = model.ToMn + "-" + model.ToYr;
                wYrMth_TO = Convert.ToString(model.ToYr + model.ToMn);
            }

            var query = from bill in context.V22Bills
                        where Convert.ToDateTime(bill.BillDt).ToString("yyyyMMdd") == "20220101" &&
                              bill.RegionCode == "N" &&
                              !bill.BpoRly.ToUpper().Contains("RITES")
                        group bill by new
                        {
                            BPOTYPE = bill.BpoType == "R" ? "1Railways" :
                                      bill.BpoType == "U" ? "2PSU" :
                                      bill.BpoType == "P" ? "3Private" :
                                      bill.BpoType == "F" ? "4Foreign Railway" :
                                      bill.BpoType == "S" ? "5State Government" : "6RITES",
                            bill.BpoRly,
                            bill.BpoOrgn
                        } into grouped
                        select new
                        {
                            BPO_TYPE = grouped.Key.BPOTYPE.Substring(1),
                            BPO_RLY = grouped.Key.BpoRly,
                            BPO_ORGN = grouped.Key.BpoOrgn,
                            INSP_FEE = grouped.Sum(bill => bill.InspFee ?? 0),
                            TAX = grouped.Sum(bill => bill.BillAmount - (bill.InspFee ?? 0)),
                            BILL_AMOUNT = grouped.Sum(bill => bill.BillAmount),
                            NO_OF_BILLS = grouped.Count()
                        };

            var result = query.OrderBy(item => item.BPO_TYPE)
                             .ThenBy(item => item.BPO_RLY)
                             .ThenBy(item => item.BPO_ORGN)
                             .ToList();


            return dTResult;
        }

        public DTResult<BillRaisedModel> GetDataList(DTParameters dtParameters)
        {

            DTResult<BillRaisedModel> dTResult = new() { draw = 0 };
            IQueryable<BillRaisedModel>? query = null;

            var searchBy = dtParameters.Search?.Value;

            string FromMn = "", FromYr = "", ToMn = "", ToYr = "", rdo = "", Region = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromMn"]))
            {
                FromMn = Convert.ToString(dtParameters.AdditionalValues["FromMn"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromYr"]))
            {
                FromYr = Convert.ToString(dtParameters.AdditionalValues["FromYr"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToMn"]))
            {
                ToMn = Convert.ToString(dtParameters.AdditionalValues["ToMn"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToYr"]))
            {
                ToYr = Convert.ToString(dtParameters.AdditionalValues["ToYr"]);
            }

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["rdo"]))
            {
                rdo = Convert.ToString(dtParameters.AdditionalValues["rdo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]))
            {
                Region = Convert.ToString(dtParameters.AdditionalValues["Region"]);
            }

            FromMn = FromMn.ToString() == "" ? string.Empty : FromMn.ToString();
            FromYr = FromYr.ToString() == "" ? string.Empty : FromYr.ToString();
            ToMn = ToMn.ToString() == "" ? string.Empty : ToMn.ToString();
            ToYr = ToYr.ToString() == "" ? string.Empty : ToYr.ToString();

            rdo = rdo.ToString() == "" ? string.Empty : rdo.ToString();
            Region = Region.ToString() == "" ? string.Empty : Region.ToString();

            FromMn = (Convert.ToInt32(FromMn) < 10) ? '0' + FromMn : FromMn;

            string wHdr_YrMth_FR = FromMn + "-" + FromYr;
            string wYrMth_FR = Convert.ToString(FromYr + FromMn);
            string wHdr_YrMth_TO = "";
            string wYrMth_TO = "";
            string wRegion = "";

            if (Region == "N")
            {
                wRegion = "Northern Region";
            }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            int ctr = 60;
            string first_page = "Y";

            if (rdo == "M")
            {
                wHdr_YrMth_TO = FromMn + "-" + FromYr;
                wYrMth_TO = Convert.ToString(FromYr + FromMn);
            }
            else if (rdo == "P")
            {
                wHdr_YrMth_TO = ToMn + "-" + ToYr;
                wYrMth_TO = Convert.ToString(ToYr + ToMn);
            }

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("wYrMth_FR", OracleDbType.Varchar2, wYrMth_FR, ParameterDirection.Input);
            par[1] = new OracleParameter("wYrMth_TO", OracleDbType.Varchar2, wYrMth_TO, ParameterDirection.Input);
            par[2] = new OracleParameter("pRegion", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[3] = new OracleParameter("ref_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_BILLRAISED_SUMMARY", par, 1);
            DataTable dt = ds.Tables[0];


            BillRaisedModel model = new();
            List<BillRaisedModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                //string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                //model = JsonConvert.DeserializeObject<List<BillRegisterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();

                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<BillRaisedModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.BPO_ORGN).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;

            dTResult.data = query.Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
