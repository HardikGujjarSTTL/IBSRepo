using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories
{
    public class NonRlyClientMasterRepository : INonRlyClientMasterRepository
    {
        private readonly ModelContext context;

        public NonRlyClientMasterRepository(ModelContext context)
        {
            this.context = context;
        }
        public int ClientDetailsInsertUpdate(Clientmaster model)
        {
            int maxID = 0;
            int NonClientId = 0;
            var NonClient = (from r in context.NonRlyClients where r.Id == Convert.ToInt32(model.Id) select r).FirstOrDefault();
            var existingClient = context.NonRlyClients.FirstOrDefault(r => r.Clientname == model.ClientName && r.Shortcode == model.ShortCode && r.Orgntype == model.Orgn_Type);

            if (existingClient == null || existingClient.Id == Convert.ToInt32(model.Id))
            {
                #region Non Client save
                if (NonClient == null)
                {
                    if (context.NonRlyClients.Any())
                    {
                        maxID = context.NonRlyClients.Max(x => x.Id) + 1;
                    }
                    else
                    {
                        maxID = 1;
                    }
                    NonRlyClient obj = new NonRlyClient();
                    obj.Id = maxID;
                    obj.Clientname = model.ClientName;
                    obj.Contactdesignation = model.Client_DESIGNATION;
                    obj.Emailid = model.EMAIL;
                    obj.Mobileno = model.MOBILE;
                    obj.Orgntype = model.Orgn_Type;
                    obj.Shortcode = model.ShortCode;
                    obj.Contactname = model.ContactName;
                    obj.Isdeleted = Convert.ToByte(false);
                    obj.Createdby = model.Createdby;
                    obj.Createddate = DateTime.Now;
                    context.NonRlyClients.Add(obj);
                    context.SaveChanges();
                    NonClientId = Convert.ToInt32(obj.Id);
                }
                else
                {
                    NonClient.Clientname = model.ClientName;
                    NonClient.Contactdesignation = model.Client_DESIGNATION;
                    NonClient.Emailid = model.EMAIL;
                    NonClient.Mobileno = model.MOBILE;
                    NonClient.Orgntype = model.Orgn_Type;
                    NonClient.Shortcode = model.ShortCode;
                    NonClient.Contactname = model.ContactName;
                    NonClient.Isdeleted = Convert.ToByte(false);
                    NonClient.Updatedby = model.Updatedby;
                    NonClient.Updateddate = DateTime.Now;
                    context.SaveChanges();
                    NonClientId = Convert.ToInt32(NonClient.Id);
                }
                #endregion
            }
            else
            {
                return NonClientId;
            }

            return NonClientId;
        }

        public DTResult<Clientmaster> GetNonClientList(DTParameters dtParameters)
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
                    orderCriteria = "CLIENTNAME";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "CLIENTNAME";
                orderAscendingDirection = true;
            }

            string clientname = "", ShortCode="";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["clientname"]))
            {
                clientname = Convert.ToString(dtParameters.AdditionalValues["clientname"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ShortCode"]))
            {
                ShortCode = Convert.ToString(dtParameters.AdditionalValues["ShortCode"]);
            }

            DataTable dt = new DataTable();
            DataSet ds;

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_clientname", OracleDbType.Varchar2, clientname, ParameterDirection.Input);
            par[1] = new OracleParameter("p_shortcode", OracleDbType.Varchar2, ShortCode, ParameterDirection.Input);
            par[2] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("GetNonClientInfo", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];

                List<Clientmaster> list = dt.AsEnumerable().Select(row => new Clientmaster
                {
                    ClientName = row.Field<string>("CLIENTNAME"),
                    ShortCode = row.Field<string>("SHORTCODE"),
                    ContactName = row.Field<string>("CONTACTNAME"),
                    Client_DESIGNATION = row.Field<string>("CONTACTDESIGNATION"),
                    MOBILE = row.Field<string>("MOBILENO"),
                    EMAIL = row.Field<string>("EMAILID"),
                    Orgn_Type = row.Field<string>("ORGNTYPE"),
                    Id = row.Field<int>("NonClientId"),
                }).ToList();

                query = list.AsQueryable();

                dTResult.recordsTotal = ds.Tables[0].Rows.Count;

                if (!string.IsNullOrEmpty(searchBy))
                    query = query.Where(w => Convert.ToString(w.ClientName).ToLower().Contains(searchBy.ToLower())
                    || Convert.ToString(w.ShortCode).ToLower().Contains(searchBy.ToLower())
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

        public Clientmaster FindNonClientByID(int ID)
        {
            Clientmaster model = new();
            NonRlyClient client = context.NonRlyClients.Find(ID);

            if (client == null)
                throw new Exception("Client Not found");
            else
            {
                model.ClientName = client.Clientname;
                model.ContactName = client.Contactname;
                model.ShortCode = client.Shortcode;
                model.Client_DESIGNATION = client.Contactdesignation;
                model.Orgn_Type = client.Orgntype;
                model.MOBILE = client.Mobileno;
                model.EMAIL = client.Emailid;
                return model;
            }
        }

        public bool Remove(int ID, int UserID)
        {
            var client = context.NonRlyClients.Find(ID);
            if (client == null) { return false; }
            client.Isdeleted = Convert.ToByte(true);
            client.Updatedby = UserID;
            client.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }
    }
}
