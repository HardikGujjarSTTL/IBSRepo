using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.Reports
{
    public class RemitanceReportsRepository : IRemitanceReportsRepository
    {
        private readonly ModelContext context;

        public RemitanceReportsRepository(ModelContext context)
        {
            this.context = context;
        }

        public RemitanceModel GetRemitanceReport(DateTime FromDate, DateTime ToDate,string AccCode, string Region)
        {
            RemitanceModel model = new();
            List<RemitanceListModel> lstRemitance = new();
            List<RemitanceBillWisePeriodListModel> lstRemitanceBillWisePeriod = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            string FDt = FromDate.ToString("dd/MM/yyyy");
            string TDt = ToDate.ToString("dd/MM/yyyy");

            OracleParameter[] parameter = new OracleParameter[6];
            parameter[0] = new OracleParameter("p_FrmDt", OracleDbType.Varchar2, FDt, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_ToDt", OracleDbType.Varchar2, TDt, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_ACC_CD", OracleDbType.Varchar2, AccCode, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[4] = new OracleParameter("p_Result1", OracleDbType.RefCursor, ParameterDirection.Output);
            parameter[5] = new OracleParameter("p_Result1", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_REMITANCE_CHECKWISE", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                if(ds.Tables[0].Rows.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstRemitance = JsonConvert.DeserializeObject<List<RemitanceListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    lstRemitance.ToList().ForEach(i =>
                    {
                        i.VCHR_NO = Convert.ToString(i.VCHR_NO);
                        i.BANK_NAME = Convert.ToString(i.BANK_NAME);
                        i.CHQ_NO = Convert.ToString(i.CHQ_NO);
                        i.CHQ_DT = Convert.ToDateTime(i.CHQ_DT);
                        i.AMOUNT = Convert.ToDecimal(i.AMOUNT);
                        i.ACC_CD = Convert.ToInt32(i.ACC_CD);
                        i.CASE_NO = Convert.ToString(i.CASE_NO);
                        i.VCHR_DT = Convert.ToDateTime(i.VCHR_DT);
                        i.NARRATION = Convert.ToString(i.NARRATION);
                        i.BPO = Convert.ToString(i.BPO);
                    });
                    model.ACC_CD = Convert.ToString(ds.Tables[0].Rows[0]["ACC_CD"]);
                    model.ACC_DESC = Convert.ToString(ds.Tables[0].Rows[0]["ACC_DESC"]);
                }
                else if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
                    lstRemitanceBillWisePeriod = JsonConvert.DeserializeObject<List<RemitanceBillWisePeriodListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    lstRemitanceBillWisePeriod.ToList().ForEach(i =>
                    {
                        i.VCHR_NO = Convert.ToString(i.VCHR_NO);
                        i.VCHR_DT = Convert.ToDateTime(i.VCHR_DT);
                        i.BANK_NAME = Convert.ToString(i.BANK_NAME);
                        i.CHQ_NO = Convert.ToString(i.CHQ_NO);
                        i.CHQ_DT = Convert.ToDateTime(i.CHQ_DT);
                        i.CHQ_AMT = Convert.ToDecimal(i.CHQ_AMT);
                        i.ACC_CD = Convert.ToInt32(i.ACC_CD);
                        i.BILL_AMT = Convert.ToDecimal(i.BILL_AMT);
                        i.AMT_CLEARED = Convert.ToDecimal(i.AMT_CLEARED);
                        i.EXCESSORSHORT = Convert.ToDecimal(i.EXCESSORSHORT);
                        i.BILL_NO = Convert.ToString(i.BILL_NO);
                        i.POSTING_DT = Convert.ToDateTime(i.POSTING_DT);
                        i.BILL_DT = Convert.ToDateTime(i.BILL_DT);
                        i.BPO = Convert.ToString(i.BPO);
                    });
                }
            }
            

            model.lstRemitance = lstRemitance;
            model.lstRemitanceBillWisePeriod = lstRemitanceBillWisePeriod;

            return model;
        }
    }
}
