using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace IBS.Repositories
{
    public class SAPIntegrationRepository : ISAPIntegrationRepository
    {
        private readonly ModelContext context;

        public SAPIntegrationRepository(ModelContext context)
        {
            this.context = context;
        }

        public DataSet ExportExcelBPO(string BPO_Cd)
        {
            OracleConnection conn = (OracleConnection)context.Database.GetDbConnection();
            conn.Open();
            int ID = 0;

            using (OracleCommand cmd = new OracleCommand("CREATE_SAP_QA_BPO", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("pBPO_CD", OracleDbType.Char).Value = BPO_Cd;
                cmd.Parameters.Add("pID", OracleDbType.Int64, 13).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                ID = (int)((OracleDecimal)cmd.Parameters["pID"].Value);
            }

            conn.Close();

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("P_ID", OracleDbType.Int64, ID, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            return DataAccessDB.GetDataSet("SP_GET_SAP_QA_DATA", par);
        }

        public DataSet ExportExcelSelectiveBPO(string BPO_Cd)
        {
            OracleConnection conn = (OracleConnection)context.Database.GetDbConnection();
            conn.Open();
            int ID = 0;

            using (OracleCommand cmd = new OracleCommand("CREATE_SAP_QA_BPO_SELECTIVE", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("pBPO_CD", OracleDbType.Char).Value = BPO_Cd;
                cmd.Parameters.Add("pID", OracleDbType.Int64, 13).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                ID = (int)((OracleDecimal)cmd.Parameters["pID"].Value);
            }

            conn.Close();

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("P_ID", OracleDbType.Int64, ID, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            return DataAccessDB.GetDataSet("SP_GET_SAP_QA_DATA", par);
        }
        public DataSet ExportExcelConsigneSelect(string BPO_Cd)
        {
            OracleConnection conn = (OracleConnection)context.Database.GetDbConnection();
            conn.Open();
            int ID = 0;

            using (OracleCommand cmd = new OracleCommand("CREATE_SAP_QA_CONSIGNE_SELECT", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("pCONSIGNEE", OracleDbType.Char).Value = BPO_Cd;
                cmd.Parameters.Add("pID", OracleDbType.Int64, 13).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                ID = (int)((OracleDecimal)cmd.Parameters["pID"].Value);
            }

            conn.Close();

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("P_ID", OracleDbType.Int64, ID, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            return DataAccessDB.GetDataSet("SP_GET_SAP_QA_CONSIGNE_SELECT", par);
        }

        public int UpdateBPO(DataSet ds)
        {
            int id = 0;
            foreach (DataTable dataTable in ds.Tables)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row.Table.Columns.Contains("SORT2_BPO_CD") && row["SORT2_BPO_CD"] != null && row["SORT2_BPO_CD"].ToString() != "")
                    {
                        var t12Bpo = (from m in context.T12BillPayingOfficers
                                      where m.BpoCd == row["SORT2_BPO_CD"].ToString()
                                      select m).FirstOrDefault();
                        if (t12Bpo != null)
                        {
                            if (row.Table.Columns.Contains("SapCustCdBpo") && row["SapCustCdBpo"] != null && row["SapCustCdBpo"].ToString() != "")
                            {
                                t12Bpo.SapCustCdBpo = row["SapCustCdBpo"].ToString();
                                context.SaveChanges();
                                id = 1;
                            }
                        }
                    }
                }
            }
            return id;
        }
        public int UpdateConsigne(DataSet ds)
        {
            int id = 0;
            foreach (DataTable dataTable in ds.Tables)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row.Table.Columns.Contains("SORT2_CONSIGNEE_CD") && row["SORT2_CONSIGNEE_CD"] != null && row["SORT2_CONSIGNEE_CD"].ToString() != "")
                    {
                        var t12Bpo = (from m in context.T06Consignees
                                      where m.ConsigneeCd == Convert.ToInt32(row["SORT2_CONSIGNEE_CD"].ToString())
                                      select m).FirstOrDefault();
                        if (t12Bpo != null)
                        {
                            if (row.Table.Columns.Contains("SapCustCdCon") &&  row["SapCustCdCon"] != null && row["SapCustCdCon"].ToString() != "")
                            {
                                t12Bpo.SapCustCdCon = row["SapCustCdCon"].ToString();
                                context.SaveChanges();
                                id = 1;
                            }
                        }
                    }
                }
            }
            return id;
        }
    }
}

