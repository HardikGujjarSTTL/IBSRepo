using IBSAPI.DataAccess;
using IBSAPI.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBSAPI.Helper
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                context.Database.CloseConnection();
            }
            return ds;
        }

        public static DataSet GetDataSet(string procedurename, OracleParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = procedurename;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    
                    var reader = command.ExecuteReader();
                    do
                    {
                        var tb = new DataTable();
                        tb.Load(reader);
                        ds.Tables.Add(tb);

                    } while (!reader.IsClosed);
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }

            return ds;
        }
    }
}
