using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.OracleClient;
using System.Reflection;
using System.Security.Cryptography;

namespace IBS.Repositories
{
    public class HologramAccountalRepository: IHologramAccountalRepository
    {
        private readonly ModelContext context;
        private readonly IConfiguration configuration;
        public HologramAccountalRepository(ModelContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public DTResult<HologramAccountalModel> GetHologramAcountList(DTParameters dtParameters)
        {
            DTResult<HologramAccountalModel> dTResult = new() { draw = 0 };
            //IQueryable<HologramAccountalModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            OracleConnection con = new OracleConnection(configuration.GetConnectionString("DefaultConnection"));
            con.Open();
            //OracleDataAdapter adapter = new OracleDataAdapter("Select * from t02_users", con);
            OracleDataAdapter adapter = new OracleDataAdapter();
            
            OracleCommand cmd = con.CreateCommand();            
            
            cmd.CommandText = "SP_GET_HOLOGRAM_ACCOUNTAL";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("BK_NO", "3965");
            cmd.Parameters.Add("SET_NO", "");
            cmd.Parameters.Add("REGION", "N");
            cmd.Parameters.Add("RESULT_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            adapter.SelectCommand = cmd;
            DataTable ds = new DataTable();
            adapter.Fill(ds);

            con.Close();                                    

            var query = ds.AsEnumerable().AsQueryable().Cast<HologramAccountalModel>();
            dTResult.recordsTotal = query.Count();
            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }        
    }
}
