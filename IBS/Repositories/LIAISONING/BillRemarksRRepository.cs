using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class BillRemarksRRepository : IBillRemarksRepository
    {
        private readonly ModelContext context;

        public BillRemarksRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<BillRemarksModel> GetBills(DTParameters dtParameters, string OrgType, string Org, IWebHostEnvironment webHostEnvironment)
        {


            DTResult<BillRemarksModel> dTResult = new() { draw = 0 };
            IQueryable<BillRemarksModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;


            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "BILL_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "BILL_NO";
                orderAscendingDirection = true;
            }


            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_BPO_T", OracleDbType.NVarchar2, OrgType, ParameterDirection.Input);
            par[1] = new OracleParameter("P_BPO_R", OracleDbType.NVarchar2, Org, ParameterDirection.Input);
            par[2] = new OracleParameter("CUR_OUT", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_BILL_INFO", par, 2);

            List<BillRemarksModel> modelList = new List<BillRemarksModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    BillRemarksModel model = new BillRemarksModel
                    {
                        BILL_NO = Convert.ToString(row["bill_no"]),
                        BILL_AMOUNT = Convert.ToString(row["BILL_AMOUNT"]),
                        AU = Convert.ToString(row["AU"]),
                        LO_REMARKS = Convert.ToString(row["LO_REMARKS"]),
                        BPO_TYPE = Convert.ToString(row["BPO_TYPE"]),
                        BPO_RLY = Convert.ToString(row["BPO_RLY"]),

                    };

                    modelList.Add(model);
                }
            }

            //query = from l in context.Roles
            //        where l.Isdeleted == 0
            //        select new RoleModel
            //        {
            //            RoleId = l.RoleId,
            //            Rolename = l.Rolename,
            //            Roledescription = l.Roledescription,
            //            Issysadmin = Convert.ToBoolean(l.Issysadmin),
            //            Isactive = Convert.ToBoolean(l.Isactive),
            //            Isdeleted = l.Isdeleted,
            //            Createddate = l.Createddate,
            //            Createdby = l.Createdby,
            //            Updateddate = l.Updateddate,
            //            Updatedby = l.Updatedby
            //        };

            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.BILL_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.BILL_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            //dTResult.data = query.ToList();
            dTResult.draw = dtParameters.Draw;

            return dTResult;

            //using (var dbContext = context.Database.GetDbConnection())
            //{

            //}

            //return dTResult;
        }

        public bool SaveData(BillRemarksModel BillRemarksModel)
        {
            try
            {
                using (var conn = context.Database.GetDbConnection())
                {
                    conn.Open();

                    string query = "UPDATE T22_BILL SET LO_REMARKS = :LO_REMARKS WHERE BILL_NO = :BILL_NO";
                    using (var cmd = new OracleCommand(query, (OracleConnection)conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("LO_REMARKS", OracleDbType.NVarchar2)).Value = BillRemarksModel.LO_REMARKS;
                        cmd.Parameters.Add(new OracleParameter("BILL_NO", OracleDbType.NVarchar2)).Value = BillRemarksModel.BILL_NO;

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
