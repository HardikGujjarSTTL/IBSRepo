using IBS.DataAccess;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Helper
{
    public static class DataAccessDB
    {
        public static DataSet GetDataSet(string procedurename, OracleParameter[] par, int Tablecount)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            DataSet ds = new DataSet();
            try
            {
                var cmd = context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procedurename;

                if (par != null && par.Length > 0)
                {
                    foreach (var item in par)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                context.Database.OpenConnection();

                string[] tableNames = new string[Tablecount];
                for (int i = 0; i < Tablecount; i++)
                {
                    DataTable dt = new DataTable();
                    dt.TableName = "Table" + i.ToString();
                    tableNames[i] = dt.TableName;
                }

                using (var reader = cmd.ExecuteReader())
                {
                    ds.Load(reader, LoadOption.OverwriteChanges, tableNames);
                }
                context.Database.CloseConnection();

            }
            catch (Exception)
            {
                context.Database.CloseConnection();
            }
            return ds;
        }

        public static DataSet ExecuteNonQuery(string procedurename, OracleParameter[] par, int Tablecount)
        {
            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            DataSet ds = new DataSet();
            try
            {
                var cmd = context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procedurename;

                if (par != null && par.Length > 0)
                {
                    foreach (var item in par)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                context.Database.OpenConnection();
                cmd.ExecuteNonQuery();
                context.Database.CloseConnection();
            }
            catch (Exception)
            {
                context.Database.CloseConnection();
            }
            return ds;
        }
    }
}
