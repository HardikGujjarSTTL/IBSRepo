using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories
{
    public class ClientMasterRepository : IClientMasterRepository
    {
        private readonly ModelContext context;

        public ClientMasterRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<Clientmaster> GetClientList(DTParameters dtParameters)
        {
            DTResult<Clientmaster> dTResult = new() { draw = 0 };
            IQueryable<Clientmaster>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "USER_NAME";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "USER_NAME";
                orderAscendingDirection = true;
            }

            var clientname = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["clientname"]))
            {
                clientname = Convert.ToString(dtParameters.AdditionalValues["clientname"]);
            }

            DataTable dt = new DataTable();
            DataSet ds;
          
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_clientname", OracleDbType.Varchar2, clientname, ParameterDirection.Input);
            par[1] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("GetClientInfo", par, 1);
           
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];

                List<Clientmaster> list = dt.AsEnumerable().Select(row => new Clientmaster
                {
                    USER_NAME = row.Field<string>("USER_NAME"),
                    ORGANISATION = row.Field<string>("ORGANISATION"),
                    DESIGNATION = row.Field<string>("DESIGNATION"),
                    MOBILE = row.Field<string>("MOBILE"),
                    EMAIL = row.Field<string>("EMAIL"),
                    UNIT = row.Field<string>("UNIT"),
                }).ToList();

                query = list.AsQueryable();

                dTResult.recordsTotal = ds.Tables[0].Rows.Count;

                if (!string.IsNullOrEmpty(searchBy))
                    query = query.Where(w => Convert.ToString(w.USER_NAME).ToLower().Contains(searchBy.ToLower())
                    || Convert.ToString(w.ORGANISATION).ToLower().Contains(searchBy.ToLower())
                    );

                dTResult.recordsFiltered = query.Count();

                dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

                dTResult.draw = dtParameters.Draw;

            }
            else
            {
                return dTResult;
            }
            return dTResult;
        }

        public int ClientDetailsInsertUpdate(Clientmaster model)
        {
            int ClientId = 0;
            //var Client = (from r in context.T32ClientLogins where r.ID == Convert.ToInt32(model.Id) select r).FirstOrDefault();
            #region Client save
            //if (Client == null)
            //{
            //    T32ClientLogin obj = new T32ClientLogin();
            //    obj.UserName = model.USER_NAME;
            //    obj.Organisation = model.ORGANISATION;
            //    obj.Designation = model.DESIGNATION;
            //    obj.Email = model.EMAIL;
            //    obj.Mobile = model.MOBILE;
            //    obj.Isdeleted = Convert.ToByte(false);
            //    obj.Createdby = model.Createdby;
            //    obj.Createddate = DateTime.Now;
            //    context.T32ClientLogins.Add(obj);
            //    context.SaveChanges();
            //    ClientId = Convert.ToInt32(obj.ID);
            //}
            //else
            //{
            //    role.Rolename = model.Rolename;
            //    role.Roledescription = model.Roledescription;
            //    role.Issysadmin = Convert.ToByte(model.Issysadmin);
            //    role.Isactive = Convert.ToByte(model.Isactive);
            //    role.Isdeleted = Convert.ToByte(false);
            //    role.Updatedby = model.Updatedby;
            //    role.Updateddate = DateTime.Now;
            //    context.SaveChanges();
            //    RoleId = Convert.ToInt32(role.RoleId);
            //}
            #endregion
            return ClientId;
        }
    }
}
