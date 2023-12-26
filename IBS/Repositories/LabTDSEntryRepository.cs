using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class LabTDSEntryRepository : ILabTDSEntryRepository
    {
        private readonly ModelContext context;

        public LabTDSEntryRepository(ModelContext context)
        {
            this.context = context;
        }

        public LabTDSEntryModel SearchRegNo(string RegNo, string Region)
        {

            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.NVarchar2, RegNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_REGION", OracleDbType.NVarchar2, Region, ParameterDirection.Output);
                par[2] = new OracleParameter("p_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("SP_GET_SearchRegNo", par, 2);

                LabTDSEntryModel model = new();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    model = new LabTDSEntryModel
                    {
                        SampleRegNo = row["SAMPLE_REG_NO"] as string, // Replace "Property1" with the actual column name in the table
                        CaseNo = Convert.ToString(row["CASE_NO"]),
                        AmountReceived = Convert.ToString(row["AMT_REC"]),
                        TDSAmount = Convert.ToString(row["TDS"]),
                        TDSDate = Convert.ToString(row["TDS_DT"]),
                        TotalLabCharges = Convert.ToString(row["TOTAL_CHARGES"]),
                    };
                }
                //if (ds != null && ds.Tables.Count > 0)
                //{
                //    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                //    List<LabTDSEntryModel> modelList = JsonConvert.DeserializeObject<List<LabTDSEntryModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                //    model = modelList.FirstOrDefault();
                //    //model = JsonConvert.DeserializeObject<List<LabTDSEntryModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                //}


                return model;
            }
        }

        public bool SaveTDSEntry(string RegNo, string TDSAmt, string TDSDate)
        {
            try
            {
                using (var conn = context.Database.GetDbConnection())
                {
                    conn.Open();

                    string query = "UPDATE T50_LAB_REGISTER SET TDS = :TDS, TDS_DT = :TDS_DT WHERE SAMPLE_REG_NO = :SAMPLE_REG_NO";
                    using (var cmd = new OracleCommand(query, (OracleConnection)conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("TDS", OracleDbType.NVarchar2)).Value = TDSAmt;
                        cmd.Parameters.Add(new OracleParameter("TDS_DT", OracleDbType.Date)).Value = TDSDate;
                        cmd.Parameters.Add(new OracleParameter("SAMPLE_REG_NO", OracleDbType.NVarchar2)).Value = RegNo;

                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception if needed (e.g., log the error)
                // You can also throw the exception to propagate it to the caller if required
                return false; // Update failed
            }
        }

    }
}
