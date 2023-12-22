using Humanizer;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Interfaces.Reports.Billing;
using IBS.Models;
using IBS.Models.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using static IBS.Helper.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories.Reports
{
    public class PCDOReportRepository : IPCDOReportRepository
    {
        private readonly ModelContext context;

        public PCDOReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<HighlightReportsModel> GetHighlightData(string p_wYrMth)
        {
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_wYrMth", OracleDbType.Varchar2, p_wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_Highlights", par, 1);
            List<HighlightReportsModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<HighlightReportsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

        public COHighlightMainModel GetCOHighlightData(string p_CumYrMth, string p_wYrMth, string p_byear, int p_dmonth, string p_lstdate, string p_CumYrPast,string p_wYrMth_Past)
        {
            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("p_CumYrMth", OracleDbType.Varchar2, p_CumYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_wYrMth", OracleDbType.Varchar2, p_wYrMth, ParameterDirection.Input);
            par[2] = new OracleParameter("p_byear", OracleDbType.Varchar2, p_byear, ParameterDirection.Input);
            par[3] = new OracleParameter("p_dmonth", OracleDbType.Int32, p_dmonth, ParameterDirection.Input);
            par[4] = new OracleParameter("p_lstdate", OracleDbType.Varchar2, p_lstdate, ParameterDirection.Input);
            par[5] = new OracleParameter("p_CumYrPast", OracleDbType.Varchar2, p_CumYrPast, ParameterDirection.Input);
            par[5] = new OracleParameter("p_wYrMth_Past", OracleDbType.Varchar2, p_wYrMth_Past, ParameterDirection.Input);
            par[6] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_CO_Highlights", par, 2);
            COHighlightMainModel model = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model.cOHighlightModels = JsonConvert.DeserializeObject<List<COHighlightModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model.cOHighlight1Models = JsonConvert.DeserializeObject<List<COHighlight1Model>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return model;
        }

        public List<FinancialBillingModel> GetFinancialBillingData(int dmonth, string wYrMth_Past, string CumYrPast, string wYrMth, string CumYrMth, int byear)
        {
            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("dmonth", OracleDbType.Varchar2,Convert.ToString(dmonth), ParameterDirection.Input);
            par[1] = new OracleParameter("wYrMth_Past", OracleDbType.Varchar2, wYrMth_Past, ParameterDirection.Input);
            par[2] = new OracleParameter("CumYrPast", OracleDbType.Varchar2, CumYrPast, ParameterDirection.Input);
            par[3] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[4] = new OracleParameter("CumYrMth", OracleDbType.Varchar2, CumYrMth, ParameterDirection.Input);
            par[5] = new OracleParameter("byear", OracleDbType.Varchar2, Convert.ToString(byear), ParameterDirection.Input);
            par[6] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_FinancialBilling", par, 1);
            List<FinancialBillingModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<FinancialBillingModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }
        void process_realisations(string wYrMth)
        {
            

            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            try
            {
                var cmd = context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "REALISATIONALL";

                OracleParameter[] par = new OracleParameter[3];

                par[0] = new OracleParameter("IN_YR_MTH_FR", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
                par[1] = new OracleParameter("IN_YR_MTH_TO", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
                par[2] = new OracleParameter("OUT_ERR_CD", OracleDbType.Decimal, ParameterDirection.Output);
                par[2].DbType = DbType.Int32;

                cmd.Parameters.AddRange(par);

                context.Database.OpenConnection();
                cmd.ExecuteNonQuery();
                context.Database.CloseConnection();


            }
            catch (Exception ex)
            {
                context.Database.CloseConnection();
            }

        }
        void process_realisations1(string CumYrMth, string wYrMth)
        {


            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            try
            {
                var cmd = context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "REALISATIONALL";

                OracleParameter[] par = new OracleParameter[3];

                par[0] = new OracleParameter("IN_YR_MTH_FR", OracleDbType.Varchar2, CumYrMth, ParameterDirection.Input);
                par[1] = new OracleParameter("IN_YR_MTH_TO", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
                par[2] = new OracleParameter("OUT_ERR_CD", OracleDbType.Decimal, ParameterDirection.Output);
                par[2].DbType = DbType.Int32;

                cmd.Parameters.AddRange(par);

                context.Database.OpenConnection();
                cmd.ExecuteNonQuery();
                context.Database.CloseConnection();


            }
            catch (Exception ex)
            {
                context.Database.CloseConnection();
            }

        }
        public FinancialExpenditureRealizationMainModel GetFinancialExpenditureRealizationData(string wYrMth_Past, string CumYrPast, string wYrMth, string CumYrMth, int byear)
        {
            process_realisations(wYrMth);
            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("wYrMth_Past", OracleDbType.Varchar2, wYrMth_Past, ParameterDirection.Input);
            par[1] = new OracleParameter("CumYrPast", OracleDbType.Varchar2, CumYrPast, ParameterDirection.Input);
            par[2] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[3] = new OracleParameter("CumYrMth", OracleDbType.Varchar2, CumYrMth, ParameterDirection.Input);
            par[4] = new OracleParameter("byear", OracleDbType.Varchar2, Convert.ToString(byear), ParameterDirection.Input);
            par[5] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            par[6] = new OracleParameter("p_Result1", OracleDbType.RefCursor, ParameterDirection.Output);
            par[7] = new OracleParameter("p_Result2", OracleDbType.RefCursor, ParameterDirection.Output);
            par[8] = new OracleParameter("p_Result3", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_FinancialExpenditureRealization", par, 8);

            FinancialExpenditureRealizationMainModel model = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model.financialExpenditureRealizationModels = JsonConvert.DeserializeObject<List<FinancialExpenditureRealizationModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
                model.realisationModel = JsonConvert.DeserializeObject<List<RealisationModel>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                //string serializeddt2 = JsonConvert.SerializeObject(ds1.Tables[2], Formatting.Indented);
                //model.realisation1Model = JsonConvert.DeserializeObject<List<Realisation1Model>>(serializeddt2, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt3 = JsonConvert.SerializeObject(ds.Tables[3], Formatting.Indented);
                model.realisation2Model = JsonConvert.DeserializeObject<List<Realisation2Model>>(serializeddt3, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            }
            process_realisations1(CumYrMth, wYrMth);
            OracleParameter[] par1 = new OracleParameter[9];
            par1[0] = new OracleParameter("wYrMth_Past", OracleDbType.Varchar2, wYrMth_Past, ParameterDirection.Input);
            par1[1] = new OracleParameter("CumYrPast", OracleDbType.Varchar2, CumYrPast, ParameterDirection.Input);
            par1[2] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par1[3] = new OracleParameter("CumYrMth", OracleDbType.Varchar2, CumYrMth, ParameterDirection.Input);
            par1[4] = new OracleParameter("byear", OracleDbType.Varchar2, Convert.ToString(byear), ParameterDirection.Input);
            par1[5] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            par1[6] = new OracleParameter("p_Result1", OracleDbType.RefCursor, ParameterDirection.Output);
            par1[7] = new OracleParameter("p_Result2", OracleDbType.RefCursor, ParameterDirection.Output);
            par1[8] = new OracleParameter("p_Result3", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds1 = DataAccessDB.GetDataSet("sp_PCDOReport_FinancialExpenditureRealization", par1, 8);

            if (ds1 != null && ds1.Tables.Count > 0)
            {
                string serializeddt4 = JsonConvert.SerializeObject(ds1.Tables[2], Formatting.Indented);
                model.realisation1Model = JsonConvert.DeserializeObject<List<Realisation1Model>>(serializeddt4, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            }
            return model;
        }

        public FinancialOutstandingMainModel GetFinancialOutstandingData(string wYrMth_Past, string CumYrPast, string wYrMth, string CumYrMth, string bakdate, string lstdate)
        {
            OracleParameter[] par = new OracleParameter[8];
            par[0] = new OracleParameter("wYrMth_Past", OracleDbType.Varchar2, wYrMth_Past, ParameterDirection.Input);
            par[1] = new OracleParameter("CumYrPast", OracleDbType.Varchar2, CumYrPast, ParameterDirection.Input);
            par[2] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[3] = new OracleParameter("CumYrMth", OracleDbType.Varchar2, CumYrMth, ParameterDirection.Input);
            par[4] = new OracleParameter("bakdate", OracleDbType.Varchar2, bakdate, ParameterDirection.Input);
            par[5] = new OracleParameter("lstdate", OracleDbType.Varchar2, lstdate, ParameterDirection.Input);
            par[6] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            par[7] = new OracleParameter("p_Result1", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_FinancialOutstanding", par, 2);
            FinancialOutstandingMainModel model = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model.financialOutstandingModels = JsonConvert.DeserializeObject<List<FinancialOutstandingModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
                model.financialOutstanding1Models = JsonConvert.DeserializeObject<List<FinancialOutstanding1Model>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return model;
        }

        public List<EOIPricedOfferSentModel> GetEOIPricedOfferSentData(string wYrMth)
        {
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_EOIPricedOfferSent", par, 1);
            List<EOIPricedOfferSentModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<EOIPricedOfferSentModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

        public List<BDEffortsModel> GetBDEffortsData(string wYrMth)
        {
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_BDEfforts", par, 1);
            List<BDEffortsModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<BDEffortsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

        public List<EOIPricedOfferSentModel> GetPreviousOfferSentData(string wYrMth)
        {
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_PreviousOfferSent", par, 1);
            List<EOIPricedOfferSentModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<EOIPricedOfferSentModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

        public List<ProgressofChecksheetsModel> GetProgressofChecksheetsData(string wYrMth)
        {
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_ProgressofChecksheets", par, 1);
            List<ProgressofChecksheetsModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<ProgressofChecksheetsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

        public ComplaintsMainModel GetComplaintsData(string wYrMth)
        {
            OracleParameter[] par = new OracleParameter[10];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            par[2] = new OracleParameter("p_Result_jIdisposedComplaints", OracleDbType.RefCursor, ParameterDirection.Output);
            par[3] = new OracleParameter("p_Result_breakupComplaintsModels", OracleDbType.RefCursor, ParameterDirection.Output);
            par[4] = new OracleParameter("p_Result_othercasesComplaintsModels", OracleDbType.RefCursor, ParameterDirection.Output);
            par[5] = new OracleParameter("p_Result_long_PendingModels", OracleDbType.RefCursor, ParameterDirection.Output);
            par[6] = new OracleParameter("p_Result_cR_REJModels", OracleDbType.RefCursor, ParameterDirection.Output);
            par[7] = new OracleParameter("p_Result_othercasesComplaintsE", OracleDbType.RefCursor, ParameterDirection.Output);
            par[8] = new OracleParameter("p_Result_othercasesComplaintsW", OracleDbType.RefCursor, ParameterDirection.Output);
            par[9] = new OracleParameter("p_Result_othercasesComplaintsS", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_Complaints", par, 9);
            ComplaintsMainModel model = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model.complaintsModels = JsonConvert.DeserializeObject<List<ComplaintsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
                model.jIdisposedComplaintsModels = JsonConvert.DeserializeObject<List<JIdisposedComplaintsModel>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt2 = JsonConvert.SerializeObject(ds.Tables[2], Formatting.Indented);
                model.breakupComplaintsModels = JsonConvert.DeserializeObject<List<BreakupComplaintsModel>>(serializeddt2, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt3 = JsonConvert.SerializeObject(ds.Tables[3], Formatting.Indented);
                model.othercasesComplaintsModels = JsonConvert.DeserializeObject<List<OthercasesComplaintsModel>>(serializeddt3, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt4 = JsonConvert.SerializeObject(ds.Tables[4], Formatting.Indented);
                model.long_PendingModels = JsonConvert.DeserializeObject<List<Long_pendingModel>>(serializeddt4, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt5 = JsonConvert.SerializeObject(ds.Tables[5], Formatting.Indented);
                model.cR_REJModels = JsonConvert.DeserializeObject<List<CR_REJModel>>(serializeddt5, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                
                string serializeddt6 = JsonConvert.SerializeObject(ds.Tables[6], Formatting.Indented);
                model.othercasesComplaintsE = JsonConvert.DeserializeObject<List<OthercasesComplaintsModel>>(serializeddt6, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt7 = JsonConvert.SerializeObject(ds.Tables[7], Formatting.Indented);
                model.othercasesComplaintsW = JsonConvert.DeserializeObject<List<OthercasesComplaintsModel>>(serializeddt7, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt8 = JsonConvert.SerializeObject(ds.Tables[8], Formatting.Indented);
                model.othercasesComplaintsS = JsonConvert.DeserializeObject<List<OthercasesComplaintsModel>>(serializeddt8, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            }
            return model;
        }

        public List<QualityofInspectionModel> GetQualityofInspectionData(string wYrMth)
        {
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_QualityofInspection", par, 1);
            List<QualityofInspectionModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<QualityofInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

        public QualityofInspectionCentralMainModel GetQualityofInspectionCentralData(string wYrMth, string CumYrMth)
        {
            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("CumYrMth", OracleDbType.Varchar2, CumYrMth, ParameterDirection.Input);
            par[2] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            par[3] = new OracleParameter("p_Result_RSM", OracleDbType.RefCursor, ParameterDirection.Output);
            par[4] = new OracleParameter("p_Result_URM", OracleDbType.RefCursor, ParameterDirection.Output);
            par[5] = new OracleParameter("p_Result_JINDAL", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_QualityofInspectionCentral", par, 4);
            QualityofInspectionCentralMainModel model = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model.qualityofInspectionCentralModels = JsonConvert.DeserializeObject<List<QualityofInspectionCentralModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
                model.qualityofInspectionCentral_RSMModels = JsonConvert.DeserializeObject<List<QualityofInspectionCentral_RSMModel>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt2 = JsonConvert.SerializeObject(ds.Tables[2], Formatting.Indented);
                model.qualityofInspectionCentral_URMModels = JsonConvert.DeserializeObject<List<QualityofInspectionCentral_URMModel>>(serializeddt2, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt3 = JsonConvert.SerializeObject(ds.Tables[3], Formatting.Indented);
                model.qualityofInspectionCentral_JINDALModels = JsonConvert.DeserializeObject<List<QualityofInspectionCentral_JINDALModel>>(serializeddt3, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return model;
        }

        public ImprovementInQualityofServiceMainModel GetImprovementInQualityofServiceData(string wYrMth)
        {
            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            par[2] = new OracleParameter("p_Result1", OracleDbType.RefCursor, ParameterDirection.Output);
            par[3] = new OracleParameter("p_Result2", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_ImprovementofQualityofService", par, 3);
            ImprovementInQualityofServiceMainModel model = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model.improvementInQualityofServiceModels = JsonConvert.DeserializeObject<List<ImprovementInQualityofServiceModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
                model.improvementInQualityofService1Models = JsonConvert.DeserializeObject<List<ImprovementInQualityofService1Model>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt2 = JsonConvert.SerializeObject(ds.Tables[2], Formatting.Indented);
                model.improvementInQualityofService2Models = JsonConvert.DeserializeObject<List<ImprovementInQualityofService2Model>>(serializeddt2, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return model;
        }

        public List<OutstandingRailwaysModel> GetOutstandingRailwaysData(string wYrMth)
        {
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_OutstandingRailways", par, 1);
            List<OutstandingRailwaysModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<OutstandingRailwaysModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

        public List<OutstandingNonRailwaysModel> GetOutstandingNonRailwaysData(string wYrMth)
        {
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_OutstandingNonRailways", par, 1);
            List<OutstandingNonRailwaysModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<OutstandingNonRailwaysModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

        public Top5OutstandingRailwayNonRailwaysMainModel GetTop5OutstandingRailwayNonRailwaysData(string wYrMth)
        {
            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            par[2] = new OracleParameter("p_Result1", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_Top5OutstandingRailwayNonRailways", par, 2);
            Top5OutstandingRailwayNonRailwaysMainModel model = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model.top5OutstandingRailwayNonRailwaysModels = JsonConvert.DeserializeObject<List<Top5OutstandingRailwayNonRailwaysModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
                model.top5OutstandingRailwayNonRailways1Models = JsonConvert.DeserializeObject<List<Top5OutstandingRailwayNonRailways1Model>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return model;
        }

        public List<ClientContactModel> GetClientContactData(string wYrMth)
        {
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_ClientContact", par, 1);
            List<ClientContactModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<ClientContactModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

        public List<ClientContactModel> GetDFOVisitData(string wYrMth)
        {
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_DFOVisit", par, 1);
            List<ClientContactModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<ClientContactModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

        public TrainingMainModel GetTrainingData(string wYrMth,string CumYrMth)
        {
            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("CumYrMth", OracleDbType.Varchar2, CumYrMth, ParameterDirection.Input);
            par[2] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            par[3] = new OracleParameter("p_Result1", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_Training", par, 2);
            TrainingMainModel model = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model.trainingModels = JsonConvert.DeserializeObject<List<TrainingModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
                model.training1Models = JsonConvert.DeserializeObject<List<Training1Model>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return model;
        }

        public List<TechnicalReferencesModel> GetTechnicalReferencesData(string wYrMth)
        {
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_TechnicalReferences", par, 1);
            List<TechnicalReferencesModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<TechnicalReferencesModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

        public List<PCDOSummaryModel> GetPCDOSummaryData(string wYrMth, string CumYrMth, string byear, string dmonth)
        {
            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("CumYrMth", OracleDbType.Varchar2, CumYrMth, ParameterDirection.Input);
            par[2] = new OracleParameter("byear", OracleDbType.Varchar2, byear, ParameterDirection.Input);
            par[3] = new OracleParameter("dmonth", OracleDbType.Varchar2, dmonth, ParameterDirection.Input);
            par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_Summary", par, 1);
            List<PCDOSummaryModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<PCDOSummaryModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }
    }
}
