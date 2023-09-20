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

        public RemitanceModel GetRemitanceReport(DateTime FromDate, DateTime ToDate, string AccCode, string Region, string RReport, string BPOName, string ClientType, string ClientName)
        {
            RemitanceModel model = new();
            List<RemitanceListModel> lstRemitance = new();
            List<RemitanceBillWisePeriodListModel> lstRemitanceBillWisePeriod = new();
            List<RemitanceBillWiseCreatedBillListModel> lstRemitanceBillWiseCreatedBill = new();
            List<RemitanceChequeWiseBillListModel> lstRemitanceChequeWiseBill = new();
            List<RemitanceAccountCodeWiseListModel> lstRemitanceAccountCodeWise = new();
            List<RemitanceClientBPOWiseListModel> lstRemitanceClientBPOWise = new();
            List<RemitanceStatementExcessListModel> lstRemitanceStatementExcess = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            string FDt = FromDate.ToString("dd/MM/yyyy");
            string TDt = ToDate.ToString("dd/MM/yyyy");

            OracleParameter[] parameter = new OracleParameter[9];
            parameter[0] = new OracleParameter("p_FrmDt", OracleDbType.Varchar2, FDt, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_ToDt", OracleDbType.Varchar2, TDt, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_ACC_CD", OracleDbType.Varchar2, AccCode, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[4] = new OracleParameter("P_ReportType", OracleDbType.Varchar2, RReport, ParameterDirection.Input);
            parameter[5] = new OracleParameter("P_BPOName", OracleDbType.Varchar2, BPOName, ParameterDirection.Input);
            parameter[6] = new OracleParameter("P_ClientType", OracleDbType.Varchar2, ClientType, ParameterDirection.Input);
            parameter[7] = new OracleParameter("P_ClientName", OracleDbType.Varchar2, ClientName, ParameterDirection.Input);
            parameter[8] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_REMITANCE_CHECKWISE", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (RReport == "Report1")
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
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

                        model.lstRemitance = lstRemitance;
                    }
                }

                else if (RReport == "Report2")
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
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
                        model.lstRemitanceBillWisePeriod = lstRemitanceBillWisePeriod;
                    }
                }

                else if (RReport == "Report3")
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                        lstRemitanceBillWiseCreatedBill = JsonConvert.DeserializeObject<List<RemitanceBillWiseCreatedBillListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        lstRemitanceBillWiseCreatedBill.ToList().ForEach(i =>
                        {
                            i.BANK_NAME = Convert.ToString(i.BANK_NAME);
                            i.CHQ_NO = Convert.ToString(i.CHQ_NO);
                            i.CHQ_DT = Convert.ToDateTime(i.CHQ_DT);
                            i.CHQ_AMT = Convert.ToDecimal(i.CHQ_AMT);
                            i.BILL_AMT = Convert.ToDecimal(i.BILL_AMT);
                            i.AMT_CLEARED = Convert.ToDecimal(i.AMT_CLEARED);
                            i.EXCESSORSHORT = Convert.ToDecimal(i.EXCESSORSHORT);
                            i.ACC_CD = Convert.ToInt32(i.ACC_CD);
                            i.BILL_NO = Convert.ToString(i.BILL_NO);
                            i.POSTING_DT = Convert.ToDateTime(i.POSTING_DT);
                            i.BILL_DT = Convert.ToDateTime(i.BILL_DT);
                            i.BPO = Convert.ToString(i.BPO);
                        });
                        model.lstRemitanceBillWiseCreatedBill = lstRemitanceBillWiseCreatedBill;
                    }
                }

                else if (RReport == "Report4")
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                        lstRemitanceChequeWiseBill = JsonConvert.DeserializeObject<List<RemitanceChequeWiseBillListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        lstRemitanceChequeWiseBill.ToList().ForEach(i =>
                        {
                            i.BANK_NAME = Convert.ToString(i.BANK_NAME);
                            i.CHQ_NO = Convert.ToString(i.CHQ_NO);
                            i.CHQ_DT = Convert.ToDateTime(i.CHQ_DT);
                            i.AMOUNT = Convert.ToDecimal(i.AMOUNT);
                            i.BILL_NO = Convert.ToString(i.BILL_NO);
                            i.BILL_AMOUNT = Convert.ToDecimal(i.BILL_AMOUNT);
                            i.AMOUNT_CLEARED = Convert.ToDecimal(i.AMOUNT_CLEARED);
                            i.POSTING_DT = Convert.ToDateTime(i.POSTING_DT);
                        });
                        model.lstRemitanceChequeWiseBill = lstRemitanceChequeWiseBill;
                    }
                }

                else if (RReport == "Report5")
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                        lstRemitanceAccountCodeWise = JsonConvert.DeserializeObject<List<RemitanceAccountCodeWiseListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        lstRemitanceAccountCodeWise.ToList().ForEach(i =>
                        {
                            i.VCHR_NO = Convert.ToString(i.VCHR_NO);
                            i.VCHR_DT = Convert.ToDateTime(i.VCHR_DT);
                            i.BANK_NAME = Convert.ToString(i.BANK_NAME);
                            i.CHQ_NO = Convert.ToString(i.CHQ_NO);
                            i.CHQ_DT = Convert.ToDateTime(i.CHQ_DT);
                            i.AMOUNT = Convert.ToDecimal(i.AMOUNT);
                            i.SUSPENSE_AMT = Convert.ToDecimal(i.SUSPENSE_AMT);
                            i.NARRATION = Convert.ToString(i.NARRATION);
                            i.SAP_CUST_CD = Convert.ToString(i.SAP_CUST_CD);
                            i.BILL_NO = Convert.ToString(i.BILL_NO);
                            i.BILL_AMOUNT = Convert.ToDecimal(i.BILL_AMOUNT);
                            i.AMOUNT_CLEARED = Convert.ToDecimal(i.AMOUNT_CLEARED);
                            i.POSTING_DT = Convert.ToDateTime(i.POSTING_DT);
                            i.ACC_GROUP = Convert.ToString(i.ACC_GROUP);
                            i.RECIPIENT_GSTIN_NO = Convert.ToString(i.RECIPIENT_GSTIN_NO);
                            i.BILL_DT = Convert.ToDateTime(i.BILL_DT);
                            i.TDS_AMT = Convert.ToDecimal(i.TDS_AMT);
                            i.TDS_DT = Convert.ToDateTime(i.TDS_DT);
                        });
                        model.ACC_CD = Convert.ToString(ds.Tables[0].Rows[0]["ACC_CD"]);
                        model.ACC_DESC = Convert.ToString(ds.Tables[0].Rows[0]["ACC_DESC"]);
                        if (model.ACC_CD == null && model.ACC_DESC == null)
                        {
                            model.ACC_CD = "All";
                            model.ACC_DESC = "All";
                        }
                        model.lstRemitanceAccountCodeWise = lstRemitanceAccountCodeWise;
                    }
                }

                else if (RReport == "Report6")
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                        lstRemitanceClientBPOWise = JsonConvert.DeserializeObject<List<RemitanceClientBPOWiseListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        lstRemitanceClientBPOWise.ToList().ForEach(i =>
                        {
                            i.BANK_NAME = Convert.ToString(i.BANK_NAME);
                            i.CHQ_NO = Convert.ToString(i.CHQ_NO);
                            i.CHQ_DT = Convert.ToDateTime(i.CHQ_DT);
                            i.AMOUNT = Convert.ToDecimal(i.AMOUNT);
                            i.BILL_NO = Convert.ToString(i.BILL_NO);
                            i.BILL_DT = Convert.ToDateTime(i.BILL_DT);
                            i.BILL_AMOUNT = Convert.ToDecimal(i.BILL_AMOUNT);
                            i.AMOUNT_CLEARED = Convert.ToDecimal(i.AMOUNT_CLEARED);
                            i.POSTING_DT = Convert.ToDateTime(i.POSTING_DT);
                            i.BPO_CD = Convert.ToString(i.BPO_CD);
                            i.BPO = Convert.ToString(i.BPO);
                        });
                        model.ClientType = Convert.ToString(ds.Tables[0].Rows[0]["BPO_TYPE"]);
                        model.ClientName = Convert.ToString(ds.Tables[0].Rows[0]["BPO_RLY"]);
                        if (BPOName != null)
                            model.BPOName = Convert.ToString(ds.Tables[0].Rows[0]["BPO"]);

                        model.lstRemitanceClientBPOWise = lstRemitanceClientBPOWise;
                    }
                }

                else if (RReport == "Report7")
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                        lstRemitanceStatementExcess = JsonConvert.DeserializeObject<List<RemitanceStatementExcessListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        lstRemitanceStatementExcess.ToList().ForEach(i =>
                        {
                            i.BILL_NO = Convert.ToString(i.BILL_NO);
                            i.BILL_DT = Convert.ToDateTime(i.BILL_DT);
                            i.BILL_AMOUNT = Convert.ToDecimal(i.BILL_AMOUNT);
                            i.BILL_AMT_CLEARED = Convert.ToDecimal(i.BILL_AMT_CLEARED);
                            i.EXCESS_SHORT = Convert.ToDecimal(i.EXCESS_SHORT);
                            i.BPO = Convert.ToString(i.BPO);
                        });

                        model.lstRemitanceStatementExcess = lstRemitanceStatementExcess;
                    }
                }

            }
            return model;
        }
    }
}
