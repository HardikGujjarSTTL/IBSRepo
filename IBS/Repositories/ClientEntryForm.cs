using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class ClientEntryForm : IClientEntryForm
    {
        private readonly ModelContext context;

        public ClientEntryForm(ModelContext context)
        {
            this.context = context;
        }
        public ClientEntryFormModel FindByID(int Mobile)
        {
            ClientEntryFormModel model = new();
            T32ClientLogin role = context.T32ClientLogins.Find(Convert.ToByte(Mobile));

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.UserName = role.UserName;
                model.Organisation = role.Organisation;
                model.Designation = role.Designation;
                model.Mobile = role.Mobile;
                model.Email = role.Email;
                model.Unit = role.Unit;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<ClientEntryFormModel>GetClientEntryFormList(DTParameters dtParameters)
        {

            DTResult<ClientEntryFormModel> dTResult = new() { draw = 0 };
            IQueryable<ClientEntryFormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Mobile";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Mobile";
                orderAscendingDirection = true;
            }
            query = from l in context.T32ClientLogins
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new ClientEntryFormModel
                    {
                        UserName = l.UserName,
                        Organisation = l.Organisation,
                        Designation = l.Designation,
                        Mobile = l.Mobile,
                        Email = l.Email,
                        Unit = l.Unit,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Mobile).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.UserName).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int Mobile, int UserID)
        {
            var roles = context.T32ClientLogins.Find(Convert.ToByte(Mobile));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int ClientEntryFormDetailsInsertUpdate(ClientEntryFormModel model)
        {
            int RoleId = 0;
            var CEF = context.T32ClientLogins.Where(x => x.Mobile == model.Mobile).FirstOrDefault();
            #region Role save
            //if (CEF == null || CEF.Mobile == 0)
                if (CEF == null )
                {
                T32ClientLogin obj = new T32ClientLogin();

                obj.Organisation = model.Organisation;
                obj.Designation = model.Designation;
                obj.UserName = model.UserName;
                obj.Unit = model.Unit;
                obj.Email = model.Email;
                obj.Mobile = model.Mobile;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T32ClientLogins.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.Mobile);
            }
            else
            {
                CEF.Organisation = model.Organisation;
                CEF.Designation = model.Designation;
                CEF.UserName = model.UserName;
                CEF.Email = model.Email;
                CEF.Unit = model.Unit;
                CEF.Updatedby = model.Updatedby;
                CEF.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(CEF.Mobile);
            }
            #endregion
            return RoleId;
        }
    }

}