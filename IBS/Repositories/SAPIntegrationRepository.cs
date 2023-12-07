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
    }
}

