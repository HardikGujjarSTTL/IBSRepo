using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories.Reports
{
    public class ManagementReportsRepository : IManagementReportsRepository
    {
        private readonly ModelContext context;

        public ManagementReportsRepository(ModelContext context)
        {
            this.context = context;
        }

        public IEPerformanceModel GetIEPerformanceData(DateTime FromDate, DateTime ToDate, string Region)
        {
            IEPerformanceModel model = new();
            List<IEPerformanceListModel> lstPerformance = new();
            List<IEPerformanceSummaryListModel> lstPerformanceSummaryList = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            OracleParameter[] parameter = new OracleParameter[5];
            parameter[0] = new OracleParameter("p_FROM_DT", OracleDbType.Date, FromDate, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_TO_DT", OracleDbType.Date, ToDate, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_result_cursor1", OracleDbType.RefCursor, ParameterDirection.Output);
            parameter[4] = new OracleParameter("p_result_cursor2", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_IE_PERFORMANCE_DETAILS", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstPerformance = JsonConvert.DeserializeObject<List<IEPerformanceListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                lstPerformance.ToList().ForEach(i =>
                {
                    i.C3 = decimal.Truncate(i.C3);
                    i.C7 = decimal.Truncate(i.C7);
                    i.CM7 = decimal.Truncate(i.CM7);
                    i.C10 = decimal.Truncate(i.C10);
                    i.C0 = decimal.Truncate(i.C0);
                    i.CALLS = decimal.Truncate(i.CALLS);
                    i.CALL_CANCEL = decimal.Truncate(i.CALL_CANCEL);
                    i.REJECTIONS = decimal.Truncate(i.REJECTIONS);
                });

                model.RejectionsIssued = Convert.ToInt32(ds.Tables[1].Rows[0]["REJECTIONS_ISSUED"]);
                model.TotalICs = Convert.ToInt32(ds.Tables[1].Rows[0]["TOTAL_IC"]);
                model.CallsAttendedWithin7Days = Convert.ToInt32(ds.Tables[1].Rows[0]["CALLS_ATTENDED_WITHIN"]);
                model.CallsAttendedBeyond7Days = Convert.ToInt32(ds.Tables[1].Rows[0]["CALLS_ATTENDED_BEYOND"]);
            }

            model.lstPerformance = lstPerformance;

            model.lstPerformanceSummaryList = (from t20 in context.T20Ics
                                               join t13 in context.T13PoMasters on t20.CaseNo equals t13.CaseNo
                                               join t22 in context.T22Bills on t20.BillNo equals t22.BillNo
                                               where t20.CaseNo.Substring(0, 1) == Region && (t22.BillDt >= FromDate && t22.BillDt <= ToDate)
                                               select new IEPerformanceSummaryListModel
                                               {
                                                   RLY_NONRLY = t13.RlyNonrly == "R" ? "Railway Inspections" : "Non-Railway Inspections",
                                                   IC_COUNT = 1,
                                                   MATERIAL_VALUE = t22.MaterialValue ?? 0
                                               }
                                              ).GroupBy(group => group.RLY_NONRLY).Select(x => new IEPerformanceSummaryListModel { RLY_NONRLY = x.Key, IC_COUNT = x.Count(), MATERIAL_VALUE = x.Sum(x => x.MATERIAL_VALUE) }).OrderByDescending(x => x.RLY_NONRLY).ToList();

            return model;
        }

        public ClusterPerformanceModel GetClusterPerformanceData(DateTime FromDate, DateTime ToDate, string Region)
        {
            ClusterPerformanceModel model = new();
            List<ClusterPerformanceListModel> lstPerformance = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            OracleParameter[] parameter = new OracleParameter[4];
            parameter[0] = new OracleParameter("p_FROM_DT", OracleDbType.Date, FromDate, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_TO_DT", OracleDbType.Date, ToDate, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_CLUSTER_PERFORMANCE_DETAILS", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstPerformance = JsonConvert.DeserializeObject<List<ClusterPerformanceListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                lstPerformance.ToList().ForEach(i =>
                {
                    i.C3 = decimal.Truncate(i.C3);
                    i.C7 = decimal.Truncate(i.C7);
                    i.CM7 = decimal.Truncate(i.CM7);
                    i.C10 = decimal.Truncate(i.C10);
                    i.C0 = decimal.Truncate(i.C0);
                    i.CALLS = decimal.Truncate(i.CALLS);
                    i.CALL_CANCEL = decimal.Truncate(i.CALL_CANCEL);
                    i.REJECTIONS = decimal.Truncate(i.REJECTIONS);
                });
            }

            model.lstPerformance = lstPerformance;

            return model;
        }

        public RWBSummaryModel GetRWBSummaryData(string FromYearMonth, string ToYearMonth)
        {
            RWBSummaryModel model = new();
            List<RWBSummaryListModel> lstRWBSummaryList = new();
            List<RBWSectorListModel> lstRBWSectorList = new();

            if (!string.IsNullOrEmpty(ToYearMonth))
            {
                model.FilterTitle = "The Period " + GetFilterTitle(FromYearMonth) + " to " + GetFilterTitle(ToYearMonth);
            }
            else
            {
                model.FilterTitle = "The Month of " + GetFilterTitle(FromYearMonth);
            }

            OracleParameter[] parameter = new OracleParameter[4];
            parameter[0] = new OracleParameter("p_FROM_DT", OracleDbType.Varchar2, FromYearMonth, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_TO_DT", OracleDbType.Varchar2, string.IsNullOrEmpty(ToYearMonth) ? FromYearMonth : ToYearMonth, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_result_cursor1", OracleDbType.RefCursor, ParameterDirection.Output);
            parameter[3] = new OracleParameter("p_result_cursor2", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_REGION_WISE_BILLING_SUMMARY", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstRWBSummaryList = JsonConvert.DeserializeObject<List<RWBSummaryListModel>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt2 = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
                lstRBWSectorList = JsonConvert.DeserializeObject<List<RBWSectorListModel>>(serializeddt2, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                lstRWBSummaryList.ToList().ForEach(i =>
                {
                    i.NR_FEE = decimal.Truncate(i.NR_FEE);
                    i.NR_TAX = decimal.Truncate(i.NR_TAX);
                    i.NR_BILL_AMT = decimal.Truncate(i.NR_BILL_AMT);
                    i.NR_BILLLS = decimal.Truncate(i.NR_BILLLS);
                    i.WR_FEE = decimal.Truncate(i.WR_FEE);
                    i.WR_TAX = decimal.Truncate(i.WR_TAX);
                    i.WR_BILL_AMT = decimal.Truncate(i.WR_BILL_AMT);
                    i.WR_BILLLS = decimal.Truncate(i.WR_BILLLS);
                    i.ER_FEE = decimal.Truncate(i.ER_FEE);
                    i.ER_TAX = decimal.Truncate(i.ER_TAX);
                    i.ER_BILL_AMT = decimal.Truncate(i.ER_BILL_AMT);
                    i.ER_BILLLS = decimal.Truncate(i.ER_BILLLS);
                    i.SR_FEE = decimal.Truncate(i.SR_FEE);
                    i.SR_TAX = decimal.Truncate(i.SR_TAX);
                    i.SR_BILL_AMT = decimal.Truncate(i.SR_BILL_AMT);
                    i.SR_BILLLS = decimal.Truncate(i.SR_BILLLS);
                    i.CR_FEE = decimal.Truncate(i.CR_FEE);
                    i.CR_TAX = decimal.Truncate(i.CR_TAX);
                    i.CR_BILL_AMT = decimal.Truncate(i.CR_BILL_AMT);
                    i.CR_BILLLS = decimal.Truncate(i.CR_BILLLS);
                });

                lstRBWSectorList.ToList().ForEach(i =>
                {
                    i.INSP_FEE = decimal.Truncate(i.INSP_FEE);
                    i.BILL_AMOUNT = decimal.Truncate(i.BILL_AMOUNT);
                    i.NO_OF_BILLLS = decimal.Truncate(i.NO_OF_BILLLS);
                });
            }

            model.lstRWBSummaryList = lstRWBSummaryList;
            model.lstRBWSectorList = lstRBWSectorList;

            return model;
        }

        public string GetFilterTitle(string YearMonth)
        {
            string filterTitle = string.Empty;
            if (!string.IsNullOrEmpty(YearMonth))
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(YearMonth.Substring(4, 2)));
                filterTitle = monthName + "-" + YearMonth.Substring(0, 4);
            }
            return filterTitle;
        }

    }
}

