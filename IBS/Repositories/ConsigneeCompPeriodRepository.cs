using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories
{
    public class ConsigneeCompPeriodRepository : IConsigneeCompPeriodRepository
    {
        private readonly ModelContext context;

        public ConsigneeCompPeriodRepository(ModelContext context)
        {
            this.context = context;
        }

        public ConsigneeCompPeriodReport GetCompPeriodData(string FromDate, string ToDate, string Allregion, string regionorth, string regionsouth, string regioneast, string regionwest, string jiallregion,
            string jinorth, string jisourth, string jieast, string jiwest, string compallregion, string compyes, string compno, string cancelled, string underconsider, string allaction, string particilaraction, string actiondrp)
        {
            ConsigneeCompPeriodReport model = new();
            List<IEPerformanceListModel> lstPerformance = new();
            List<IEPerformanceSummaryListModel> lstPerformanceSummaryList = new();

            //model.FromDate = FromDate;
            //model.ToDate = ToDate;
            //model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            //OracleParameter[] parameter = new OracleParameter[5];
            //parameter[0] = new OracleParameter("p_FROM_DT", OracleDbType.Date, FromDate, ParameterDirection.Input);
            //parameter[1] = new OracleParameter("p_TO_DT", OracleDbType.Date, ToDate, ParameterDirection.Input);
            //parameter[2] = new OracleParameter("p_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            //parameter[3] = new OracleParameter("p_result_cursor1", OracleDbType.RefCursor, ParameterDirection.Output);
            //parameter[4] = new OracleParameter("p_result_cursor2", OracleDbType.RefCursor, ParameterDirection.Output);

            //DataSet ds = DataAccessDB.GetDataSet("SP_GET_IE_PERFORMANCE_DETAILS", parameter);

            //if (ds != null && ds.Tables.Count > 0)
            //{
            //    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            //    lstPerformance = JsonConvert.DeserializeObject<List<IEPerformanceListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            //    lstPerformance.ToList().ForEach(i =>
            //    {
            //        i.C3 = decimal.Truncate(i.C3);
            //        i.C7 = decimal.Truncate(i.C7);
            //        i.CM7 = decimal.Truncate(i.CM7);
            //        i.C10 = decimal.Truncate(i.C10);
            //        i.C0 = decimal.Truncate(i.C0);
            //        i.CALLS = decimal.Truncate(i.CALLS);
            //        i.CALL_CANCEL = decimal.Truncate(i.CALL_CANCEL);
            //        i.REJECTIONS = decimal.Truncate(i.REJECTIONS);
            //    });

            //    model.RejectionsIssued = Convert.ToInt32(ds.Tables[1].Rows[0]["REJECTIONS_ISSUED"]);
            //    model.TotalICs = Convert.ToInt32(ds.Tables[1].Rows[0]["TOTAL_IC"]);
            //    model.CallsAttendedWithin7Days = Convert.ToInt32(ds.Tables[1].Rows[0]["CALLS_ATTENDED_WITHIN"]);
            //    model.CallsAttendedBeyond7Days = Convert.ToInt32(ds.Tables[1].Rows[0]["CALLS_ATTENDED_BEYOND"]);
            //}

            //model.lstPerformance = lstPerformance;

            //model.lstPerformanceSummaryList = (from t20 in context.T20Ics
            //                                   join t13 in context.T13PoMasters on t20.CaseNo equals t13.CaseNo
            //                                   join t22 in context.T22Bills on t20.BillNo equals t22.BillNo
            //                                   where t20.CaseNo.Substring(0, 1) == Region && (t22.BillDt >= FromDate && t22.BillDt <= ToDate)
            //                                   select new IEPerformanceSummaryListModel
            //                                   {
            //                                       RLY_NONRLY = t13.RlyNonrly == "R" ? "Railway Inspections" : "Non-Railway Inspections",
            //                                       IC_COUNT = 1,
            //                                       MATERIAL_VALUE = t22.MaterialValue ?? 0
            //                                   }
            //                                  ).GroupBy(group => group.RLY_NONRLY).Select(x => new IEPerformanceSummaryListModel { RLY_NONRLY = x.Key, IC_COUNT = x.Count(), MATERIAL_VALUE = x.Sum(x => x.MATERIAL_VALUE) }).OrderByDescending(x => x.RLY_NONRLY).ToList();

            return model;
        }
    }
}
