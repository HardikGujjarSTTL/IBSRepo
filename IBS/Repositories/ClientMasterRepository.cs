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
                    Id = row.Field<int>("ClientId"),
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
            var Client = (from r in context.T32ClientLogins where r.Id == Convert.ToInt32(model.Id) select r).FirstOrDefault();
            #region Client save
            if (Client == null)
            {
                int maxID = context.T32ClientLogins.Max(x => x.Id) + 1;
                T32ClientLogin obj = new T32ClientLogin();
                obj.Id = maxID;
                obj.UserName = model.USER_NAME;
                obj.Organisation = model.ORGANISATION;
                obj.Designation = model.DESIGNATION;
                obj.Email = model.EMAIL;
                obj.Mobile = model.MOBILE;
                obj.Unit = model.UNIT;
                obj.OrgnType = model.Orgn_Type;
                obj.Pwd = model.Pwd;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                context.T32ClientLogins.Add(obj);
                context.SaveChanges();
                ClientId = Convert.ToInt32(obj.Id);
            }
            else
            {
                Client.UserName = model.USER_NAME;
                Client.Organisation = model.ORGANISATION;
                Client.Designation = model.DESIGNATION;
                Client.Email = model.EMAIL;
                Client.Unit = model.UNIT;
                Client.Mobile = model.MOBILE;
                Client.OrgnType = model.Orgn_Type;
                Client.Pwd = model.Pwd;
                Client.Isdeleted = Convert.ToByte(false);
                Client.Updatedby = model.Updatedby;
                Client.Updateddate = DateTime.Now;
                context.SaveChanges();
                ClientId = Convert.ToInt32(Client.Id);
            }
            #endregion
            return ClientId;
        }

        public Clientmaster FindClientByID(int ID)
        {
            Clientmaster model = new();
            T32ClientLogin client = context.T32ClientLogins.Find(ID);

            if (client == null)
                throw new Exception("Client Not found");
            else
            {
                model.USER_NAME = client.UserName;
                model.ORGANISATION = client.Organisation;
                model.DESIGNATION = client.Designation;
                model.UNIT = client.Unit;
                model.Orgn_Type = client.OrgnType;
                model.MOBILE = client.Mobile;
                model.EMAIL = client.Email;
                model.Pwd = client.Pwd;
                model.ConfirmPassword = client.Pwd;
                return model;
            }
        }

        public bool Remove(int ID, int UserID)
        {
            var client = context.T32ClientLogins.Find(ID);
            if (client == null) { return false; }
            client.Isdeleted = Convert.ToByte(true);
            client.Updatedby = UserID;
            client.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }
    }
}
